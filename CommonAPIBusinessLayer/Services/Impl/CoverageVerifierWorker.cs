using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon.Dto;

namespace CommonAPIBusinessLayer.Services.Impl
{
    public class CoverageVerifierWorker : ICoverageVerifierWorker
    {
        private static IEnumerable<string> AlfaCarrierNames { get; set; }

        private IList<PolicyDataDto> PolicyDatas { get; set; }
        private IList<PolicyCoverageIntervalDto> PolicyCoverageIntervals { get; set; }

        public DateTime LastExpirationDate { get; private set; }
        public int LaspseDays { get; private set; }
        public bool? PolicyIsStandard { get; private set; }
        public int? IndividualLimit { get; private set; }
        public int? OccurranceLimit { get; private set; }
        public string PolicyState { get; private set; }
        public bool LatestIsAvic { get; private set; }
        public bool LatestIsAsic { get; private set; }
        public bool LatestIsHS { get; private set; }

        static CoverageVerifierWorker()
        {
            AlfaCarrierNames = new List<string>()
            {
                "ALFA SPECIALTY",
                "ALFA VISION",
                "HOMESTATE",
                "TREXIS",
                "TREXIS ONE"
            };
        }

        public CoverageVerifierWorker(IList<PolicyDataDto> policyDatas, IList<PolicyCoverageIntervalDto> policyCoverageIntervals)
        {
            PolicyDatas = policyDatas;
            PolicyCoverageIntervals = policyCoverageIntervals;
        }

        public void Solve(string policyHolderName, string carrierName, DateTime effectiveDate)
        {
            PolicyDataDto policyInfo = PolicyDatas.Where(p => p.PolicySubjects.Select(ps => ps.Name).Contains(policyHolderName))
                .OrderByDescending(p => p.LastReportedTermExpirationDate).FirstOrDefault();

            PolicyDataDto mostRecentPolicy = this.PolicyDatas.OrderByDescending(p => p.LastReportedTermExpirationDate).FirstOrDefault();
            if (mostRecentPolicy != null)
            {
                LatestIsAsic = false;
                LatestIsAvic = false;
                LatestIsHS = false;
                if (mostRecentPolicy.CarrierName.ToUpper().Contains(System.Configuration.ConfigurationManager.AppSettings["CompanyNameAlfaSpecialty"]) || mostRecentPolicy.CarrierName.ToUpper().Contains(System.Configuration.ConfigurationManager.AppSettings["CompanyNameTrexisOne"]))
                {
                    LatestIsAsic = true;
                }
                else if (mostRecentPolicy.CarrierName.ToUpper().Contains(System.Configuration.ConfigurationManager.AppSettings["CompanyNameAlfaVision"]) || mostRecentPolicy.CarrierName.ToUpper().Contains(System.Configuration.ConfigurationManager.AppSettings["CompanyNameTrexis"]))
                {
                    LatestIsAvic = true;
                }
                else if (mostRecentPolicy.CarrierName.ToUpper().Contains(System.Configuration.ConfigurationManager.AppSettings["CompanyNameHomestate"]))
                {
                    LatestIsHS = true;
                }
            }

            if (policyInfo != null)
            {
                PolicyIsStandard = policyInfo.IsStandardPolicy;
                IndividualLimit = policyInfo.BIIndividualLimit;
                OccurranceLimit = policyInfo.BIOccuranceLimit;
                PolicyState = policyInfo.PolicyState;
                LastExpirationDate = policyInfo.LastReportedTermExpirationDate;
            }

            // We want to look at the date range priorDate ("cvDays" before EffectiveDate) to EffectiveDate
            int cvDays = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CVDaysPriorToEffectiveDate"]);
            DateTime priorDate = effectiveDate.AddDays(-cvDays);

            // Get CoverageIntervals that exist during date range
            IOrderedEnumerable<PolicyCoverageIntervalDto> orderedIntervals = this.PolicyCoverageIntervals.Where(p => p.ToDate >= priorDate && p.FromDate <= effectiveDate).OrderByDescending(p => p.FromDate);
            if (!orderedIntervals.Any())
            {
                LaspseDays = 999;
                return;
            }

            PolicyCoverageIntervalDto latest = orderedIntervals.First();

            // Modify check of CarrierName for new company name (Trexis)
            string latestCarrierName = latest.Company.ToUpper();
            if (latest.Company.ToUpper().Contains(System.Configuration.ConfigurationManager.AppSettings["CompanyNameAlfaSpecialty"]))
            {
                latestCarrierName = System.Configuration.ConfigurationManager.AppSettings["CompanyNameTrexisOne"];
            }
            else if (latest.Company.ToUpper().Contains(System.Configuration.ConfigurationManager.AppSettings["CompanyNameAlfaVision"]))
            {
                latestCarrierName = System.Configuration.ConfigurationManager.AppSettings["CompanyNameTrexis"];
            }
            else if (latest.Company.ToUpper().Contains(System.Configuration.ConfigurationManager.AppSettings["CompanyNameHomestate"]))
            {
                latestCarrierName = System.Configuration.ConfigurationManager.AppSettings["CompanyNameHomestate"];
            }

            // If the latest interval is from one of our policies with the same carrier, it does not qualify for prior coverage.
            if (latestCarrierName.Equals(carrierName, StringComparison.InvariantCultureIgnoreCase))
            {
                LaspseDays = 999;
                return;
            }

            int totalLapse = 0;
            int totalCoverage = 0;

            // If the latest CoverageInterval extends to the EffectiveDate or beyond, there was no lapse between it and effective.  Otherwise, get the gap between them.
            int dayDifference = Math.Abs((effectiveDate - latest.ToDate).Days);
            if (latest.ToDate < effectiveDate)
            {
                totalLapse = dayDifference;
            }

            // Loop through the CoverageIntervals until prior insurance (based on cvDays) is met.
            foreach (PolicyCoverageIntervalDto row in orderedIntervals)
            {
                int tempCoverage = row.CoverageDays;
                // row.CoverageDays could extend beyond the priorDate and/or effectiveDate.  Don't count the days outside of those dates.
                if (row.ToDate > effectiveDate)
                {
                    tempCoverage = tempCoverage - (row.ToDate - effectiveDate).Days;
                }
                if (row.FromDate < priorDate)
                {
                    tempCoverage = tempCoverage - (priorDate - row.FromDate).Days;
                }
                totalCoverage += tempCoverage;

                // Once we meet the prior insurance (based on cvDays), no need to continue.
                if (totalCoverage >= cvDays - totalLapse)
                {
                    LaspseDays = totalLapse;
                    return;
                }

                // row.LapsedDays could extend beyond the priorDate.  Don't allow it to be greater than the number of days between priorDate and row.FromDate
                int tempLapse = 0;
                if (row.FromDate > priorDate)
                {
                    tempLapse = (row.FromDate - priorDate).Days;
                }
                if (tempLapse > row.LapsedDays)
                {
                    tempLapse = row.LapsedDays;
                }
                totalLapse += tempLapse;

                // Once we meet the prior insurance (based on cvDays), no need to continue.
                if (totalCoverage >= cvDays - totalLapse)
                {
                    LaspseDays = totalLapse;
                    return;
                }
            }

            // If the prior insurance is not met (based on cvDays), set LaspseDays to 999 
            if (totalCoverage < cvDays - totalLapse)
            {
                LaspseDays = 999;
                return;
            }
            LaspseDays = totalLapse;
        }
    }
}
