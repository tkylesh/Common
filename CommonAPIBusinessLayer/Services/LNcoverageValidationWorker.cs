using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon.Dto;
using CommonAPICommon;
using CommonAPIDAL.BBDBModels;
using CommonAPIDAL.DataAccess;
using CommonAPIDAL.Repository.Impl;
using CommonAPIBusinessLayer.Services.Impl;

namespace CommonAPIBusinessLayer.Services
{
    public class LNcoverageValidationWorker
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

        static LNcoverageValidationWorker()
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
        public void coverageValidationWorker(IList<PolicyDataDto> policyDatas, IList<PolicyCoverageIntervalDto> policyCoverageIntervals)
        {
            PolicyDatas = policyDatas;
            PolicyCoverageIntervals = policyCoverageIntervals;
        }
        public void Solve(string policyHolderName, string carrierName, DateTime effectiveDate, int lapesdayslimit, int quoteID)
        {
            var policyInfo = PolicyDatas.Where(w => w.PolicySubjects.Select(ps => ps.SubjectId.SafeTrim()).Contains("1")).OrderByDescending(o => o.LastReportedTermExpirationDate).FirstOrDefault(); //subjectID 1 is always the applicant
            var orderedIntervals = PolicyCoverageIntervals.Where(w => w.SubjectUnitNumber.SafeTrim().Contains("1")).OrderByDescending(p => p.InceptionDate).ToList(); //SubjectUnitNumber 1 is always the applicant
            if (policyInfo == null)
            {
                policyInfo = PolicyDatas.Where(p => p.PolicySubjects.Select(ps => ps.Name.ToUpper().SafeTrim()).Contains(policyHolderName.ToUpper().SafeTrim())).FirstOrDefault();
            }
            if (orderedIntervals.Count == 0)
            {
                orderedIntervals = PolicyCoverageIntervals.Where(w => w.PolicyHolderName.ToUpper().SafeTrim().Contains(policyHolderName.ToUpper().SafeTrim())).OrderByDescending(o => o.InceptionDate).ToList();
            }
            var mostRecentPolicy = this.PolicyDatas.OrderByDescending(p => p.LastReportedTermExpirationDate).FirstOrDefault();
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
            var programMasterDataAccess = new ProgramMasterDataAccess();
            var programMaster = programMasterDataAccess.GetProgramMaster(quoteID);
            var cvDays = Convert.ToInt16(programMaster.CVDayCountLimit);
            var priorDate = effectiveDate.AddDays(-cvDays);

            if (policyInfo == null || orderedIntervals.Count == 0)
            {
                LaspseDays = 999;
                return;
            }
            int totalLapse = 0;
            int totalCoverage = 0;
            int totalCountedDays = 0;

            // Loop through the CoverageIntervals until prior insurance (based on cvDays) is met.
            foreach (var row in orderedIntervals)
            {
                totalCoverage = row.CoverageDays + totalCoverage;
                totalLapse = row.LapsedDays + totalLapse;
                totalCountedDays = totalLapse + totalCoverage;

                // Once we meet the prior insurance (based on cvDays), no need to continue.
                if (totalCountedDays >= cvDays && totalLapse <= lapesdayslimit)
                {
                    LaspseDays = totalLapse;
                    return;
                }
                if (totalLapse > lapesdayslimit)
                {
                    LaspseDays = 999;
                    return;
                }
            }
            // If the prior insurance is not met (based on cvDays), set LaspseDays to 999 
            if (totalCountedDays < cvDays)
            {
                LaspseDays = 999;
                return;
            }
            LaspseDays = totalLapse;
        }

        public bool isPreviousCarrierTrexis(string latestCarrierName, string carrierName)
        {
            var PreviousCarrierTrexis = false;
            // Modify check of CarrierName for company name "Trexis"
            if (latestCarrierName.Contains(System.Configuration.ConfigurationManager.AppSettings["CompanyNameAlfaSpecialty"]))
            {
                latestCarrierName = System.Configuration.ConfigurationManager.AppSettings["CompanyNameTrexisOne"];
            }
            else if (latestCarrierName.Contains(System.Configuration.ConfigurationManager.AppSettings["CompanyNameAlfaVision"]))
            {
                latestCarrierName = System.Configuration.ConfigurationManager.AppSettings["CompanyNameTrexis"];
            }
            else if (latestCarrierName.Contains(System.Configuration.ConfigurationManager.AppSettings["CompanyNameHomestate"]))
            {
                latestCarrierName = System.Configuration.ConfigurationManager.AppSettings["CompanyNameHomestate"];
            }
            // If the latest interval is from one of our policies with the same carrier, it does not qualify for prior coverage.
            if (latestCarrierName.Equals(carrierName, StringComparison.InvariantCultureIgnoreCase))
            {
                PreviousCarrierTrexis = true;
            }
            return PreviousCarrierTrexis;
        }
        public List<string> getApplicantNameMatch(int quoteId, ApplicantDto applicant, PrefillPolicyData prefillPolicyData, string carrierName)
        {
            var prefillDataAccess = new PrefillDataAccess();
            var soundexService = new SoundexService();
            var primaryApplicantMatch = false;
            var isApplicant = false;
            var isPrimary = false;
            var lapseReason = string.Empty;
            var applicantMatch = new List<string>();

            applicant.InsFirstName = applicant.InsFirstName.ToUpper().SafeTrim();
            applicant.InsLastName = applicant.InsLastName.ToUpper().SafeTrim();
            var applicantName = String.Format("{0} {1}", applicant.InsFirstName, applicant.InsLastName);
            var policyHolderName = prefillPolicyData.PolicyHolderName.ToUpper().SafeTrim();
            var policyHolderNames = policyHolderName.Split(' ');
            var policyHolderFirstName = policyHolderNames[0].ToUpper().SafeTrim();
            var policyHolderLastName = policyHolderNames[1].ToUpper().SafeTrim();
            var policyHolderUnitNumber = prefillPolicyData.SubjectUnitNumber.SafeTrim();
            var policyHolderRelationship = prefillPolicyData.PolicyHolderRelationship.ToUpper().SafeTrim();
            var latestCarrierName = prefillPolicyData.CarrierName.ToUpper().SafeTrim();

            if (policyHolderRelationship == "PRIMARY")
            {
                isPrimary = true;
            }
            if (policyHolderUnitNumber == "1")  // policyHolderUnitNumber 1 is always the applicant
            {
                isApplicant = true;
            }
            if (!isApplicant && policyHolderName.Contains(applicantName))
            {
                isApplicant = true;
            }
            if (!isApplicant && soundexService.NameComparison(applicantName, policyHolderName))
            {
                isApplicant = true;
            }
            if (!isApplicant && applicant.DOB != DateTime.MinValue)
            {
                var xml = prefillDataAccess.GetPreviousPrefillXml(quoteId);     // go get previously saved prefill/ cv xml
                var searchDataSet = xml?.ProductResults?.AutoDataPrefillResults[0]?.SearchDataSet;

                if (searchDataSet != null)
                {
                    var searchSubjectFirstName = searchDataSet.Subjects[0].Name.First.ToUpper().SafeTrim();
                    var searchSubjectLastName = searchDataSet.Subjects[0].Name.Last.ToUpper().SafeTrim();
                    var _searchSubjectDOB = searchDataSet.Subjects[0].BirthDate.Month.ToString("00") + "/" + searchDataSet.Subjects[0].BirthDate.Day.ToString("00") + "/" + searchDataSet.Subjects[0].BirthDate.Year.ToString();
                    var searchSubjectDOB = Convert.ToDateTime(_searchSubjectDOB);

                    if (applicant.InsLastName.Contains(searchSubjectLastName) && applicant.DOB == searchSubjectDOB)
                    {
                        isApplicant = true;
                    }
                }
            }
            var previousCarrierTrexis = isPreviousCarrierTrexis(latestCarrierName, carrierName);

            if (isPrimary && isApplicant && !previousCarrierTrexis)
            {
                primaryApplicantMatch = true;
            }
            if (!isPrimary)
            {
                lapseReason = "Policy Holder Relationship to policy is " + policyHolderRelationship + ".";
            }
            if (!isApplicant)
            {
                lapseReason = "Primary Policy Holder Name does Not match Applicant Name.";
            }
            if (previousCarrierTrexis)
            {
                lapseReason = "Policy Holder is applying for a " + carrierName + " policy and their previous Carrier is " + latestCarrierName + ".";
            }
            if (!isPrimary && !isApplicant)
            {
                lapseReason = "Primary Policy Holder Name does Not match Applicant Name, and Policy Holder Relationship to policy is " + policyHolderRelationship + ".";
            }
            if (previousCarrierTrexis && !isPrimary)
            {
                lapseReason = "Policy Holder is applying for a " + carrierName + " policy and their previous Carrier is " + latestCarrierName + ", and Policy Holder Relationship to policy is " + policyHolderRelationship + ".";
            }
            if (previousCarrierTrexis && !isApplicant)
            {
                lapseReason = "Policy Holder is applying for a " + carrierName + " policy and their previous Carrier is " + latestCarrierName + ", and Primary Policy Holder Name does Not match Applicant Name.";
            }
            if (previousCarrierTrexis && !isPrimary && !isApplicant)
            {
                lapseReason = "Policy Holder is applying for a " + carrierName + " policy and their previous Carrier is " + latestCarrierName + ", Policy Holder Relationship to policy is " + policyHolderRelationship + ", and Primary Policy Holder Name does Not match Applicant Name.";
            }
            applicantMatch.Add(primaryApplicantMatch.ToString());
            applicantMatch.Add(lapseReason);

            return applicantMatch;
        }
        /// <summary>
        /// This method is to calculate Policy Coverage Intervals for Lexis Nexis Coverage Validation
        /// </summary>
        /// <param name="quoteID"></param>
        /// <param name="applicantName"></param>
        /// <param name="intervalPolicyDatas"></param>
        /// <returns></returns>
        public List<PolicyCoverageIntervalDto> CalculatePolicyCoverageIntervals(string carrierName, int quoteId, int isoMasterId, int? supplierId, ApplicantDto applicant, List<PolicyDataDto> policyDatas)
        {
            var policyCoverageIntervals = new List<PolicyCoverageIntervalDto>();
            var prefillDataAccess = new PrefillDataAccess();
            var applicantMatch = new List<string>(); // applicantMatches[0] = applicant name matches policy holders name  // applicantMatches[1] = lapse reason
            var _programMasterDataAccess = new ProgramMasterDataAccess();
            var isoRepository = new ISORepository();
            int? initialLapsedDays = 0;
            int? lapsedDays = 0;
            int? coverageDays = 0;
            int? _coverageDays = 0;
            int? overlappingCovDays = 0;
            int? initialCoverageDays = 0;
            int? _initialCoverageDays = 0;
            int? adjustedCoverageDays = 0;
            int? overlapDays = 0;
            var initialPolicyComplete = false;
            var calculationsComplete = false;
            var lapseReason = string.Empty;
            var breakFromPrior = string.Empty;
            int i = 0;
            int j = 1;
            int k = 2;

            var applicantPolicyDatas = prefillDataAccess.GetPrefillPolicyData(isoMasterId, applicant);

            // 1 policy only
            // only 1 loop through
            // only calculate applicantPolicyDatas[i]
            if (applicantPolicyDatas != null)
            {
                if (applicantPolicyDatas.Count == 1)
                {
                    applicantMatch = getApplicantNameMatch(quoteId, applicant, applicantPolicyDatas[i], carrierName);
                    var match = applicantMatch[0];
                    var _lapseReason = applicantMatch[1];

                    if (match == "True")
                    {
                        // 1 policy
                        // if most recent policy's toDate is a later date than today
                        if (applicantPolicyDatas[i].PolicyToDate > DateTime.Now)
                        {
                            lapsedDays = 0;
                            _coverageDays = Convert.ToInt16((applicantPolicyDatas[i].PolicyToDate - applicantPolicyDatas[i].InceptionDate).TotalDays);
                            overlappingCovDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[i].PolicyToDate).TotalDays);
                            adjustedCoverageDays = overlappingCovDays + _coverageDays;
                            coverageDays = adjustedCoverageDays;
                            breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                        }
                        // one policy at a time no abnormailties/ straight forward
                        if (applicantPolicyDatas[i].PolicyToDate <= DateTime.Now)
                        {
                            lapsedDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[i].PolicyToDate).TotalDays);
                            coverageDays = Convert.ToInt16((applicantPolicyDatas[i].PolicyToDate - applicantPolicyDatas[i].InceptionDate).TotalDays);
                            lapseReason = lapsedDays > 0 ? "There's a Lapse in coverage." : string.Empty;
                            breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                        }
                    }
                    if (match == "False")
                    {
                        coverageDays = 0;
                        lapsedDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[i].InceptionDate).TotalDays);
                        lapseReason = _lapseReason;
                        breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                    }
                    policyCoverageIntervals.Add(new PolicyCoverageIntervalDto()
                    {
                        Company = applicantPolicyDatas[i].CarrierName.SafeTrim(),
                        AMBest = applicantPolicyDatas[i].AmbestNumber.SafeTrim(),
                        InceptionDate = applicantPolicyDatas[i].InceptionDate,
                        FromDate = applicantPolicyDatas[i].PolicyFromDate,
                        ToDate = applicantPolicyDatas[i].PolicyToDate,
                        CoverageDays = Convert.ToInt16(coverageDays),
                        BreakFromPrior = breakFromPrior,
                        LapsedDays = Convert.ToInt16(lapsedDays),
                        LapseReason = lapseReason,
                        PolicyHolderName = applicantPolicyDatas[i].PolicyHolderName,
                        PolicyHolderRelationship = applicantPolicyDatas[i].PolicyHolderRelationship,
                        SubjectUnitNumber = applicantPolicyDatas[i].SubjectUnitNumber,
                        PolicyNumber = applicantPolicyDatas[i].PolicyNumber,
                        PolicyStatus = applicantPolicyDatas[i].PolicyStatus

                    });
                }
                // 2 policies only
                // first policy initial loop through; get lapse and coverage days from most recent policy
                // only calculate applicantPolicyDatas[i]
                if (applicantPolicyDatas.Count == 2)
                {
                    for (i = 0; i < 3; i++)
                    {
                        if (i > 0)
                        {
                            initialPolicyComplete = true;
                        }
                        if (initialPolicyComplete == false)
                        {
                            applicantMatch = getApplicantNameMatch(quoteId, applicant, applicantPolicyDatas[i], carrierName);
                            var match = applicantMatch[0];
                            var _lapseReason = applicantMatch[1];

                            if (match == "True")
                            {
                                // example 1
                                // 2nd policy's toDate is later than 1st policy's toDate and later than today: calculate with 2nd policy and dont add coverage days passed today
                                if (applicantPolicyDatas[i].PolicyToDate < applicantPolicyDatas[j].PolicyToDate &&
                                    applicantPolicyDatas[j].PolicyToDate > DateTime.Now)
                                {
                                    initialLapsedDays = 0;
                                    overlappingCovDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[j].PolicyToDate).TotalDays);
                                    initialCoverageDays = overlappingCovDays;
                                    breakFromPrior = initialLapsedDays > 0 ? "Y" : "N";
                                }
                                // example 5
                                // 2nd policy's toDate is later than 1st policy's toDate and later than today: calculate with 2nd policy
                                if (applicantPolicyDatas[i].PolicyToDate < applicantPolicyDatas[j].PolicyToDate &&
                                    applicantPolicyDatas[j].PolicyToDate <= DateTime.Now)
                                {
                                    initialCoverageDays = 0;
                                    initialLapsedDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[j].PolicyToDate).TotalDays);
                                    lapseReason = initialLapsedDays > 0 ? "There's a Lapse in coverage." : string.Empty;
                                    breakFromPrior = initialLapsedDays > 0 ? "Y" : "N";
                                }
                                // 1st policy's toDate is greater than today
                                // overlapDays should be negative number
                                if (applicantPolicyDatas[i].PolicyToDate >= applicantPolicyDatas[j].PolicyToDate &&
                                    applicantPolicyDatas[i].PolicyToDate > DateTime.Now)
                                {
                                    initialLapsedDays = 0;
                                    _initialCoverageDays = Convert.ToInt16((applicantPolicyDatas[i].PolicyToDate - applicantPolicyDatas[i].InceptionDate).TotalDays);
                                    overlappingCovDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[i].PolicyToDate).TotalDays);
                                    adjustedCoverageDays = overlappingCovDays + _initialCoverageDays;
                                    initialCoverageDays = adjustedCoverageDays;
                                    breakFromPrior = initialLapsedDays > 0 ? "Y" : "N";
                                }
                                // one policy at a time/ no abnormailties no overlaps
                                if (applicantPolicyDatas[i].PolicyToDate >= applicantPolicyDatas[j].PolicyToDate &&
                                    applicantPolicyDatas[i].PolicyToDate <= DateTime.Now)
                                {
                                    initialLapsedDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[i].PolicyToDate).TotalDays);
                                    initialCoverageDays = Convert.ToInt16((applicantPolicyDatas[i].PolicyToDate - applicantPolicyDatas[i].InceptionDate).TotalDays);
                                    lapseReason = initialLapsedDays > 0 ? "There's a Lapse in coverage." : string.Empty;
                                    breakFromPrior = initialLapsedDays > 0 ? "Y" : "N";
                                }
                            }
                            if (match == "False")
                            {
                                initialCoverageDays = 0;
                                initialLapsedDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[i].InceptionDate).TotalDays);
                                lapseReason = _lapseReason;
                                breakFromPrior = initialLapsedDays > 0 ? "Y" : "N";
                            }
                            policyCoverageIntervals.Add(new PolicyCoverageIntervalDto()
                            {
                                Company = applicantPolicyDatas[i].CarrierName.SafeTrim(),
                                AMBest = applicantPolicyDatas[i].AmbestNumber.SafeTrim(),
                                InceptionDate = applicantPolicyDatas[i].InceptionDate,
                                FromDate = applicantPolicyDatas[i].PolicyFromDate,
                                ToDate = applicantPolicyDatas[i].PolicyToDate,
                                CoverageDays = Convert.ToInt16(initialCoverageDays),
                                BreakFromPrior = breakFromPrior,
                                LapsedDays = Convert.ToInt16(initialLapsedDays),
                                LapseReason = lapseReason,
                                PolicyHolderName = applicantPolicyDatas[i].PolicyHolderName,
                                PolicyHolderRelationship = applicantPolicyDatas[i].PolicyHolderRelationship,
                                SubjectUnitNumber = applicantPolicyDatas[i].SubjectUnitNumber,
                                PolicyNumber = applicantPolicyDatas[i].PolicyNumber,
                                PolicyStatus = applicantPolicyDatas[i].PolicyStatus
                            });
                        }
                        // 2 policies only 
                        // 2nd loop through; ; get lapse and coverage days from policies prior to most recent policy
                        // only calculate applicantPolicyDatas[j]
                        if (initialPolicyComplete == true)
                        {
                            for (i = 0; i < applicantPolicyDatas.Count; i++)
                            {
                                initialLapsedDays = 0;
                                lapsedDays = 0;
                                coverageDays = 0;
                                _coverageDays = 0;
                                overlappingCovDays = 0;
                                initialCoverageDays = 0;
                                _initialCoverageDays = 0;
                                adjustedCoverageDays = 0;
                                overlapDays = 0;
                                lapseReason = string.Empty;
                                breakFromPrior = string.Empty;

                                // 1 policy left
                                if (i == (applicantPolicyDatas.Count - 1))
                                {
                                    calculationsComplete = true;
                                }
                                // 2 policies 
                                if (i == (applicantPolicyDatas.Count - 2))
                                {
                                    applicantMatch = getApplicantNameMatch(quoteId, applicant, applicantPolicyDatas[j], carrierName);
                                    var match = applicantMatch[0];
                                    var _lapseReason = applicantMatch[1];

                                    if (match == "True")
                                    {
                                        // example 5
                                        // 2nd policy's toDate is later than first policies toDate so calculate against today
                                        if (applicantPolicyDatas[i].PolicyToDate <= applicantPolicyDatas[j].PolicyToDate &&
                                            applicantPolicyDatas[j].PolicyToDate <= DateTime.Now)
                                        {
                                            lapsedDays = 0;
                                            coverageDays = Convert.ToInt16((applicantPolicyDatas[j].PolicyToDate - applicantPolicyDatas[j].InceptionDate).TotalDays);
                                            breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                        }
                                        // example 1
                                        if (applicantPolicyDatas[i].PolicyToDate <= applicantPolicyDatas[j].PolicyToDate &&
                                            applicantPolicyDatas[j].PolicyToDate > DateTime.Now)
                                        {
                                            lapsedDays = 0;
                                            overlapDays = policyCoverageIntervals[i].CoverageDays > 0 ? Convert.ToInt16((DateTime.Now - applicantPolicyDatas[j].PolicyToDate).TotalDays) : 0;
                                            _coverageDays = Convert.ToInt16((applicantPolicyDatas[j].PolicyToDate - applicantPolicyDatas[j].InceptionDate).TotalDays);
                                            adjustedCoverageDays = overlapDays + _coverageDays;
                                            coverageDays = adjustedCoverageDays;
                                            breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                        }
                                        // example 2
                                        // 1st and 2nd policy overlap 
                                        if (applicantPolicyDatas[i].PolicyToDate > applicantPolicyDatas[j].PolicyToDate &&
                                            applicantPolicyDatas[i].InceptionDate < applicantPolicyDatas[j].PolicyToDate)
                                        {
                                            // overlapDays should be negative number
                                            lapsedDays = 0;
                                            overlapDays = policyCoverageIntervals[i].CoverageDays > 0 ? Convert.ToInt16((applicantPolicyDatas[i].InceptionDate - applicantPolicyDatas[j].PolicyToDate).TotalDays) : 0;
                                            _coverageDays = Convert.ToInt16((applicantPolicyDatas[j].PolicyToDate - applicantPolicyDatas[j].InceptionDate).TotalDays);
                                            adjustedCoverageDays = overlapDays + _coverageDays;
                                            coverageDays = adjustedCoverageDays;
                                            breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                        }
                                        // one policy at a time/ no abnormailties no overlaps
                                        if (applicantPolicyDatas[i].InceptionDate >= applicantPolicyDatas[j].PolicyToDate)
                                        {
                                            lapsedDays = Convert.ToInt16((applicantPolicyDatas[i].InceptionDate - applicantPolicyDatas[j].PolicyToDate).TotalDays);
                                            coverageDays = Convert.ToInt16((applicantPolicyDatas[j].PolicyToDate - applicantPolicyDatas[j].InceptionDate).TotalDays);
                                            lapseReason = lapsedDays > 0 ? "There's a Lapse in coverage." : string.Empty;
                                            breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                        }
                                    }
                                    if (match == "False")
                                    {
                                        coverageDays = 0;
                                        lapsedDays = Convert.ToInt16((applicantPolicyDatas[i].InceptionDate - applicantPolicyDatas[j].InceptionDate).TotalDays);
                                        lapseReason = _lapseReason;
                                        breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                    }
                                }
                                if (initialPolicyComplete == true && calculationsComplete == false)
                                {
                                    policyCoverageIntervals.Add(new PolicyCoverageIntervalDto()
                                    {
                                        Company = applicantPolicyDatas[j].CarrierName.SafeTrim(),
                                        AMBest = applicantPolicyDatas[j].AmbestNumber.SafeTrim(),
                                        InceptionDate = applicantPolicyDatas[j].InceptionDate,
                                        FromDate = applicantPolicyDatas[j].PolicyFromDate,
                                        ToDate = applicantPolicyDatas[j].PolicyToDate,
                                        CoverageDays = Convert.ToInt16(coverageDays),
                                        BreakFromPrior = breakFromPrior,
                                        LapsedDays = Convert.ToInt16(lapsedDays),
                                        LapseReason = lapseReason,
                                        PolicyHolderName = applicantPolicyDatas[j].PolicyHolderName,
                                        PolicyHolderRelationship = applicantPolicyDatas[j].PolicyHolderRelationship,
                                        SubjectUnitNumber = applicantPolicyDatas[j].SubjectUnitNumber,
                                        PolicyNumber = applicantPolicyDatas[j].PolicyNumber,
                                        PolicyStatus = applicantPolicyDatas[j].PolicyStatus
                                    });
                                    j++;
                                    k++;
                                }
                            }
                            initialPolicyComplete = true;
                        }
                    }
                }
                // 3 or more policies
                // initial first policy loop through; get lapse and coverage days from most recent policy
                // only calculate applicantPolicyDatas[i]
                if (applicantPolicyDatas.Count >= 3)
                {
                    // initial walk through
                    // loop through whole thing twice
                    for (i = 0; i < 3; i++)
                    {
                        if (i > 0)
                        {
                            initialPolicyComplete = true;
                        }
                        if (initialPolicyComplete == false)
                        {
                            applicantMatch = getApplicantNameMatch(quoteId, applicant, applicantPolicyDatas[i], carrierName);
                            var match = applicantMatch[0];
                            var _lapseReason = applicantMatch[1];

                            if (match == "True")
                            {
                                // one policy at a time/ no abnormailties no overlaps
                                if (applicantPolicyDatas[i].PolicyToDate > applicantPolicyDatas[j].PolicyToDate &&
                                    applicantPolicyDatas[i].PolicyToDate > applicantPolicyDatas[k].PolicyToDate &&
                                    applicantPolicyDatas[i].PolicyToDate <= DateTime.Today)
                                {
                                    initialLapsedDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[i].PolicyToDate).TotalDays);
                                    initialCoverageDays = Convert.ToInt16((applicantPolicyDatas[i].PolicyToDate - applicantPolicyDatas[i].InceptionDate).TotalDays);
                                    lapseReason = initialLapsedDays > 0 ? "There's a Lapse in coverage." : string.Empty;
                                    breakFromPrior = initialLapsedDays > 0 ? "Y" : "N";
                                }
                                // example 1
                                if (applicantPolicyDatas[i].PolicyToDate >= applicantPolicyDatas[j].PolicyToDate &&
                                    applicantPolicyDatas[i].PolicyToDate >= applicantPolicyDatas[k].PolicyToDate &&
                                    applicantPolicyDatas[i].PolicyToDate > DateTime.Now)
                                {
                                    initialLapsedDays = 0;
                                    _initialCoverageDays = Convert.ToInt16((applicantPolicyDatas[i].PolicyToDate - applicantPolicyDatas[i].InceptionDate).TotalDays);
                                    overlappingCovDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[i].PolicyToDate).TotalDays);
                                    adjustedCoverageDays = overlappingCovDays + _initialCoverageDays;
                                    initialCoverageDays = adjustedCoverageDays;
                                    breakFromPrior = initialLapsedDays > 0 ? "Y" : "N";
                                }
                                if (applicantPolicyDatas[j].PolicyToDate > applicantPolicyDatas[i].PolicyToDate &&
                                    applicantPolicyDatas[j].PolicyToDate >= applicantPolicyDatas[k].PolicyToDate &&
                                    applicantPolicyDatas[j].PolicyToDate > DateTime.Now)
                                {
                                    initialLapsedDays = 0;
                                    overlappingCovDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[j].PolicyToDate).TotalDays);
                                    initialCoverageDays = overlappingCovDays;
                                    breakFromPrior = initialLapsedDays > 0 ? "Y" : "N";
                                }
                                if (applicantPolicyDatas[j].PolicyToDate > applicantPolicyDatas[i].PolicyToDate &&
                                    applicantPolicyDatas[j].PolicyToDate >= applicantPolicyDatas[k].PolicyToDate &&
                                    applicantPolicyDatas[j].PolicyToDate <= DateTime.Now)
                                {
                                    initialCoverageDays = 0;
                                    initialLapsedDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[j].PolicyToDate).TotalDays);
                                    lapseReason = initialLapsedDays > 0 ? "There's a Lapse in coverage." : string.Empty;
                                    breakFromPrior = initialLapsedDays > 0 ? "Y" : "N";

                                }
                                if (applicantPolicyDatas[k].PolicyToDate > applicantPolicyDatas[i].PolicyToDate &&
                                    applicantPolicyDatas[k].PolicyToDate > applicantPolicyDatas[j].PolicyToDate &&
                                    applicantPolicyDatas[k].PolicyToDate > DateTime.Now)
                                {
                                    initialLapsedDays = 0;
                                    overlappingCovDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[k].PolicyToDate).TotalDays);
                                    initialCoverageDays = overlappingCovDays;
                                    breakFromPrior = initialLapsedDays > 0 ? "Y" : "N";
                                }
                                if (applicantPolicyDatas[k].PolicyToDate > applicantPolicyDatas[i].PolicyToDate &&
                                    applicantPolicyDatas[k].PolicyToDate > applicantPolicyDatas[j].PolicyToDate &&
                                    applicantPolicyDatas[k].PolicyToDate <= DateTime.Now)
                                {
                                    initialCoverageDays = 0;
                                    initialLapsedDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[k].PolicyToDate).TotalDays);
                                    lapseReason = initialLapsedDays > 0 ? "There's a Lapse in coverage." : string.Empty;
                                    breakFromPrior = initialLapsedDays > 0 ? "Y" : "N";
                                }
                            }
                            if (match == "False")
                            {
                                initialCoverageDays = 0;
                                initialLapsedDays = Convert.ToInt16((DateTime.Now - applicantPolicyDatas[i].InceptionDate).TotalDays);
                                lapseReason = _lapseReason;
                                breakFromPrior = initialLapsedDays > 0 ? "Y" : "N";
                            }
                            policyCoverageIntervals.Add(new PolicyCoverageIntervalDto()
                            {
                                Company = applicantPolicyDatas[i].CarrierName.SafeTrim(),
                                AMBest = applicantPolicyDatas[i].AmbestNumber.SafeTrim(),
                                InceptionDate = applicantPolicyDatas[i].InceptionDate,
                                FromDate = applicantPolicyDatas[i].PolicyFromDate,
                                ToDate = applicantPolicyDatas[i].PolicyToDate,
                                CoverageDays = Convert.ToInt16(initialCoverageDays),
                                BreakFromPrior = breakFromPrior,
                                LapsedDays = Convert.ToInt16(initialLapsedDays),
                                LapseReason = lapseReason,
                                PolicyHolderName = applicantPolicyDatas[i].PolicyHolderName,
                                PolicyHolderRelationship = applicantPolicyDatas[i].PolicyHolderRelationship,
                                SubjectUnitNumber = applicantPolicyDatas[i].SubjectUnitNumber,
                                PolicyNumber = applicantPolicyDatas[i].PolicyNumber,
                                PolicyStatus = applicantPolicyDatas[i].PolicyStatus
                            });
                        }
                        // 3 or more policies only
                        // 2nd loop through; ; get lapse and coverage days from policies prior to most recent policy
                        // only calculate applicantPolicyDatas[j]
                        if (initialPolicyComplete == true)
                        {
                            for (i = 0; i < applicantPolicyDatas.Count; i++)
                            {
                                initialLapsedDays = 0;
                                lapsedDays = 0;
                                coverageDays = 0;
                                _coverageDays = 0;
                                overlappingCovDays = 0;
                                initialCoverageDays = 0;
                                adjustedCoverageDays = 0;
                                overlapDays = 0;
                                lapseReason = string.Empty;
                                breakFromPrior = string.Empty;

                                // 1 last policy
                                if (i == (applicantPolicyDatas.Count - 1))
                                {
                                    calculationsComplete = true;
                                }
                                // 2 policies
                                if (i == (applicantPolicyDatas.Count - 2))
                                {
                                    applicantMatch = getApplicantNameMatch(quoteId, applicant, applicantPolicyDatas[j], carrierName);
                                    var match = applicantMatch[0];
                                    var _lapseReason = applicantMatch[1];

                                    if (match == "True")
                                    {
                                        // overlap
                                        if (applicantPolicyDatas[j].PolicyToDate > applicantPolicyDatas[i].InceptionDate)
                                        {
                                            lapsedDays = 0;
                                            overlapDays = policyCoverageIntervals[i].CoverageDays > 0 ? Convert.ToInt16((applicantPolicyDatas[i].InceptionDate - applicantPolicyDatas[j].PolicyToDate).TotalDays) : 0;
                                            _coverageDays = Convert.ToInt16((applicantPolicyDatas[j].PolicyToDate - applicantPolicyDatas[j].InceptionDate).TotalDays);
                                            adjustedCoverageDays = overlapDays + _coverageDays;
                                            coverageDays = adjustedCoverageDays;
                                            breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                        }
                                        // straight forward
                                        // 2 policies
                                        if (applicantPolicyDatas[j].PolicyToDate <= applicantPolicyDatas[i].InceptionDate)
                                        {
                                            lapsedDays = Convert.ToInt16((applicantPolicyDatas[i].InceptionDate - applicantPolicyDatas[j].PolicyToDate).TotalDays);
                                            coverageDays = Convert.ToInt16((applicantPolicyDatas[j].PolicyToDate - applicantPolicyDatas[j].InceptionDate).TotalDays);
                                            lapseReason = lapsedDays > 0 ? "There's a Lapse in coverage." : string.Empty;
                                            breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                        }
                                    }
                                    if (match == "False")
                                    {
                                        coverageDays = 0;
                                        lapsedDays = Convert.ToInt16((applicantPolicyDatas[i].InceptionDate - applicantPolicyDatas[j].InceptionDate).TotalDays);
                                        lapseReason = _lapseReason;
                                        breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                    }
                                }
                                // 3 or more policies
                                if (i <= (applicantPolicyDatas.Count - 3))
                                {
                                    applicantMatch = getApplicantNameMatch(quoteId, applicant, applicantPolicyDatas[j], carrierName);
                                    var match = applicantMatch[0];
                                    var _lapseReason = applicantMatch[1];

                                    if (match == "True")
                                    {
                                        // example 1
                                        // j has most recent toDate
                                        if (applicantPolicyDatas[j].PolicyToDate >= applicantPolicyDatas[i].PolicyToDate &&
                                            applicantPolicyDatas[j].PolicyToDate >= applicantPolicyDatas[k].PolicyToDate)
                                        {
                                            lapsedDays = 0;
                                            overlapDays = policyCoverageIntervals[i].CoverageDays > 0 ? Convert.ToInt16((applicantPolicyDatas[i].InceptionDate - applicantPolicyDatas[j].PolicyToDate).TotalDays) : 0;
                                            _coverageDays = Convert.ToInt16((applicantPolicyDatas[j].PolicyToDate - applicantPolicyDatas[j].InceptionDate).TotalDays);
                                            adjustedCoverageDays = _coverageDays + overlapDays;
                                            coverageDays = adjustedCoverageDays;
                                            breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                        }
                                        // example 2
                                        // 1st and 2nd policy overlap
                                        if (applicantPolicyDatas[j].PolicyToDate >= applicantPolicyDatas[i].InceptionDate &&
                                            applicantPolicyDatas[j].PolicyToDate >= applicantPolicyDatas[k].PolicyToDate)
                                        {
                                            lapsedDays = 0;
                                            overlapDays = policyCoverageIntervals[i].CoverageDays > 0 ? Convert.ToInt16((applicantPolicyDatas[i].InceptionDate - applicantPolicyDatas[j].PolicyToDate).TotalDays) : 0;
                                            _coverageDays = Convert.ToInt16((applicantPolicyDatas[j].PolicyToDate - applicantPolicyDatas[j].InceptionDate).TotalDays);
                                            adjustedCoverageDays = overlapDays + _coverageDays;
                                            coverageDays = adjustedCoverageDays;
                                            breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                        }
                                        // k has most recent toDate
                                        if (applicantPolicyDatas[k].PolicyToDate >= applicantPolicyDatas[i].PolicyToDate &&
                                            applicantPolicyDatas[k].PolicyToDate > applicantPolicyDatas[j].PolicyToDate)
                                        {
                                            lapsedDays = 0;
                                            coverageDays = 0;
                                            breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                        }
                                        if (applicantPolicyDatas[k].PolicyToDate < applicantPolicyDatas[i].PolicyToDate &&
                                            applicantPolicyDatas[k].PolicyToDate > applicantPolicyDatas[j].PolicyToDate)
                                        {
                                            coverageDays = 0;
                                            lapsedDays = Convert.ToInt16((applicantPolicyDatas[i].InceptionDate - applicantPolicyDatas[k].PolicyToDate).TotalDays);
                                            lapseReason = lapsedDays > 0 ? "There's a Lapse in coverage." : string.Empty;
                                            breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                        }
                                        // one policy at a time/ no abnormailties no overlaps
                                        if (applicantPolicyDatas[j].PolicyToDate < applicantPolicyDatas[i].InceptionDate &&
                                            applicantPolicyDatas[j].PolicyToDate >= applicantPolicyDatas[k].PolicyToDate)
                                        {
                                            lapsedDays = Convert.ToInt16((applicantPolicyDatas[i].InceptionDate - applicantPolicyDatas[j].PolicyToDate).TotalDays);
                                            coverageDays = Convert.ToInt16((applicantPolicyDatas[j].PolicyToDate - applicantPolicyDatas[j].InceptionDate).TotalDays);
                                            lapseReason = lapsedDays > 0 ? "There's a Lapse in coverage." : string.Empty;
                                            breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                        }
                                    }
                                    if (match == "False")
                                    {
                                        coverageDays = 0;
                                        lapsedDays = Convert.ToInt16((applicantPolicyDatas[i].InceptionDate - applicantPolicyDatas[j].InceptionDate).TotalDays);
                                        lapseReason = _lapseReason;
                                        breakFromPrior = lapsedDays > 0 ? "Y" : "N";
                                    }
                                }
                                if (initialPolicyComplete == true && calculationsComplete == false)
                                {
                                    policyCoverageIntervals.Add(new PolicyCoverageIntervalDto()
                                    {
                                        Company = applicantPolicyDatas[j].CarrierName.SafeTrim(),
                                        AMBest = applicantPolicyDatas[j].AmbestNumber.SafeTrim(),
                                        InceptionDate = applicantPolicyDatas[j].InceptionDate,
                                        FromDate = applicantPolicyDatas[j].PolicyFromDate,
                                        ToDate = applicantPolicyDatas[j].PolicyToDate,
                                        CoverageDays = Convert.ToInt16(coverageDays),
                                        BreakFromPrior = breakFromPrior,
                                        LapsedDays = Convert.ToInt16(lapsedDays),
                                        LapseReason = lapseReason,
                                        PolicyHolderName = applicantPolicyDatas[j].PolicyHolderName,
                                        PolicyHolderRelationship = applicantPolicyDatas[j].PolicyHolderRelationship,
                                        SubjectUnitNumber = applicantPolicyDatas[j].SubjectUnitNumber,
                                        PolicyNumber = applicantPolicyDatas[j].PolicyNumber,
                                        PolicyStatus = applicantPolicyDatas[j].PolicyStatus
                                    });
                                    j++;
                                    k++;
                                }
                            }
                        }
                    }
                }
            }
            isoRepository.SaveCV(isoMasterId, supplierId, policyCoverageIntervals.ToArray(), policyDatas.ToArray());
            return policyCoverageIntervals;
        }
    }
}
