using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPIDAL.BBDBModels;
using CommonAPIDAL.VisionAppModels;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace CommonAPIDAL.DataAccess
{
    internal class ISODataAccess
    {
        private static DbContextOptions<BBDBEntities> BBDBConnectionString
        {
            get
            {
                DbContextOptions<BBDBEntities> optionsBuilder = new DbContextOptions<BBDBEntities>();
                return optionsBuilder;
            }
        }
        internal static int GetExistingISOMasterId(dynamic applicant)
        {
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                string fName = applicant.InsFirstName.Trim().ToUpper();
                string lName = applicant.InsLastName.Trim().ToUpper();
                string address = applicant.GarAddress.Trim().ToUpper();
                string city = applicant.GarCity.Trim().ToUpper();
                string state = applicant.GarState.Trim().ToUpper();
                string zip = applicant.GarZip.Trim().ToUpper();
                int quoteID = applicant.QuoteID;

                DateTime testDate = DateTime.Now.AddDays(Convert.ToDouble(ConfigurationManager.AppSettings["PrefillLookBackDays"]));

                var query = context.ISOMaster.Where(isom => isom.FirstName == fName
                                       && isom.LastName == lName
                                       && isom.Address == address
                                       && isom.City == city
                                       && isom.ISOState == state
                                       && (isom.ISOState == "OH" || isom.Zip.Replace("-", "") == zip)
                                       && isom.CreateDate >= testDate)
                                       .OrderByDescending(isom => isom.ISOMasterID)
                        .FirstOrDefault();

                return query == null ? 0 : query.ISOMasterID;
            }
        }
        internal static void StoreRawISOXML(int quoteId, string xmlFromISO, double responseTime, string status,
                                            string xmlToISO, string requestId, string product, string lexID)
        {
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                context.Procedures.uspISORawXML_InsertAsync(quoteId, string.Format("{0:Mdyyyyhhmmsstt}.xml", DateTime.Now), status, xmlFromISO,
                                            (int)responseTime, xmlToISO, requestId, product, lexID);
            }
        }
        internal static int StoreISOMaster(dynamic applicant)
        {
            int quoteId = applicant.QuoteID;
            string fName = applicant.InsFirstName.Trim().ToUpper();
            string lName = applicant.InsLastName.Trim().ToUpper();
            string address = applicant.GarAddress.Trim().ToUpper();
            string city = applicant.GarCity.Trim().ToUpper();
            string state = applicant.GarState.Trim().ToUpper();
            string zip = applicant.GarZip.Trim().ToUpper();

            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var isoMaster = new ISOMaster();
                isoMaster.QuoteId = quoteId;
                isoMaster.FirstName = fName;
                isoMaster.LastName = lName;
                isoMaster.Address = address;
                isoMaster.City = city;
                isoMaster.ISOState = state;
                isoMaster.Zip = zip;
                isoMaster.CreateDate = DateTime.Now;
                isoMaster.NewRecord = true;
                context.ISOMaster.Add(isoMaster);
                context.SaveChanges();
                return isoMaster.ISOMasterID;
            }
        }
        //Save Prefill
        internal static void SavePrefill(int masterId, int quoteId, IList<dynamic> drivers, IList<dynamic> vehicles, int? supplierID)
        {
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                if (drivers != null && drivers.Any())
                {
                    foreach (var driver in drivers.OrderByDescending(d => d.DateOfBirth).OrderBy(d => d.FirstName).OrderBy(d => d.LastName))
                    {
                        //var param = new System.Data.Entity.Core.Objects.ObjectParameter("rowID", 0);
                        BBDBModels.OutputParameter<int?> param = new BBDBModels.OutputParameter<int?>();
                        context.Procedures.uspDrivers_From_ISO_Insert_LYAsync(quoteId, drivers.Count, driver.FirstName, driver.MiddleName, driver.LastName, driver.DLNumber, string.Format("{0:yyyyMMdd}", driver.DateOfBirth), driver.SSN, driver.Gender, driver.Address, driver.City, driver.State, driver.ZipCode, masterId, param);
                        driver.rowID = (int)param.Value;
                        driver.ISOMasterID = masterId;
                    }
                }
                else // No Drivers from Prefill
                {
                    BBDBModels.OutputParameter<int?> param = new BBDBModels.OutputParameter<int?>();
                    //var param = new System.Data.Entity.Core.Objects.ObjectParameter("rowID", 0);
                    context.Procedures.uspDrivers_From_ISO_Insert_LYAsync(quoteId, drivers.Count, null, null, null, null, null, null, null, null, null, null, null, masterId, param);
                }
                if (vehicles != null && vehicles.Any())
                {
                    foreach (var vehicle in vehicles.OrderByDescending(v => v.ModelYear))
                    {
                        var isoVeh = new Vehicles_From_ISO();
                        isoVeh.insertDate = DateTime.Now;
                        isoVeh.ISOMasterID = masterId;
                        isoVeh.ModelYear = vehicle.ModelYear;
                        isoVeh.quoteID = quoteId;
                        isoVeh.VIN = vehicle.VIN;
                        isoVeh.Make = vehicle.Make;
                        isoVeh.ShortModel = vehicle.ShortModel;
                        isoVeh.VehicleCount = vehicles.Count;
                        isoVeh.LienHolderSequenceNumber = Convert.ToInt32(vehicle.LienHolderSequenceNumber);
                        isoVeh.LienHolderName = vehicle.LienHolderName;
                        isoVeh.LienHolderCity = vehicle.LienHolderCity;
                        isoVeh.LienHolderStreetAddress = vehicle.LienHolderStreetAddress;
                        isoVeh.LienHolderState = vehicle.LienHolderState;
                        isoVeh.LienHolderZipCode = vehicle.LienHolderZipCode;
                        context.Vehicles_From_ISO.Add(isoVeh);
                        context.SaveChanges();
                        vehicle.rowID = isoVeh.rowID;
                    }
                }
                else // No vehicles from LN Prefill
                {
                    var isoVeh = new Vehicles_From_ISO();
                    isoVeh.insertDate = DateTime.Now;
                    isoVeh.ISOMasterID = masterId;
                    isoVeh.ModelYear = null;
                    isoVeh.quoteID = quoteId;
                    isoVeh.VIN = null;
                    isoVeh.Make = null;
                    isoVeh.ShortModel = null;
                    isoVeh.VehicleCount = vehicles.Count;
                    isoVeh.LienHolderSequenceNumber = null;
                    isoVeh.LienHolderName = null;
                    isoVeh.LienHolderStreetAddress = null;
                    isoVeh.LienHolderState = null;
                    isoVeh.LienHolderZipCode = null;
                    context.Vehicles_From_ISO.Add(isoVeh);
                    context.SaveChanges();
                }
            }
        }
        /// <summary>
        /// this is to delete previous coverage interval calculations data before
        /// recalculating and updating coverage interval
        /// </summary>
        /// <param name="isoMasterId"></param>
        /// <param name="intervalPolicyDatas"></param>
        public static void DeletePreviousCoverageIntervalData(int isoMasterId)
        {
            var prefillPolicyDatas = new List<PrefillPolicyData>();
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var coverageIntervals = context.PolicyCoverageInterval.Where(w => w.ISOMasterId == isoMasterId).ToList();

                if (coverageIntervals.Count > 0)
                {
                    context.PolicyCoverageInterval.RemoveRange(coverageIntervals);
                    context.SaveChanges();
                }
            }
        }
        public static bool DoesPolicyDataExist(int isoMasterId)
        {
            var prefillPolicyDatas = new List<PrefillPolicyData>();
            var policyDataExists = false;

            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var policyDatas = context.PolicyData.Where(w => w.ISOMasterId == isoMasterId).ToList();

                if (policyDatas.Count > 0)
                {
                    policyDataExists = true;
                }
            }
            return policyDataExists;
        }


        internal static void SaveCV(int masterId, int? supplierId, IList<dynamic> intervals, IList<dynamic> polDatas)
        {
            var doesPolicyDataExist = false;
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                if (supplierId == 2)
                {
                    DeletePreviousCoverageIntervalData(masterId);               // in order update, this deletes old coveage data before inserting new data
                    doesPolicyDataExist = DoesPolicyDataExist(masterId);    //checks if polcyData already exist to avoid duplicate records
                }
                if (intervals != null && intervals.Any())
                {
                    foreach (var interval in intervals.OrderByDescending(i => i.FromDate))
                    {
                        context.PolicyCoverageInterval.Add(new PolicyCoverageInterval()
                        {
                            AMBest = interval.AMBest,
                            BreakFromPrior = interval.BreakFromPrior,
                            Company = interval.Company,
                            CoverageDays = interval.CoverageDays,
                            InceptionDate = interval.InceptionDate != DateTime.MinValue ? interval.InceptionDate : null,
                            FromDate = interval.FromDate,
                            ISOMasterId = masterId,
                            LapsedDays = interval.LapsedDays,
                            ToDate = interval.ToDate,
                            LapseReason = interval.LapseReason,
                            PolicyHolderName = interval.PolicyHolderName,
                            PolicyHolderRelationship = interval.PolicyHolderRelationship,
                            SubjectUnitNumber = interval.SubjectUnitNumber,
                            PolicyNumber = interval.PolicyNumber,
                            PolicyStatus = interval.PolicyStatus
                        });
                    }
                }
                if (!doesPolicyDataExist && polDatas != null && polDatas.Any())
                {
                    foreach (var polData in polDatas)
                    {
                        var policyData = new PolicyData();
                        policyData.BIIndividualLimit = polData.BIIndividualLimit;
                        policyData.BIOccuranceLimit = polData.BIOccuranceLimit;
                        policyData.CancelReason = polData.CancelReason;
                        policyData.CarrierName = polData.CarrierName;
                        policyData.CoverageFromDate = polData.CoverageFromDate;
                        policyData.CoverageToDate = polData.CoverageToDate;
                        policyData.CSILimit = polData.CSILimit;
                        policyData.InceptionDate = polData.InceptionDate;
                        policyData.ISOMasterId = masterId;
                        policyData.IsStandardPolicy = polData.IsStandardPolicy;
                        policyData.LastCancelDate = polData.LastCancelDate;
                        policyData.LastReportedTermEffectiveDate = polData.LastReportedTermEffectiveDate;
                        policyData.LastReportedTermExpirationDate = polData.LastReportedTermExpirationDate;
                        policyData.NAIC = polData.NAIC;
                        policyData.NumberOfCancellations = polData.NumberOfCancellations;
                        policyData.NumberOfRenewals = polData.NumberOfRenewals;
                        policyData.PolicyHolderName = polData.PolicyHolderName;
                        policyData.PolicyNumber = polData.PolicyNumber;
                        policyData.PolicyState = polData.PolicyState;
                        policyData.PolicyStatus = polData.PolicyStatus;
                        policyData.PolicyType = polData.PolicyType;
                        policyData.ReportAsOfDate = polData.ReportAsOfDate;
                        var policySubjects = new List<PolicySubject>();
                        if (polData.PolicySubjects != null)// && polData.PolicySubjects.Any())
                        {
                            foreach (var polSub in polData.PolicySubjects)
                            {
                                var policySubject = new PolicySubject();

                                policySubject.Name = polSub.Name;
                                policySubject.RelationCode = polSub.RelationCode;
                                policySubject.FromDate = polSub.FromDate;
                                policySubject.ToDate = polSub.ToDate;
                                policySubject.RelationCodeDesc = polSub.RelationCodeDesc;
                                policySubject.SubjectId = polSub.SubjectId;

                                var policySubjectHistories = new List<PolicySubjectHistory>();
                                if (polSub.PolicySubjectHistories != null) // && polSub.PolicySubjectHistories.Any())
                                {
                                    foreach (var polSubHist in polSub.PolicySubjectHistories)
                                    {
                                        var policySubjectHistory = new PolicySubjectHistory();
                                        policySubjectHistory.FromDate = polSubHist.FromDate;
                                        policySubjectHistory.ToDate = polSubHist.ToDate;
                                        policySubjectHistories.Add(policySubjectHistory);
                                    }
                                }
                                policySubject.PolicySubjectHistory = policySubjectHistories;
                                policySubjects.Add(policySubject);
                            }
                        }
                        policyData.PolicySubject = policySubjects;
                        context.PolicyData.Add(policyData);
                    }
                }
                context.SaveChanges();
            }
        }
        internal static IList<dynamic> GetPolicyDatas(int isoMasterId)
        {
            var list = new List<dynamic>();
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var polDatas = context.PolicyData.Include("PolicySubjects.PolicySubjectHistories").Where(pd => pd.ISOMasterId == isoMasterId);
                foreach (var polData in polDatas)
                {
                    list.Add(polData);
                }
            }
            return list;
        }
        internal static IList<dynamic> GetCoverageIntervals(int isoMasterId)
        {
            var list = new List<dynamic>();
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var intervals = context.PolicyCoverageInterval.Where(pci => pci.ISOMasterId == isoMasterId && pci.BreakFromPrior != "NA");
                foreach (var interval in intervals)
                {
                    list.Add(interval);
                }
            }
            return list;
        }
        internal static IList<dynamic> GetPrefillDrivers(int isoMasterId)
        {
            var list = new List<dynamic>();
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                //var drivers = context.uspDrivers_From_ISO_Select(GetQuoteID(isoMasterId));
                var drivers = context.Procedures.uspDrivers_From_ISO_Select_LYAsync(isoMasterId).Result;
                foreach (var driver in drivers)
                {
                    dynamic drv = new ExpandoObject();
                    drv.ISOMasterID = isoMasterId;
                    drv.DateOfBirth = driver.DateOfBirth;
                    drv.DLNumber = driver.DLNumber;
                    drv.FirstName = driver.FirstName;
                    drv.Gender = driver.Gender;
                    drv.LastName = driver.LastName;
                    drv.MiddleName = driver.MiddleName;
                    drv.Address = driver.Address;
                    drv.City = driver.City;
                    drv.State = driver.State;
                    drv.ZipCode = driver.Zipcode;
                    drv.rowID = driver.rowID;
                    drv.SSN = driver.SSN;
                    list.Add(drv);
                }
            }
            return list;
        }
        internal static IList<dynamic> GetPrefillVehicles(int isoMasterId)
        {
            int quoteId = GetQuoteID(isoMasterId);
            var list = new List<dynamic>();
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var vehicles = context.Vehicles_From_ISO.Where(v => v.quoteID == quoteId && v.VIN != null);
                foreach (var vehicle in vehicles)
                {
                    list.Add(vehicle);
                }
            }
            return list;
        }
        internal static int GetQuoteID(int isoMasterId)
        {
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var query = context.ISOMaster.Where(m => m.ISOMasterID == isoMasterId)
                        .SingleOrDefault();

                return query == null ? 0 : query.QuoteId;
            }

        }
    }
}
