using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon.Dto;
using CommonAPIDAL.DataAccess;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using CommonAPICommon;

namespace CommonAPIBusinessLayer.Services.Impl
{
    public class ALIAauthenticationService : IALIAService
    {
        private static readonly log2sql log = new log2sql(nameof(ALIAauthenticationService));
        private static bool validateSSLCert(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors error)
        {
            return true;
        }

        public string CallAlfaWebService(string memberNumber, string zipCode, string quoteId)
        {
            X509Certificate2 cert = GetCert();
            try
            {
                ALIADataAccess aLIADataAccess = new ALIADataAccess();

                string accessToken = GetALIAToken(memberNumber);

                var user = new JObject();
                user.Add("memberNumber", memberNumber.PadLeft(8, '0'));
                user.Add("providerName", "Trexis");
                user.Add("zipCode", "");
                user.Add("token", GlobalParameters.VerifyToken);

                string data2 = JsonConvert.SerializeObject(user);

                string verifyurl = GlobalParameters.VerifyALIAURL;

                var policyNumber = aLIADataAccess.GetPolicyNumber(int.Parse(quoteId)) ?? "0";

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(verifyurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    using (HttpContent alfaJSON = new StringContent(data2, Encoding.UTF8, "application/json"))
                    {
                        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                        var recieved = client.PostAsync("VerifyMembership", alfaJSON).Result;

                        if (recieved.IsSuccessStatusCode)
                        {
                            var result = recieved.Content.ReadAsStringAsync().Result;
                            ALIAMemberNumberDto alfaCallResults = new ALIAMemberNumberDto();
                            alfaCallResults = JToken.Parse(result).ToObject<ALIAMemberNumberDto>();

                            bool applicantMatch = false;

                            if (alfaCallResults.found == "True")
                            {
                                List<string> applicants = aLIADataAccess.VerifyContactName(quoteId);
                                applicantMatch = AliaNameMatch(alfaCallResults, quoteId, applicants);
                            }

                            aLIADataAccess.SaveALIALog(null, quoteId, memberNumber, data2, result, "Verify", policyNumber, false, false, alfaCallResults.memberData.primaryContactName, !applicantMatch);

                            return AliaWebReturn(alfaCallResults, quoteId, policyNumber);
                        }
                        else
                        {
                            log.Error(recieved.IsSuccessStatusCode + quoteId);
                            return "QuoteId";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "False";
            }
        }

        public ALResults CallAlfaWebServiceAs400(string memberNumber, string policyNumber)
        {
            try
            {
                ALIADataAccess aLIADataAccess = new ALIADataAccess();

                string accessToken = GetALIAToken(memberNumber);

                var user = new JObject();
                user.Add("memberNumber", memberNumber.PadLeft(8, '0'));
                user.Add("providerName", "Trexis");
                user.Add("zipCode", "");
                user.Add("token", GlobalParameters.VerifyToken);

                string data2 = JsonConvert.SerializeObject(user);
                string verifyurl = GlobalParameters.VerifyALIAURL;
                string quoteId = aLIADataAccess.GetQuoteId(policyNumber).ToString() ?? "0";

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(verifyurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    using (HttpContent alfaJSON = new StringContent(data2, Encoding.UTF8, "application/json"))
                    {
                        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                        var recieved = client.PostAsync("VerifyMembership", alfaJSON).Result;

                        if (recieved.IsSuccessStatusCode)
                        {
                            var result = recieved.Content.ReadAsStringAsync().Result;
                            ALIAMemberNumberDto alfaCallResults = new ALIAMemberNumberDto();
                            alfaCallResults = JToken.Parse(result).ToObject<ALIAMemberNumberDto>();

                            bool applicantMatch = false;

                            if (alfaCallResults.found == "True")
                            {
                                List<string> applicants = aLIADataAccess.As400VerifyContactName(policyNumber);

                                applicantMatch = AliaNameMatch(alfaCallResults, quoteId, applicants);

                                ALDto alDto = AliaAs400Return(alfaCallResults, quoteId, policyNumber);

                                ALResults aLResults = new ALResults
                                {
                                    ALDto = alDto,
                                    ErrorMessage = ""
                                };

                                var as400Response = " {Status: " + aLResults.ALDto.Status + ", Expiration Date: " + aLResults.ALDto.ExpirationDate + "}";

                                aLIADataAccess.SaveALIALog(null, quoteId, memberNumber, data2, result + as400Response, "AS400", policyNumber, false, false, alfaCallResults.memberData.primaryContactName, !applicantMatch);

                                return aLResults;
                            }
                            else
                            {
                                ALDto aliaAs400Validation = new ALDto
                                {
                                    Status = "Not Found",
                                    ExpirationDate = "00000000"
                                };

                                ALResults aLResults = new ALResults
                                {
                                    ALDto = aliaAs400Validation,
                                    ErrorMessage = ""
                                };

                                var as400Response = " {Status: " + aLResults.ALDto.Status + ", Expiration Date: " + aLResults.ALDto.ExpirationDate + "}";

                                if (alfaCallResults.memberData.primaryContactName == null)
                                {
                                    aLIADataAccess.SaveALIALog(null, quoteId, memberNumber, data2, result + as400Response, "AS400", policyNumber, false, false, null, !applicantMatch);
                                }
                                else
                                {
                                    aLIADataAccess.SaveALIALog(null, quoteId, memberNumber, data2, result + as400Response, "AS400", policyNumber, false, false, alfaCallResults.memberData.primaryContactName, !applicantMatch);
                                }

                                return aLResults;
                            }
                        }
                        else
                        {
                            log.Error(recieved.IsSuccessStatusCode + policyNumber);
                            ALDto aliaAs400Validation = new ALDto
                            {
                                Status = "API Fail",
                                ExpirationDate = "00000000"
                            };

                            ALResults aLResults = new ALResults
                            {
                                ALDto = aliaAs400Validation,
                                ErrorMessage = ""
                            };

                            var as400Response = " {Status: " + aLResults.ALDto.Status + ", Expiration Date: " + aLResults.ALDto.ExpirationDate + "}";

                            aLIADataAccess.SaveALIALog(null, quoteId, memberNumber, data2, as400Response, "AS400", policyNumber, false, false, null, false);

                            return aLResults;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                ALDto aLDto = new ALDto
                {
                    Status = "API Fail",
                    ExpirationDate = "00000000"
                };
                ALResults aliaAs400Validation = new ALResults
                {
                    ALDto = aLDto,
                    ErrorMessage = ex.Message
                };

                return aliaAs400Validation;
            }
        }

        public string AddNewMembershipNumber(QuoteDataForNewMember user, int quoteId)
        {
            X509Store store = new X509Store(StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificateColl = store.Certificates.Find(X509FindType.FindBySerialNumber, GlobalParameters.MembershipCertSerialNumber, false);

            //Logging Certificates
            ALIADataAccess aLIADataAccess = new ALIADataAccess();

            var policyNumber = aLIADataAccess.GetPolicyNumber(quoteId) ?? "0";

            if (certificateColl.Count <= 0)
            {
                aLIADataAccess.SaveALIALog(null, quoteId.ToString(), null, "No Certificate Found", "Cert Count: " + certificateColl.Count.ToString(), "CertLog", policyNumber, false, false, null, false);

                throw new Exception("Certificate not found.");
            }
            else
            {
                aLIADataAccess.SaveALIALog(null, quoteId.ToString(), null, certificateColl[0].ToString(), "Cert Count: " + certificateColl.Count.ToString(), "CertLog", policyNumber, false, false, null, false);
            }

            X509Certificate2 cert = certificateColl[0];

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string data2 = JsonConvert.SerializeObject(user);
            string verifyurl = GlobalParameters.MembershipBaseUrl;

            HttpClientHandler handler = new HttpClientHandler();

            handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert2, cetChain, policyErrors) =>
            { return true; };

            handler.ClientCertificates.Add(cert);

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(verifyurl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("client_id", GlobalParameters.MembershipClientId);
                client.DefaultRequestHeaders.Add("client_secret", GlobalParameters.MembershipClientSecret);

                try
                {
                    using (HttpContent alfaJSON = new StringContent(data2, Encoding.UTF8, "application/json"))
                    {
                        HttpResponseMessage response = client.PostAsync(verifyurl, alfaJSON).Result;

                        var result = response.Content.ReadAsStringAsync().Result;
                        NewMemberData alfaCallResults = new NewMemberData();
                        alfaCallResults = JToken.Parse(result).ToObject<NewMemberData>();


                        string ret;
                        if (response.Headers.Contains("x-alfa-transaction-id"))
                        {
                            var alfaTransactionId = response.Headers.GetValues("x-alfa-transaction-id").First();

                            NewMemberFlags flags = aLIADataAccess.GetMembershipFlags(quoteId);

                            if (response.StatusCode == HttpStatusCode.Created)
                            {
                                ret = "Member Number: " + alfaCallResults.memberNumber;

                                var updateMemberNumber = aLIADataAccess.AddMemberNumber(alfaCallResults.memberNumber, quoteId);

                                aLIADataAccess.SaveALIALog(alfaTransactionId, quoteId.ToString(), alfaCallResults.memberNumber, data2, result, "NewMbrAdd", policyNumber, flags.MembershipFlag, flags.PacFlag, null, false);

                                //Call ALFA Get Id API
                                GetMembershipIdCard(alfaCallResults.memberNumber, alfaTransactionId, quoteId, policyNumber);

                                if (updateMemberNumber.Contains("Error"))
                                    return "Error. Table did not update.";
                                else
                                    return ret;
                            }
                            else
                            {
                                if (alfaCallResults.errors != null)
                                {
                                    ret = "Error: " + alfaCallResults.errors[0];

                                    aLIADataAccess.SaveALIALog(alfaTransactionId, quoteId.ToString(), alfaCallResults.memberNumber, data2, result, "NewMbrAdd", policyNumber, flags.MembershipFlag, flags.PacFlag, null, false);
                                }
                                else
                                {
                                    ret = "Error: " + alfaCallResults.message;

                                    aLIADataAccess.SaveALIALog(alfaTransactionId, quoteId.ToString(), alfaCallResults.memberNumber, data2, result, "NewMbrAdd", policyNumber, flags.MembershipFlag, flags.PacFlag, null, false);
                                }
                                return ret;
                            }
                        }
                        else
                        {
                            ret = "Error: Unable to reach ALFA";
                            return ret;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    var eMsg = string.Empty;
                    if (ex.InnerException == null)
                    {
                        eMsg = "Error: " + ex.Message.ToString();
                    }
                    else
                    {
                        eMsg = "Error: " + ex.InnerException.Message.ToString();
                    }
                    return eMsg;
                }
            }
        }

        public string MemberSearch(QuoteDataForMemberSearch user, int quoteId)
        {
            X509Certificate2 cert = GetCert();

            //X509Store store = new X509Store(StoreLocation.LocalMachine);
            //store.Open(OpenFlags.ReadOnly);
            //X509Certificate2Collection certificateColl = store.Certificates.Find(X509FindType.FindBySerialNumber, GlobalParameters.NewMemberCertSerialNumber, false);

            ALIADataAccess aLIADataAccess = new ALIADataAccess();

            var policyNumber = aLIADataAccess.GetPolicyNumber(quoteId) ?? "0";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string data2 = JsonConvert.SerializeObject(user);
            string memberSearchUrl = GlobalParameters.MembershipBaseUrl + "/search";

            HttpClientHandler handler = new HttpClientHandler();

            handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert2, cetChain, policyErrors) =>
            { return true; };

            handler.ClientCertificates.Add(cert);

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(memberSearchUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("client_id", GlobalParameters.MembershipClientId);
                client.DefaultRequestHeaders.Add("client_secret", GlobalParameters.MembershipClientSecret);
                try
                {
                    using (HttpContent alfaJSON = new StringContent(data2, Encoding.UTF8, "application/json"))
                    {
                        HttpResponseMessage response = client.PostAsync(memberSearchUrl, alfaJSON).Result;
                        var result = response.Content.ReadAsStringAsync().Result;
                        MemberSearchResponse alfaCallResults = new MemberSearchResponse();
                        alfaCallResults = JToken.Parse(result).ToObject<MemberSearchResponse>();

                        string ret;
                        if (response.Headers.Contains("x-alfa-transaction-id"))
                        {
                            var alfaTransactionId = response.Headers.GetValues("x-alfa-transaction-id").First();

                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                ret = "Member Number: " + alfaCallResults.members[0].memberNumber;

                                aLIADataAccess.SaveALIALog(alfaTransactionId, quoteId.ToString(), alfaCallResults.members[0].memberNumber, data2, result, "MbrSearch", policyNumber, false, false, null, false);

                                if (alfaCallResults.members.Count > 1)
                                {
                                    ret = "Multiple records found";
                                    return ret;
                                }
                                return ret;
                            }
                            else
                            {
                                if (alfaCallResults.errors != null)
                                {
                                    ret = "Error: " + alfaCallResults.errors[0];

                                    aLIADataAccess.SaveALIALog(alfaTransactionId, quoteId.ToString(), alfaCallResults.members[0].memberNumber, data2, result, "MbrSearch", policyNumber, false, false, null, false);
                                }
                                else
                                {
                                    ret = "Message: " + alfaCallResults.message;

                                    aLIADataAccess.SaveALIALog(alfaTransactionId, quoteId.ToString(), alfaCallResults.members[0].memberNumber, data2, result, "MbrSearch", policyNumber, false, false, null, false);
                                }
                                return ret;
                            }
                        }
                        else
                        {
                            ret = "Error: Unable to reach ALFA";
                            return ret;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    var eMsg = string.Empty;
                    if (ex.InnerException == null)
                    {
                        eMsg = ex.Message.ToString();
                    }
                    else
                    {
                        eMsg = ex.InnerException.Message.ToString();
                    }
                    return eMsg;
                }
            }
        }

        public QuoteDataForNewMember GetAFFNewMemberData(int quoteId)
        {
            QuoteDataForNewMember user = new ALIADataAccess().GetAFFNewMemberData(quoteId);

            return user;
        }
        public QuoteDataForMemberSearch GetMemberSearchData(int quoteId)
        {
            QuoteDataForMemberSearch user = new ALIADataAccess().GetMemberSearchData(quoteId);

            return user;
        }
        private string GetALIAToken(string memberNumber)
        {
            string url = GlobalParameters.VerifyALIATokenURL;

            WebClient wc = new WebClient();
            wc.QueryString.Add("client_secret", GlobalParameters.VerifyALIATokenClientSecret);
            wc.QueryString.Add("client_id", GlobalParameters.VerifyALIATokenClientId);
            wc.QueryString.Add("redirect_uri", "https://google.com");
            wc.QueryString.Add("grant_type", "password");
            wc.QueryString.Add("username", GlobalParameters.VerifyALIATokenUsername);
            wc.QueryString.Add("password", GlobalParameters.VerifyALIATokenPassword);

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(validateSSLCert);
            var data = wc.UploadValues(url, "POST", wc.QueryString);
            var responseString = Encoding.UTF8.GetString(data);

            ALIAauthenticationDto alfaToken = new ALIAauthenticationDto();
            alfaToken = JToken.Parse(responseString).ToObject<ALIAauthenticationDto>();

            return alfaToken.access_token;
        }

        private bool AliaNameMatch(ALIAMemberNumberDto alfaCallResults, string quoteId, List<string> applicants)
        {
            bool applicantMatch = false;

            string[] alfaNameList = alfaCallResults.memberData.primaryContactName.ToUpper().Replace(",", "").Replace(".", "").Replace("'", "").Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string alfaName;

            switch (alfaNameList.Length)
            {
                case 1:  //see Prince, Messi, Rihanna...
                    alfaName = alfaNameList[0];
                    break;
                case 2:
                    alfaName = alfaNameList[0] + " " + alfaNameList[1];
                    break;
                case 3:
                    if (alfaNameList[2].Length > 3 && alfaNameList[1].Length <= 2)
                    {
                        alfaName = alfaNameList[0] + " " + alfaNameList[2];
                    }
                    else if (alfaNameList[0].Length >= 2 && alfaNameList[1].Length > 2 && alfaNameList[2].Length > 2)
                    {
                        alfaName = alfaNameList[2] + " " + alfaNameList[1];
                    }
                    else
                    {
                        alfaName = alfaNameList[0] + " " + alfaNameList[1];
                    }
                    break;
                case 4:
                    alfaName = alfaNameList[0] + " " + alfaNameList[2];
                    break;
                default:
                    alfaName = alfaNameList[0] + " " + alfaNameList[2];
                    break;
            }

            var soundexService = new SoundexService();

            foreach (var applicant in applicants)
            {
                string[] trimmedApplicantList = Regex.Replace(applicant, @"\s+", " ").Replace(",", "").Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                string trimmedApplicant;
                string trimmedApplicantReverse;

                switch (trimmedApplicantList.Length)
                {
                    case 1: //see Prince, Messi, Rihanna...
                        trimmedApplicant = trimmedApplicantList[0];
                        trimmedApplicantReverse = trimmedApplicantList[0];
                        break;
                    case 2:
                        trimmedApplicant = trimmedApplicantList[0] + " " + trimmedApplicantList[1];
                        trimmedApplicantReverse = trimmedApplicantList[1] + " " + trimmedApplicantList[0];
                        break;
                    case 3:
                        if (trimmedApplicantList[2].Length > 3 && trimmedApplicantList[1].Length <= 2)
                        {
                            trimmedApplicant = trimmedApplicantList[0] + " " + trimmedApplicantList[2];
                            trimmedApplicantReverse = trimmedApplicantList[2] + " " + trimmedApplicantList[0];
                        }
                        else if (trimmedApplicantList[0].Length >= 2 && trimmedApplicantList[1].Length > 2 && trimmedApplicantList[2].Length > 2)
                        {
                            trimmedApplicant = trimmedApplicantList[2] + " " + trimmedApplicantList[1];
                            trimmedApplicantReverse = trimmedApplicantList[1] + " " + trimmedApplicantList[2];
                        }
                        else
                        {
                            trimmedApplicant = trimmedApplicantList[0] + " " + trimmedApplicantList[1];
                            trimmedApplicantReverse = trimmedApplicantList[1] + " " + trimmedApplicantList[2];
                        }
                        break;
                    case 4:
                        trimmedApplicant = trimmedApplicantList[0] + " " + trimmedApplicantList[2];
                        trimmedApplicantReverse = trimmedApplicantList[2] + " " + trimmedApplicantList[0];
                        break;
                    default:
                        trimmedApplicant = trimmedApplicantList[0] + " " + trimmedApplicantList[2];
                        trimmedApplicantReverse = trimmedApplicantList[2] + " " + trimmedApplicantList[0];
                        break;
                }

                if (soundexService.NameComparison(alfaName, trimmedApplicant))
                {
                    applicantMatch = true;
                    break;
                }
            }
            return applicantMatch;
        }
        private string AliaWebReturn(ALIAMemberNumberDto alfaCallResults, string quoteId, string policyNumber)
        {
            PendingPayment pendingPayment = PendingPayment(alfaCallResults, quoteId, policyNumber);

            if (alfaCallResults.success == "1"
                && alfaCallResults.memberData != null
                && pendingPayment.ExpirationDate > DateTime.Now.AddYears(-1)
                && pendingPayment.Status != "Cancelled")
            {
                //returns active or expired
                return pendingPayment.Status;
            }
            else
            {
                if (alfaCallResults.errors == null)
                {
                    //record not found
                    return "False";
                }
                else
                {
                    //record not found due to error
                    return alfaCallResults.errors[0].errorMessage;
                }
            }
        }

        private ALDto AliaAs400Return(ALIAMemberNumberDto alfaCallResults, string quoteId, string policyNumber)
        {
            ALDto aliaAs400Validation = new ALDto();

            PendingPayment pendingPayment = PendingPayment(alfaCallResults, quoteId, policyNumber);

            if (pendingPayment.Status == "Active")
            {
                aliaAs400Validation.Status = pendingPayment.Status;
                DateTime voidDate = pendingPayment.ExpirationDate ?? DateTime.Now;
                string voidDateString = voidDate.ToString("yyyyMMdd");
                aliaAs400Validation.ExpirationDate = voidDateString;
            }
            else
            {
                aliaAs400Validation.Status = "Not Found";
                aliaAs400Validation.ExpirationDate = "00000000";
            }

            return aliaAs400Validation;
        }

        private void GetMembershipIdCard(string membershipId, string alfaTransactionId, int quoteId, string policyNumber)
        {
            X509Certificate2 cert = GetCert();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string IdCardUrl = GlobalParameters.MembershipBaseUrl + "/" + membershipId + "/id-card";

            HttpClientHandler handler = new HttpClientHandler();

            handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert2, cetChain, policyErrors) =>
            { return true; };

            handler.ClientCertificates.Add(cert);

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(IdCardUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("client_id", GlobalParameters.MembershipClientId);
                client.DefaultRequestHeaders.Add("client_secret", GlobalParameters.MembershipClientSecret);
                client.DefaultRequestHeaders.Add("X-ALFA-TRANSACTION-ID", alfaTransactionId);

                string basePath = GlobalParameters.PDFBasePath + "\\GeneratedPDFs\\";

                Guid guid = Guid.NewGuid();

                string pdfFileLocation = basePath + DateTime.Now.ToString("yyyy-MM") + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\" + "AlfaIdCard-" + guid + "_" + quoteId + ".pdf";

                ALIADataAccess aLIADataAccess = new ALIADataAccess();
                DocumentationService documentationService = new DocumentationService();

                try
                {
                    //Call ALFA Membership Id Card API to generate Id Card
                    HttpResponseMessage response = client.GetAsync(IdCardUrl).Result;
                    var result = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        aLIADataAccess.SaveALIALog(alfaTransactionId, quoteId.ToString(), membershipId, "No Payload", result, "IdCard", policyNumber, false, false, null, false);

                        ALIAIdCardDto idCardData = new ALIAIdCardDto();
                        idCardData = JToken.Parse(result).ToObject<ALIAIdCardDto>();

                        byte[] pdf = Convert.FromBase64String(idCardData.data);

                        documentationService.GetAlfaMembershipIdCard(pdf, quoteId, policyNumber);
                    }
                    else
                    {
                        aLIADataAccess.SaveALIALog(alfaTransactionId, quoteId.ToString(), membershipId, "No Payload", result, "IdCard", policyNumber, false, false, null, false);

                        //Use backup PDF if the call to ALFA to generate the PDF fails. Gets PDF from Resources folder in this project.
                        string path = AppContext.BaseDirectory;
                        log.Debug("Static Id Card Base Directory: " + path);
                        string backupPDFpath = Path.GetFullPath(Path.Combine(path, @"bin\Content\Membership Card Failure.pdf"));
                        log.Debug("Static Id Card Path: " + backupPDFpath);
                        byte[] backupPdf = System.IO.File.ReadAllBytes(backupPDFpath);
                        log.Debug("Static Id Card backupPdf: " + backupPdf);

                        documentationService.GetAlfaMembershipIdCard(backupPdf, quoteId, policyNumber);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    //Use backup PDF if the call to ALFA to generate the PDF fails. Gets PDF from Resources folder in this project.
                    string path = AppContext.BaseDirectory;
                    string backupPDFpath = Path.GetFullPath(Path.Combine(path, @"bin\Content\Membership Card Failure.pdf"));

                    byte[] backupPdf = System.IO.File.ReadAllBytes(backupPDFpath);

                    documentationService.GetAlfaMembershipIdCard(backupPdf, quoteId, policyNumber);
                }
            }
        }

        private PendingPayment PendingPayment(ALIAMemberNumberDto alfaCallResults, string quoteId, string policyNumber)
        {
            PendingPayment pendingPayment = new PendingPayment();

            pendingPayment.Status = alfaCallResults.memberData.currentStatus;
            pendingPayment.ExpirationDate = alfaCallResults.memberData.voidAfterdate;
            //Check for pending payment file if returned status is null or expired
            if (pendingPayment.Status == null || pendingPayment.Status == "Expired" || pendingPayment.Status == "Cancelled")
            {
                bool pendingPaymentFileExists = PendingPaymentFileExists(alfaCallResults.memberData.membershipNumber, quoteId, policyNumber);

                if (pendingPaymentFileExists)
                {
                    pendingPayment.Status = "Active";
                    pendingPayment.ExpirationDate = DateTime.Now.AddYears(1);
                }
            }

            if (pendingPayment.ExpirationDate == null)
            {
                pendingPayment.ExpirationDate = DateTime.Now.AddYears(-2);
            }

            return pendingPayment;
        }

        private bool PendingPaymentFileExists(string membershipNumber, string quoteId, string policyNumber)
        {
            try
            {
                ALIADataAccess aLIADataAccess = new ALIADataAccess();

                string accessToken = GetALIAToken(membershipNumber);

                var user = new JObject();
                user.Add("memberNumber", membershipNumber.PadLeft(8, '0'));
                user.Add("providerName", "Trexis");
                user.Add("zipCode", "");
                user.Add("token", GlobalParameters.VerifyToken);

                string data2 = JsonConvert.SerializeObject(user);

                string pendingPaymentFileUrl = GlobalParameters.MembershipBaseUrl + $"/{membershipNumber}/paymentstatus";

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(pendingPaymentFileUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    client.DefaultRequestHeaders.Add("client_id", GlobalParameters.MembershipClientId);
                    client.DefaultRequestHeaders.Add("client_secret", GlobalParameters.MembershipClientSecret);

                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                    var recieved = client.GetAsync(pendingPaymentFileUrl).Result;
                    var result = recieved.Content.ReadAsStringAsync().Result;

                    aLIADataAccess.SaveALIALog(null, quoteId, membershipNumber, data2, result, "PendPay", policyNumber, false, false, null, false);

                    if (recieved.IsSuccessStatusCode)
                    {
                        ALIAPendingPaymentDto aliaPendingPaymentCallResults = new ALIAPendingPaymentDto();
                        aliaPendingPaymentCallResults = JToken.Parse(result).ToObject<ALIAPendingPaymentDto>();
                        if (aliaPendingPaymentCallResults.PendingPaymentStatus.ToUpper() == "PENDING")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                        return false;
                    }


                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        private X509Certificate2 GetCert()
        {
            X509Store store = new X509Store(StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificateColl = store.Certificates.Find(X509FindType.FindBySerialNumber, GlobalParameters.MembershipCertSerialNumber, false);

            return certificateColl[0];
        }
    }
}
