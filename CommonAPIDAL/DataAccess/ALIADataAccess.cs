using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;
using CommonAPIDAL.AlfaVisionWebModels;
using CommonAPIDAL.VisionAppModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;

namespace CommonAPIDAL.DataAccess
{
    public class ALIADataAccess: DataAccessBase
    {
        public void SaveALIALog(string alfaTransactionNumber, string quoteId, string membershipNumber, string payloadSent, string payloadReceived, string apiCalled, string policyNumber, bool membershipFlag, bool pacFlag)
        {
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                ALIATransactionLog AliaTransactionLog = new ALIATransactionLog();
                AliaTransactionLog.AlfaTransactionNumber = alfaTransactionNumber;
                AliaTransactionLog.PolicyNumber = policyNumber;
                AliaTransactionLog.Date = DateTime.Now;
                AliaTransactionLog.MembershipNumber = membershipNumber;
                AliaTransactionLog.PayloadSent = payloadSent;
                AliaTransactionLog.PayloadReceived = payloadReceived;
                AliaTransactionLog.ApiCalled = apiCalled;
                AliaTransactionLog.QuoteId = quoteId;
                AliaTransactionLog.MembershipFlag = membershipFlag;
                AliaTransactionLog.PacFlag = pacFlag;
                context.ALIATransactionLog.Add(AliaTransactionLog);
                context.SaveChanges();
            }

        }

        public void SaveALIALog(string alfaTransactionNumber, string quoteId, string membershipNumber, string payloadSent, string payloadReceived, string apiCalled, string policyNumber, bool membershipFlag, bool pacFlag, string memberName, bool failNameCheckFlag)
        {
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                ALIATransactionLog AliaTransactionLog = new ALIATransactionLog();
                AliaTransactionLog.AlfaTransactionNumber = alfaTransactionNumber;
                AliaTransactionLog.PolicyNumber = policyNumber;
                AliaTransactionLog.Date = DateTime.Now;
                AliaTransactionLog.MembershipNumber = membershipNumber;
                AliaTransactionLog.PayloadSent = payloadSent;
                AliaTransactionLog.PayloadReceived = payloadReceived;
                AliaTransactionLog.ApiCalled = apiCalled;
                AliaTransactionLog.QuoteId = quoteId;
                AliaTransactionLog.MembershipFlag = membershipFlag;
                AliaTransactionLog.PacFlag = pacFlag;
                AliaTransactionLog.MemberName = memberName;
                AliaTransactionLog.FailNameCheck = failNameCheckFlag;
                context.ALIATransactionLog.Add(AliaTransactionLog);
                context.SaveChanges();
            }
        }

        public string AddMemberNumber(string memberNumber, int quoteId)
        {
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                var stagingPolicy = context.Staging_Policy.Where(sp => sp.QuoteID == quoteId).FirstOrDefault();

                if (stagingPolicy != null)
                {
                    stagingPolicy.MembershipNum = memberNumber;
                    context.SaveChanges();
                    return stagingPolicy.MembershipNum;
                }
                else
                {
                    return "Error. Table did not update.";
                }
            }
        }

        public string GetPolicyNumber(int quoteId)
        {
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                var stagingPolicy = context.Staging_Policy.Where(sp => sp.QuoteID == quoteId).Select(sp => sp.FullPolNum).FirstOrDefault();

                if (stagingPolicy != null)
                {
                    return stagingPolicy;
                }
                return null;
            }
        }

        public int GetQuoteId(string policyNumber)
        {
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                var quoteId = context.Staging_Policy.Where(sp => sp.FullPolNum == policyNumber).Select(sp => sp.QuoteID).FirstOrDefault();

                return quoteId;

            }
        }

        public NewMemberFlags GetMembershipFlags(int quoteId)
        {
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                NewMemberFlags stagingPolicy = context.Staging_Policy.Where(sp => sp.QuoteID == quoteId).Select(sp => new NewMemberFlags { MembershipFlag = sp.MembershipDues, PacFlag = sp.FarmPACFee }).FirstOrDefault();

                if (stagingPolicy != null)
                {
                    return stagingPolicy;
                }
                return null;
            }
        }

        public List<string> VerifyContactName(string quoteId)
        {
            int quoteIdInt = Int32.Parse(quoteId);

            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                List<string> stagingApplicant = context.Staging_Applicant
                     .Where(sa => sa.QuoteID == quoteIdInt)
                     .Select(sa =>
                     sa.InsFirstName.ToUpper() + " " +
                     sa.InsLastName.ToUpper())
                     .ToList();

                return stagingApplicant;
            }
        }

        public List<string> As400VerifyContactName(string policyNumber)
        {
            using (var context = new AlfaVisionWebEntities(AlfaVisionWebConnectionString))
            {
                List<string> includedDriver = context.IncludedDrivers
                     .Where(id => id.PolicyNo == policyNumber && id.DriverType == "P" && id.DriverStatus == "A")
                     .Select(id =>
                     id.DriverName.ToUpper())
                     .ToList();

                return includedDriver;
            }
        }

        public QuoteDataForNewMember GetAFFNewMemberData(int quoteId)
        {
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                List<dynamic> quotes = new List<dynamic>();

                var result = context.Procedures.spGetAFFNewMemberDataAsync(quoteId).Result;

                QuoteDataForNewMember data = new QuoteDataForNewMember();

                foreach (var item in result)
                {
                    dynamic quote = new ExpandoObject();
                    quote.Column1 = item.Column1;
                    quote.InsAddress1 = item.InsAddress1.Substring(0, Math.Min(item.InsAddress1.Length, 50));
                    quote.InsAddress2 = item.InsAddress2.Substring(0, Math.Min(item.InsAddress2.Length, 50));
                    quote.InsCity = item.InsCity.Substring(0, Math.Min(item.InsCity.Length, 20));
                    if (item.InsEmail != null)
                    {
                        quote.InsEmail = item.InsEmail.Substring(0, Math.Min(item.InsEmail.Length, 30));
                    }
                    else
                    {
                        quote.InsEmail = " ";
                    }
                    quote.InsFirstName = item.InsFirstName.Substring(0, Math.Min(item.InsFirstName.Length, 25));
                    quote.InsLastName = item.InsLastName.Substring(0, Math.Min(item.InsLastName.Length, 25));
                    quote.InsState = item.InsState.Substring(0, 2);
                    quote.InsZip = item.InsZip.Substring(0, 5);
                    quote.LoginName = item.LoginName.Substring(0, Math.Min(item.LoginName.Length, 50));
                    quote.agentNumber = item.agentNumber.Substring(0, Math.Min(item.agentNumber.Length, 10));
                    quote.comments = item.comments.Substring(0, Math.Min(item.comments.Length, 1024));
                    quote.dateOfBirth = item.dateOfBirth.ToString();
                    quote.phoneNumber = item.phoneNumber.Replace("-", "");
                    quote.status = item.status.Substring(0, Math.Min(item.status.Length, 25));
                    quote.typeIndicator = item.typeIndicator.Substring(0, Math.Min(item.typeIndicator.Length, 25));
                    if (item.SecondaryFirstName != null)
                    {
                        quote.secondaryFirstName = item.SecondaryFirstName.Substring(0, Math.Min(item.SecondaryFirstName.Length, 25));
                    }
                    else
                    {
                        quote.secondaryFirstName = " ";
                    }
                    if (item.SecondaryLastName != null)
                    {
                        quote.secondaryLastName = item.SecondaryLastName.Substring(0, Math.Min(item.SecondaryLastName.Length, 25));
                    }
                    else
                    {
                        quote.secondaryLastName = " ";
                    }

                    quotes.Add(quote);
                }

                data.serviceCenterNumber = quotes[0].Column1;
                data.addressLine1 = quotes[0].InsAddress1;
                data.addressLine2 = quotes[0].InsAddress2;
                data.addressCity = quotes[0].InsCity;
                data.email = quotes[0].InsEmail;
                data.primaryFirstName = quotes[0].InsFirstName;
                data.primaryLastName = quotes[0].InsLastName;
                data.addressState = quotes[0].InsState;
                data.addressZipCode = quotes[0].InsZip;
                data.agentCsrEmail = quotes[0].LoginName;
                data.agentNumber = quotes[0].agentNumber;
                data.comments = quotes[0].comments;
                data.dateOfBirth = quotes[0].dateOfBirth;
                data.phoneNumber = quotes[0].phoneNumber;
                data.status = quotes[0].status;
                data.typeIndicator = quotes[0].typeIndicator;
                data.secondaryFirstName = quotes[0].secondaryFirstName + " ";
                data.secondaryLastName = quotes[0].secondaryLastName + " ";

                return data;
            }
        }

        public QuoteDataForMemberSearch GetMemberSearchData(int quoteId)
        {
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                List<dynamic> quotes = new List<dynamic>();

                var result = context.Procedures.spGetMemberSearchDataAsync(quoteId.ToString()).Result;

                QuoteDataForMemberSearch data = new QuoteDataForMemberSearch();

                foreach (var item in result)
                {
                    dynamic quote = new ExpandoObject();

                    quote.InsFirstName = item.InsFirstName.Substring(0, Math.Min(item.InsFirstName.Length, 25));
                    quote.InsLastName = item.InsLastName.Substring(0, Math.Min(item.InsLastName.Length, 25));
                    quote.InsAddress1 = item.InsAddress1.Substring(0, Math.Min(item.InsAddress1.Length, 50));
                    quote.InsAddress2 = item.InsAddress2.Substring(0, Math.Min(item.InsAddress2.Length, 50));
                    quote.InsCity = item.InsCity.Substring(0, Math.Min(item.InsCity.Length, 20));
                    quote.InsState = item.InsState.Substring(0, 2);
                    quote.InsZip = item.InsZip.Substring(0, 5);

                    quotes.Add(quote);
                }

                data.firstName = quotes[0].InsFirstName;
                data.lastName = quotes[0].InsLastName;
                data.addressLine1 = quotes[0].InsAddress1;
                data.addressLine2 = quotes[0].InsAddress2;
                data.addressCity = quotes[0].InsCity;
                data.addressState = quotes[0].InsState;
                data.addressZipCode = quotes[0].InsZip;

                return data;
            }
        }

        public void AddAlfaIdCardToWebTables(Guid guid, int pdfOrder, string documentName, string downloadFile, DateTime dateAdded, int quoteId, string fileLocation, string oldFileLocation, string policyNumber)
        {
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                WebPDFs webPdfs = new WebPDFs();
                webPdfs.ticket = guid.ToString();
                webPdfs.pdforder = pdfOrder;
                webPdfs.DocumentName = documentName;
                webPdfs.DownloadFile = downloadFile;
                webPdfs.dateAdded = dateAdded;
                webPdfs.quoteID = quoteId;
                webPdfs.FileLocation = fileLocation;
                webPdfs.OldFileLocation = oldFileLocation;
                webPdfs.PolicyNo = policyNumber;
                context.WebPDFs.Add(webPdfs);
                context.SaveChanges();
            }
        }
    }
}
