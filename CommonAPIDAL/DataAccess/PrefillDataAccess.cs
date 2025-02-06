using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;
using CommonAPICommon.Dto;
using CommonAPIDAL.BBDBModels;
using CommonAPIDAL.DataAccess.Mapping;
using CommonAPIDAL.VisionAppModels;
using LexisNexisService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;

namespace CommonAPIDAL.DataAccess
{
    public class PrefillDataAccess
    {
        private static DbContextOptions<BBDBEntities> BBDBConnectionString
        {
            get
            {
                DbContextOptions<BBDBEntities> optionsBuilder = new DbContextOptions<BBDBEntities>();
                return optionsBuilder;
            }
        }
        private static DbContextOptions<VisionAppEntities> VisionAppConnectionString
        {
            get
            {
                DbContextOptions<VisionAppEntities> optionsBuilder = new DbContextOptions<VisionAppEntities>();
                return optionsBuilder;
            }
        }

        /// <summary>
        /// This saves all successeve calls to Lexis Nexis Prefill
        /// </summary>
        /// <param name="quoteId"></param>
        /// <param name="isoMasterId"></param>
        public void SaveLNPrefillOrderHistory(int quoteId, int isoMasterId)
        {
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var prefillHistory = new PrefillOrderHistory();
                prefillHistory.QuoteID = quoteId;
                prefillHistory.ISOMasterID = isoMasterId;
                prefillHistory.CallDate = DateTime.Now;
                context.PrefillOrderHistory.Add(prefillHistory);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// This saves all Prefill policy info
        /// </summary>
        /// <param name="quoteId"></param>
        /// <param name="prefillResult"></param>
        public void SavePrefillPolicyData(int quoteId, int isoMasterId, AutoDataPrefillResult prefillResult)
        {
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                foreach (var _policy in prefillResult.DriverDiscovery.CurrentCarrier.CurrentCarrierReport.Report.PolicyInformation)
                {
                    {
                        var prefillPolicyData = new PrefillPolicyData();

                        prefillPolicyData.QuoteID = quoteId;
                        prefillPolicyData.IsoMasterId = isoMasterId;
                        prefillPolicyData.PolicyNumber = _policy.PolicyNumber;
                        prefillPolicyData.PolicyType = _policy.PolicyType.ToString();
                        prefillPolicyData.PolicyStatus = _policy.PolicyStatus.ToString();
                        prefillPolicyData.CarrierName = _policy.CarrierName;
                        prefillPolicyData.AmbestNumber = _policy.AmbestNumber;
                        prefillPolicyData.InceptionDate = new DateTime(_policy.InceptionDate.Year, _policy.InceptionDate.Month, _policy.InceptionDate.Day);

                        foreach (var item in _policy.PolicyHoldersList)
                        {
                            if (item.StartDate != null && item.StartDate.Year != 0)
                            {
                                prefillPolicyData.StartDate = new DateTime(item.StartDate.Year, item.StartDate.Month, item.StartDate.Day);
                            }
                            if (item.LastCancelDate != null && item.LastCancelDate.Year != 0)
                            {
                                prefillPolicyData.LastCancelDate = new DateTime(item.LastCancelDate.Year, item.LastCancelDate.Month, item.LastCancelDate.Day);
                            }
                            if (item.PolicyFromDate != null && item.PolicyFromDate.Year != 0001)
                            {
                                prefillPolicyData.PolicyFromDate = new DateTime(item.PolicyFromDate.Year, item.PolicyFromDate.Month, item.PolicyFromDate.Day);
                            }
                            if (item.PolicyToDate != null && item.PolicyToDate.Year != 0001)
                            {
                                prefillPolicyData.PolicyToDate = new DateTime(item.PolicyToDate.Year, item.PolicyToDate.Month, item.PolicyToDate.Day);
                            }
                            prefillPolicyData.Occurences = item.Occurrences;
                            prefillPolicyData.SubjectUnitNumber = item.SubjectUnitNumber;
                            prefillPolicyData.PolicyHolderName = item.PolicyHolderName.First + " " + item.PolicyHolderName.Last;
                            prefillPolicyData.PolicyHolderRelationship = item.PolicyHolderRelationship.ToString();
                            context.PrefillPolicyData.Add(prefillPolicyData);
                            context.SaveChanges();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// /This gets the most recent Prefill policy data
        /// </summary>
        /// <param name="quoteID"></param>
        /// <returns></returns>
        public List<PrefillPolicyData> GetPrefillPolicyData(int isoMasterId, ApplicantDto applicant)
        {
            var prefillPolicyDatas = new List<PrefillPolicyData>();

            applicant.InsFirstName = applicant.InsFirstName.ToUpper().Trim();
            applicant.InsLastName = applicant.InsLastName.ToUpper().Trim();
            var applicantName = String.Format("{0} {1}", applicant.InsFirstName, applicant.InsLastName);

            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var policies = context.Procedures.uspPolicies_From_PrefillPolicyDatasAsync(isoMasterId).Result.ToList();
                if (policies.Count > 0)
                {
                    var applicantPolicyData = policies.Where(w => w.SubjectUnitNumber == "1").OrderByDescending(o => o.InceptionDate).ToList();     // SubjectUnitNumber 1 is always the applicant
                    applicantPolicyData = applicantPolicyData.Count == 0 ? policies.Where(w => w.PolicyHolderName.ToUpper().Trim().Contains(applicantName)).OrderByDescending(o => o.InceptionDate).ToList() : applicantPolicyData;

                    if (applicantPolicyData.Count > 0)
                    {
                        foreach (var policy in applicantPolicyData)
                        {
                            var prefillPolicyData = new PrefillPolicyData();

                            prefillPolicyData.QuoteID = policy.QuoteID;
                            prefillPolicyData.PolicyNumber = policy.PolicyNumber;
                            prefillPolicyData.PolicyType = policy.PolicyType;
                            prefillPolicyData.PolicyStatus = policy.PolicyStatus;
                            prefillPolicyData.CarrierName = policy.CarrierName;
                            prefillPolicyData.AmbestNumber = policy.AmbestNumber;
                            prefillPolicyData.InceptionDate = policy.InceptionDate;
                            prefillPolicyData.StartDate = policy.StartDate;
                            prefillPolicyData.PolicyHolderName = policy.PolicyHolderName;
                            prefillPolicyData.PolicyHolderRelationship = policy.PolicyHolderRelationship;
                            prefillPolicyData.PolicyFromDate = policy.PolicyFromDate;
                            prefillPolicyData.PolicyToDate = policy.PolicyToDate;
                            prefillPolicyData.SubjectUnitNumber = policy.SubjectUnitNumber;

                            prefillPolicyDatas.Add(prefillPolicyData);
                        }
                    }
                }
                return prefillPolicyDatas;
            }
        }

        /// <summary>
        /// If prefill results are from a new call (new isoMasterID), this method gets prefill driver results
        /// </summary>
        /// <param name="quoteID"></param>
        /// <returns></returns>
        public List<Staging_Driver> GetExistingPrefillDrivers(int quoteID)
        {
            List<Staging_Driver> s_drivers = new List<Staging_Driver>();
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var drivers = context.Procedures.uspDrivers_From_Prefill_Driver_Select_LYAsync(quoteID).Result.ToList();
                foreach (var driver in drivers)
                {
                    Staging_Driver s_driver = new Staging_Driver();
                    s_driver.DrvFirstName = driver.FirstName;
                    s_driver.DrvLastName = driver.LastName;
                    //s_driver.DOB = driver.IsoDriverDOB != "00000000" && driver.IsoDriverDOB != null ? driver.IsoDriverDOB : DateTime.MinValue.ToString();
                    //s_driver.DOB = s_driver.DOB != DateTime.MinValue.ToString() ? s_driver.DOB : driver.StagingDriverDOB.ToString();
                    s_driver.sdID = Convert.ToInt32(driver.sdID);
                    s_drivers.Add(s_driver);
                }
            }
            return s_drivers;
        }
        /// <summary>
        /// If prefill results are from previous call (previous isoMasterID), this method gets prefill driver results 
        /// </summary>
        /// <param name="quoteID"></param>
        /// <returns></returns>
        public List<Staging_Driver> GetPreviousPrefillDrivers(int quoteID)
        {
            List<Staging_Driver> s_drivers = new List<Staging_Driver>();
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var drivers = context.Procedures.uspDrivers_From_Previous_Prefill_Driver_Select_LYAsync(quoteID).Result.ToList();
                foreach (var driver in drivers)
                {
                    Staging_Driver s_driver = new Staging_Driver();
                    s_driver.DrvFirstName = driver.DrvFirstName;
                    s_driver.DrvLastName = driver.DrvLastName;
                    //s_driver.DOB = driver.IsoDriverDOB != "00000000" && driver.IsoDriverDOB != null ? driver.IsoDriverDOB : DateTime.MinValue.ToString();
                    //s_driver.DOB = s_driver.DOB != DateTime.MinValue.ToString() ? s_driver.DOB : driver.StagingDriverDOB.ToString();
                    s_driver.sdID = Convert.ToInt32(driver.sdID);
                    s_drivers.Add(s_driver);
                }
            }
            return s_drivers;
        }

        /// <summary>
        /// /This method gets all Prefill vehicles for quoteId
        /// </summary>
        /// <param name="quoteID"></param>
        /// <returns></returns>
        public List<ISOVehicles> GetPreviousPrefillVehicles(int quoteID)
        {
            var isoVehicle = new ISOVehicles();
            var isoVehicles = new List<ISOVehicles>();
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var vehicles = context.Procedures.uspVehicles_From_Prefill_Vehicle_Select_LYAsync(quoteID).Result.ToList();
                foreach (var vehicle in vehicles)
                {
                    isoVehicle = new ISOVehicles();
                    isoVehicle.VIN = vehicle.VIN;
                    isoVehicle.ModelYear = vehicle.ModelYear;
                    isoVehicle.Make = vehicle.Make;
                    isoVehicle.ShortModelName = vehicle.Model;
                    isoVehicle.SvId = vehicle.svID;
                    isoVehicles.Add(isoVehicle);
                }
            }
            return isoVehicles;
        }

        public ClientResponse GetPreviousPrefillXml(int quoteID)
        {
            var lexisNexisMapper = new LexisNexisMapper();
            var result = new uspXML_ISO_Data_ViewResult();
            var xml = new ClientResponse();

            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var record = context.ISO_XML_Backup_Hdr.Where(w => w.QuoteID == quoteID).OrderByDescending(o => o.HdrID).FirstOrDefault();

                if (record != null)
                {
                    result = context.Procedures.uspXML_ISO_Data_ViewAsync(record.HdrID).Result.FirstOrDefault();
                    xml = result != null ? lexisNexisMapper.DeserializeLN(result.XMLFrom) : null;
                }
            }
            return xml;
        }
        public Staging_Policy GetStagingPolicy(int quoteID)
        {
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                return context.Staging_Policy.Where(w => w.QuoteID == quoteID).SingleOrDefault();
            }
        }

        public List<ISOMaster> GetIsoMaster(int quoteID)
        {
            var _isoMaster = new List<ISOMaster>();
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                _isoMaster = context.ISOMaster.Where(w => w.QuoteId.Equals(quoteID)).ToList();
            }
            return _isoMaster;
        }

        public void InsertDriverRejectType(int quoteID, int isoMasterID, Staging_Driver rejectDriver, int rejectionType)
        {
            var isoDriver = new ISODrivers();
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                // This prevent duplicate inserts 
                var existingRejection = context.ISODrivers.Where(w => w.QuoteID == quoteID && w.sdID == rejectDriver.sdID
                    && (w.RejectionType == 4 || w.RejectionType == 5 || w.RejectionType == 6)).ToList();
                if (existingRejection.Count.Equals(0))
                {
                    //this is to insert reject reason for exisiting drivers from rater or if agent changes address and makes new prefill call 

                    isoDriver.QuoteID = quoteID;
                    isoDriver.FirstName = rejectDriver.DrvFirstName;
                    isoDriver.LastName = rejectDriver.DrvLastName;
                    isoDriver.Accepted = false;
                    isoDriver.Duplicate = false;
                    isoDriver.RejectionType = rejectionType;
                    isoDriver.sdID = rejectDriver.sdID;
                    isoDriver.ISOMasterId = isoMasterID;
                    context.ISODrivers.Add(isoDriver);
                    context.SaveChanges();
                }
            }
        }
        public void InsertVehicleRejectType(int quoteID, int isoMasterID, ISOVehicles rejectVehicle, int rejectionType)
        {
            var isoVehicle = new ISOVehicles();
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                // This prevent duplicate inserts
                var existingRejection = context.ISOVehicles.Where(w => w.QuoteID == quoteID && w.SvId == rejectVehicle.SvId
                    && (w.RejectionType == 4 || w.RejectionType == 5 || w.RejectionType == 6)).ToList();
                if (existingRejection.Count.Equals(0))
                {
                    //this is to insert reject reason for exisiting vehicles from rater or if agent changes address and makes new prefill call 

                    isoVehicle.QuoteID = quoteID;
                    isoVehicle.VIN = rejectVehicle.VIN;
                    isoVehicle.ModelYear = rejectVehicle.ModelYear;
                    isoVehicle.Make = rejectVehicle.Make;
                    isoVehicle.ShortModelName = rejectVehicle.ShortModelName;
                    isoVehicle.Accepted = rejectionType == 6 ? true : false;
                    isoVehicle.ISOMasterId = isoMasterID;
                    isoVehicle.SvId = rejectVehicle.SvId;
                    isoVehicle.RejectionType = rejectionType;
                    context.ISOVehicles.Add(isoVehicle);
                    context.SaveChanges();
                }
            }
        }
        public void LogPrefillErrors(int quoteID, LexisNexisService.ClientResponse _response, string _LNisoType, string _processingStatus, string _transactionID, string innerEx)
        {
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                Prefill_CV_Clue_ErrorLog prefill_CV_Clue_ErrorLog = new Prefill_CV_Clue_ErrorLog();
                prefill_CV_Clue_ErrorLog.ErrorDate = DateTime.Now;
                prefill_CV_Clue_ErrorLog.ErrorMessage = _processingStatus;
                prefill_CV_Clue_ErrorLog.QuoteId = quoteID;
                prefill_CV_Clue_ErrorLog.TransactionID = (string.IsNullOrEmpty(_transactionID)) ? string.Empty : _transactionID;
                prefill_CV_Clue_ErrorLog.StackTrace = _response == null ? null : _response.Messages[0].Message.ToString();
                prefill_CV_Clue_ErrorLog.StackTrace = prefill_CV_Clue_ErrorLog.StackTrace == null && innerEx != null ? innerEx : null;
                prefill_CV_Clue_ErrorLog.Process = _LNisoType;
                context.Prefill_CV_Clue_ErrorLog.Add(prefill_CV_Clue_ErrorLog);
                context.SaveChanges();
            }
        }
    }
    }
