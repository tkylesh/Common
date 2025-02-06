using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using CommonAPICommon.Dto;
using Microsoft.IdentityModel.Protocols;
using LexisNexisService;
using System.ServiceModel;
using System.ServiceModel.Channels;
using CommonAPIDAL.VisionAppModels;
using CommonAPICommon;
using CommonAPIDAL.DataAccess.Mapping;
using System.Configuration;
using CommonAPIDAL.Repository.Impl;
using CommonAPICommon.Dto;

namespace CommonAPIDAL.DataAccess
{
    public class LexisNexisDataAccess
    {
        LexisNexisService.Request request = new LexisNexisService.Request();
        private string LexisNexisUserId
        {
            get
            {
                return RijndaelSimple.DoRijndael(ConfigurationManager.AppSettings["LexisNexisUserName"].ToString(), EncrytionDirection.Decrypt);
            }
        }
        private string LexisNexisPassword
        {
            get
            {
                return RijndaelSimple.DoRijndael(ConfigurationManager.AppSettings["LexisNexisPassword"].ToString(), EncrytionDirection.Decrypt);
            }
        }
        private string LexisNexisAccountNumber(string LNprefillStateAcctNbr)
        {
            {
                return RijndaelSimple.DoRijndael(LNprefillStateAcctNbr, EncrytionDirection.Decrypt);
            }
        }

        /// <summary>
        /// This mehtod builds the request for the Lexis Nexis Auto Data Prefill call.
        /// </summary>
        /// <param name="_firstName"></param>
        /// <param name="_lastName"></param>
        /// <param name="_middleName"></param>
        /// <param name="_address"></param>
        /// <param name="_city"></param>
        /// <param name="_state"></param>
        /// <param name="_zip"></param>
        /// <param name="_programMaster"></param>
        public LexisNexisDataAccess(string _firstName, string _lastName, string _middleName, string _address, string _city, string _state, string _zip, ProgramMaster _programMaster)
        {
            //RequesterInfo requried
            var info = new RequesterInformation();
            info.AccountNumber = LexisNexisAccountNumber(_programMaster.LexisNexisPrefillStateAcctNbr.SafeTrim());

            //TransactionDetails required
            var transDetails = new TransactionDetails();
            transDetails.RuleplanId = Convert.ToInt16(ConfigurationManager.AppSettings["LexisNexisPrefillRulePlan"]);

            //SearchBy required
            var searchBy = new SearchBy();
            var subject = new Subject();
            subject.subjectID = "S1";

            //name
            var name = new Name();
            name.First = _firstName;
            name.Last = _lastName;
            subject.Name = name;

            ////SSN
            //subject.SSN = "";

            ////DOB
            //Date date = new Date();
            //date.Year = 1942;
            //date.Month = 7;
            //date.Day = 12;
            //subject.DOB = date;

            //current address
            var address = new AddressType();
            address.StreetAddress1 = _address;
            address.City = _city;
            address.State = _state;
            address.Zip5 = _zip;
            subject.CurrentAddress = address;
            subject.CurrentAddress.addressId = "A1";

            //drivers license
            var dl = new DriversLicense();
            //dl.DriversLicenseNumber = "";  
            dl.TypeSpecified = true;
            dl.Type = DriversLicenseTypeEnum.Personal;
            subject.DriversLicense = dl;
            var subjects = new Subject[1];
            subjects[0] = subject;
            searchBy.Subjects = subjects;

            //products
            var products = new ProductChoiceType();
            var prefill = new InquiryAutoDataPrefill();
            var autofill = new InquiryAutoDataPrefill[1];
            var prefillSearchBy = new InquiryAutoDataPrefillSearchBy();
            var productSubjects = new InquiryAutoDataPrefillSearchBySubjects();
            var productSubject = new IdentificationType();
            productSubject.@ref = "S1";
            productSubjects.Subject = productSubject;
            prefillSearchBy.Subjects = productSubjects;
            prefill.SearchBy = prefillSearchBy;
            autofill[0] = prefill;
            products.AutoDataPrefill = autofill;

            //Build request
            request.Products = products;
            request.RequesterInformation = info;
            request.TransactionDetails = transDetails;
            request.SearchBy = searchBy;
        }

        /// <summary>
        /// This method makes the Lexis Nexis Prefill call and get the response.
        /// </summary>
        /// <param name="quoteId"></param>
        /// <param name="LNisoType"></param>
        /// <returns></returns>
        public async Task<string[]> GetLexisNexisResponseDataAsync(int quoteId, string LNisoType, ApplicantDto applicant)
        {
            var IsoRepository = new ISORepository();
            var lexisNexisMapper = new LexisNexisMapper();
            var prefillDataAccess = new PrefillDataAccess();
            var startTime = DateTime.Now;
            var ts = new TimeSpan();

            // Build client to send request
            var binding = new BasicHttpsBinding();
            binding.MaxReceivedMessageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PrefillMaxReceivedMessageSize"]);
            binding.MaxBufferSize = Convert.ToInt32(ConfigurationManager.AppSettings["PrefillMaxBufferSize"]);

            var remoteAddress = new EndpointAddress(@ConfigurationManager.AppSettings["LexisNexisEndPointAddress"]);
            var client = new RiskXMLOrderHandlerClient(binding, remoteAddress);

            // Build and send request with auth header to get the response
            //https://stackoverflow.com/questions/4080986/authorization-header-is-missing-in-http-request-using-wcf

            client.ClientCredentials.UserName.UserName = LexisNexisUserId;
            client.ClientCredentials.UserName.Password = LexisNexisPassword;

            // Build Security Protocol TLS 1.2
            //https://stackoverflow.com/questions/45465731/how-to-fix-the-server-certificate-is-not-configured-properly-with-http-sys-on/48100471

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Basic Authetication
            var httpRequestProperty = new HttpRequestMessageProperty();
            httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(client.ClientCredentials.UserName.UserName + ":" + client.ClientCredentials.UserName.Password));

            // May have to run in visual studio 2015 if errors on client.InnerChannel
            // handleRequestResponse Response = null;
            var context = new OperationContext(client.InnerChannel);

            using (new OperationContextScope(context))  // Making request and getting response
            {
                context.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                var task = await Task.FromResult(client.handleRequestAsync(request));

                if (task.Result != null)    // recieved a response
                {
                    if (task.Result.ResponseEx.Response.TransactionDetailsEx.ProcessingStatus.ToUpper() == "COMPLETE SUCCESS")    // complete success call
                    {
                        var response = task?.Result?.ResponseEx?.Response;
                        var processingStatus = response?.TransactionDetailsEx?.ProcessingStatus;
                        var transactionID = response?.TransactionDetailsEx?.TransactionId;
                        var requestXml = lexisNexisMapper.SerializeRequest(request);
                        var unMappedResponseXml = lexisNexisMapper.SerializeResponse(response);
                        var lexId = response?.ProductResults?.AutoDataPrefillResults[0]?.SearchDataSet?.Subjects[0]?.LexID ?? null;
                        ts = new TimeSpan(DateTime.Now.Ticks - startTime.Ticks);

                        // logging
                        var isoMasterId = IsoRepository.StoreRawAndMaster(quoteId, unMappedResponseXml, ts.Ticks, processingStatus, requestXml, transactionID, LNisoType, applicant, lexId);
                        prefillDataAccess.SaveLNPrefillOrderHistory(quoteId, isoMasterId);      // this saves all calls to Lexis Nexis and reuses of data from previous calls

                        // mapping
                        var _iso = lexisNexisMapper.mapPrefill(response, quoteId, isoMasterId);
                        var mappedResponseXml = lexisNexisMapper.SerializeISO(_iso);

                        var returnResponse = new List<string>();
                        returnResponse.Add(string.Empty);
                        returnResponse.Add(processingStatus);
                        returnResponse.Add(transactionID);
                        returnResponse.Add(mappedResponseXml);
                        returnResponse.Add(unMappedResponseXml);
                        returnResponse.Add(isoMasterId.ToString());

                        return returnResponse.ToArray();
                    }
                    else    // not a complete success call
                    {
                        var response = task?.Result?.ResponseEx?.Response;
                        var processingStatus = response?.TransactionDetailsEx?.ProcessingStatus;
                        var transactionID = response?.TransactionDetailsEx?.TransactionId;
                        var requestXml = lexisNexisMapper.SerializeRequest(request);
                        var unMappedResponseXml = lexisNexisMapper.SerializeResponse(response);
                        var lexId = response?.ProductResults?.AutoDataPrefillResults[0]?.SearchDataSet?.Subjects[0]?.LexID ?? null;
                        ts = new TimeSpan(DateTime.Now.Ticks - startTime.Ticks);

                        // logging
                        var isoMasterId = IsoRepository.StoreRawAndMaster(quoteId, unMappedResponseXml, ts.Ticks, processingStatus, requestXml, transactionID, LNisoType, applicant, lexId);
                        prefillDataAccess.SaveLNPrefillOrderHistory(quoteId, isoMasterId);      // this saves all calls to Lexis Nexis and reuses of data from previous calls
                        prefillDataAccess.LogPrefillErrors(quoteId, response, LNisoType, processingStatus, transactionID, null);

                        // mapping
                        var _iso = lexisNexisMapper.mapPrefill(response, quoteId, isoMasterId);
                        var mappedResponseXml = lexisNexisMapper.SerializeISO(_iso);

                        var returnResponse = new List<string>();
                        returnResponse.Add("send email");
                        returnResponse.Add(processingStatus);
                        returnResponse.Add(transactionID);
                        returnResponse.Add(mappedResponseXml);
                        returnResponse.Add(unMappedResponseXml);
                        returnResponse.Add(isoMasterId.ToString());

                        return returnResponse.ToArray();
                    }
                }
                else
                {
                    // no response call
                    var processingStatus = "Communication Failure: No Response";
                    var requestXml = lexisNexisMapper.SerializeRequest(request);
                    ts = new TimeSpan(DateTime.Now.Ticks - startTime.Ticks);

                    // logging
                    IsoRepository.StoreRawOnly(quoteId, ts.TotalMilliseconds, processingStatus, requestXml, null, LNisoType);
                    prefillDataAccess.LogPrefillErrors(quoteId, null, LNisoType, processingStatus, string.Empty, null);

                    var returnResponse = new List<string>();
                    returnResponse.Add("send email");
                    returnResponse.Add(processingStatus);
                    returnResponse.Add(string.Empty);

                    return returnResponse.ToArray();
                }
            }
        }
    }
}
