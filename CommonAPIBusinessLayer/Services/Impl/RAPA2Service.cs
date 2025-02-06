using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon.Dto;
using CommonAPIDAL.Repository.Impl;
using Newtonsoft.Json;
using System.Configuration;
using CommonAPICommon;
using CommonAPIDAL.Repository.Interface;

namespace CommonAPIBusinessLayer.Services.Impl
{
    public class RAPA2Service : IRAPA2Service
    {
        string vsr = "";
        Rapa2LimitsDto rapa2Limits = new Rapa2LimitsDto();
        private IVehicleLookupRepository repository = null;

        private static readonly log2sql log = new log2sql(nameof(RAPA2Service));

        private Rapa2LookupRepository RAPA2Repository = new Rapa2LookupRepository();

        public RAPA2Service()
        {
            repository = new VehicleLookupRAPARepository();
        }

        public async Task<VINResultDto> GetVINResults(string vin, Dictionary<string, string> RapaParmDict, string RapaParm)
        {
            //Need some parm values
            string ParmResult = string.Empty;
            string ParmModel = string.Empty;
            string ParmMake = string.Empty;
            string policy = "";
            string state = string.Empty;
            int quoteId = 0;
            if (RapaParmDict.ContainsKey("POLICY")) { policy = RapaParmDict["POLICY"]; }
            if (RapaParmDict.ContainsKey("QUOTEID")) { quoteId = int.TryParse(RapaParmDict["QUOTEID"], out int i) ? i : 0; }
            if (RapaParmDict.ContainsKey("RESULT")) { ParmResult = RapaParmDict["RESULT"]; }
            if (RapaParmDict.ContainsKey("MODEL")) { ParmModel = RapaParmDict["MODEL"]; }
            if (RapaParmDict.ContainsKey("MAKE")) { ParmMake = RapaParmDict["MAKE"]; }
            if (RapaParmDict.ContainsKey("STATE")) { state = RapaParmDict["STATE"]; }


            rapa2Limits = RAPA2Repository.GetRapa2Limits(state);

            //Call verisk to get token and response
            var thisResponse = await GetVeriskVinData(vin, quoteId, policy, rapa2Limits);

            VINResultDto result = new VINResultDto();
            if (thisResponse.Body[0].message == "Found")
            {   //Got results
                result = ProcessVariskData(thisResponse, vin, rapa2Limits);
            }
            else
            {
                //No results.  Use defaults
                result = ProcessVeriskNotFound(thisResponse, vin);
            }

            //Write the audit log
            int HDRId = RAPA2Repository.Rapa2WriteAuditHdr(vsr, thisResponse, quoteId, policy, vin, RapaParm);
            if (thisResponse.Body[0].message == "Found") //Yeah, we checked this earlier, but it wasn't the place to write audit yet, so here we are. AKA don't write details if we don't have any.
            {
                RAPA2Repository.Rapa2WriteAuditDetail(thisResponse, HDRId, vin);
            }

            //See if we require a single return item - AS400 renewals and maybe outside raters
            if (result.VinItems.Count > 1 && ParmResult.ToUpper() == "ONLY1")
            {
                //Do the rules thing
                result = Rapa2RenewalProcessing(result, ParmMake, ParmModel, HDRId);
            }
            return result;
        }

        public static VINResultDto ProcessVeriskNotFound(Rapa2VinResponseDto rapa2VinResponse, string searchedVin)
        {

            VINResultDto thisVinResult = new VINResultDto();

            thisVinResult.Supplemental = false;
            thisVinResult.ErrorMessage = rapa2VinResponse.Body[0].details;
            thisVinResult.ThirdPartyCalled = false;
            VINItemDto vinItems = new VINItemDto();
            vinItems.PassedVIN = searchedVin;

            vinItems.PIPSymbol = "Q9";
            vinItems.PDSymbol = "Q9";
            vinItems.BiSymbol = "Q9";
            vinItems.MedPaySymbol = "Q9";
            vinItems.CollSymbol = "Q9";
            vinItems.CompSymbol = "Q9";
            List<VINItemDto> items = new List<VINItemDto>();
            items.Add(vinItems);
            thisVinResult.VinItems = items;

            return thisVinResult;
        }
        public static VINResultDto ProcessVariskData(Rapa2VinResponseDto rapa2VinResponse, string searchedVin, Rapa2LimitsDto rapa2Limitstate)
        {
            int seq = 0; //Added this as a key for vin selection in RTR
            Rapa2LookupRepository repository = new Rapa2LookupRepository();
            VINResultDto thisVinResult = new VINResultDto();
            List<VINItemDto> items = new List<VINItemDto>();

            thisVinResult.Supplemental = false;
            thisVinResult.ErrorMessage = string.Empty;
            thisVinResult.ThirdPartyCalled = false;
            if (rapa2VinResponse.Body.Length == 1)
            {
                thisVinResult.ISODirectMatch = true;
            }
            else
            {
                thisVinResult.ISODirectMatch = false;
            }

            //Check the weight and msrp - send error messages

            try
            {
                foreach (Body body in rapa2VinResponse.Body)
                {
                    seq += 1;
                    VINItemDto vinItems = new VINItemDto();
                    vinItems.PassedVIN = searchedVin;
                    vinItems.MatchedVIN = body.Vehicle.VIN;
                    vinItems.ModelYear = Convert.ToInt16(body.Vehicle.ModelYear);
                    if (body.Vehicle.DistributionDate.Length == 4) //apparently we get no distribution date on newer entries
                    {
                        vinItems.EffDate = Convert.ToDateTime(body.Vehicle.DistributionDate.Substring(2, 2) + "-01-" + body.Vehicle.DistributionDate.Substring(0, 2));
                    }
                    vinItems.MakeCode = body.Vehicle.Make;
                    vinItems.MakeId = repository.GetOldMakeID(body.Vehicle.Make);
                    vinItems.Make = repository.GetOldMakeDesc(body.Vehicle.Make);
                    vinItems.ShortModel = body.Vehicle.BasicModelName;
                    vinItems.FullModel = body.Vehicle.FullModelName;
                    vinItems.ISOSymbol = "";
                    vinItems.CompSymbol = body.PhysicalDamage.RiskAnalyzerComprehensiveIndicatedSymbol;
                    vinItems.CollSymbol = body.PhysicalDamage.RiskAnalyzerCollisionRatingSymbol;
                    vinItems.VinId = 1; //Used to be the id from the vinmaster table. 
                    vinItems.BiSymbol = body.Liability.RiskAnalyzerBodilyInjuryIndicatedSymbol;
                    vinItems.PDSymbol = body.Liability.RiskAnalyzerPropertyDamageIndicatedSymbol;
                    vinItems.MedPaySymbol = body.Liability.RiskAnalyzerMedicalPaymentsIndicatedSymbol;
                    vinItems.PIPSymbol = body.Liability.RiskAnalyzerPersonalInjuryProtectionIndicatedSymbol;
                    vinItems.RestraintInd = body.Vehicle.RestraintInfo;
                    vinItems.AntiTheftInd = body.Vehicle.AntiTheftIndicator;
                    vinItems.FourWheelDriveInd = body.Vehicle.FourWheelDriveIndicator;
                    vinItems.ISONumber = string.Empty;
                    vinItems.Cylinders = body.Vehicle.EngineCylinders;
                    vinItems.EngineType = body.Vehicle.EngineType;
                    vinItems.EngineSize = body.Vehicle.EngineSize;
                    vinItems.EngineInfo = body.Vehicle.EngineInfo;
                    vinItems.ClassCode = body.Vehicle.ClassCode;
                    vinItems.DaytimeRunningLightsInd = body.Vehicle.DaytimeRunningLightIndicator;
                    vinItems.AntiLockInd = body.Vehicle.AntiLockBrakes;
                    vinItems.WheelbaseInfo = body.Vehicle.Wheelbase;
                    vinItems.BodyStyle = body.Vehicle.BodyStyle;
                    vinItems.BodyStyleDesc = repository.GetBodyStyleDesc(body.Vehicle.BodyStyle);
                    vinItems.TransmissionInfo = body.Vehicle.TransmissionInfo;
                    vinItems.StateException = body.Vehicle.StateException;
                    vinItems.NCIC_Manufacturer = body.Vehicle.NCICCode;
                    vinItems.SpecialInfoSelector = body.Vehicle.SpecialInfoSelector;
                    vinItems.LiabilitySymbol = body.Liability.RiskAnalyzerSingleLimitIndicatedSymbol;
                    vinItems.BaseMSRP = body.Vehicle.BaseMSRP;
                    vinItems.GrossVehicleWeight = body.Vehicle.GrossVehicleWeight;
                    if (int.TryParse(vinItems.BaseMSRP, out int i))
                    {
                        if (i > rapa2Limitstate.MSRPLimit)
                        { vinItems.UnacceptableVehicleReason = "MSRP"; }
                    }
                    if (int.TryParse(vinItems.GrossVehicleWeight, out int g))
                    {
                        if (g > rapa2Limitstate.WeightLimit)
                        { vinItems.UnacceptableVehicleReason = "Weight"; }
                    }
                    vinItems.seq = seq;
                    items.Add(vinItems);
                }
                thisVinResult.VinItems = items;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return thisVinResult;
        }

        public DateTime IntDateToDate(int intDate)
        {
            string stringIntDate = intDate.ToString();
            string stringDate = stringIntDate.Substring(2, 2) + "-01-" + stringIntDate.Substring(0, 2);
            return Convert.ToDateTime(stringDate);
        }

        public async Task<Rapa2VinResponseDto> GetVeriskVinData(string vin, int quoteId, string policyNbr, Rapa2LimitsDto rapa2Limits)
        {
            TokenDto rapa2Token = new TokenDto();
            Rapa2VinResponseDto vinResponse = new Rapa2VinResponseDto();
            Rapa2NotFoundDto vinResponseNotFound = new Rapa2NotFoundDto();
            //NFBody nfBody = new NFBody();
            NFHeader nFHeader = new NFHeader();
            Body responseBody = new Body();
            Rapa2VinRequestDto vinRequest = new Rapa2VinRequestDto();
            CommonAPICommon.Dto.Authorization auth = new CommonAPICommon.Dto.Authorization();
            RequestBody bod = new RequestBody();
            RequestHeader heder = new RequestHeader();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                HttpClient tokenClient = new HttpClient();

                HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings["Rapa2TokenStore"]);

                //var url = new Uri("https://gatewayuat.verisk.com/token");
                var tokenCollection = new List<KeyValuePair<string, string>>();
                tokenCollection.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                var content = new FormUrlEncodedContent(tokenCollection);
                tokenRequest.Content = content;
                string rawToken = ConfigurationManager.AppSettings["Rapa2Key"].ToString() + ":" + ConfigurationManager.AppSettings["Rapa2Secret"].ToString();
                var tokenByteArray = Encoding.ASCII.GetBytes(rawToken);
                tokenClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(tokenByteArray));
                var response = await tokenClient.SendAsync(tokenRequest).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = await response.Content.ReadAsStringAsync();
                    rapa2Token = JsonConvert.DeserializeObject<TokenDto>(tokenResponse);
                }
            }
            catch (HttpRequestException ex)
            {
                log.Error(ex.Message, ex);
            }
            //string tokenResponse = response.Content.ReadAsStringAsync().Result;

            auth.OrgID = ConfigurationManager.AppSettings["Rapa2OrgID"];
            auth.ShipId = ConfigurationManager.AppSettings["Rapa2ShipId"];
            bod.VIN = vin; // "KM8SB12B84U773753";
            if (rapa2Limits.UseCappedSymbols)
            {
                bod.StateCode = rapa2Limits.State;
            }

            heder.Authorization = auth;
            vinRequest.Body = bod;
            vinRequest.Header = heder;

            StringContent requestBody = new StringContent(JsonConvert.SerializeObject(vinRequest), Encoding.UTF8, "application/json");

            using (HttpClient vinClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings["Rapa2ApiAddress"]);
                request.Headers.Add("Accept", "application/json");
                //request.Headers.Add("Content-Type", "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", rapa2Token.access_token);
                request.Content = requestBody;
                var vinSymbolResponse = await vinClient.SendAsync(request);
                vsr = vinSymbolResponse.Content.ReadAsStringAsync().Result;
            }
            if (vsr.Contains("message")) //Got no hit on the full vin, try again with just the first 10
            {
                bod.VIN = bod.VIN.Substring(0, 10);
                requestBody = new StringContent(JsonConvert.SerializeObject(vinRequest), Encoding.UTF8, "application/json");
                using (HttpClient vinClientResend = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings["Rapa2ApiAddress"]);
                    request.Headers.Add("Accept", "application/json");
                    //request.Headers.Add("Content-Type", "application/json");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", rapa2Token.access_token);
                    request.Content = requestBody;
                    var vinSymbolResponse = await vinClientResend.SendAsync(request);
                    vsr = vinSymbolResponse.Content.ReadAsStringAsync().Result;
                }
            }

            if (vsr.Contains("message")) //200 response but no results so we get a different object
            {
                vinResponseNotFound = JsonConvert.DeserializeObject<Rapa2NotFoundDto>(vsr);
                Body[] nfbodies = new Body[1];
                Body nfbody = new Body();
                nfbody.message = vinResponseNotFound.Body.message;
                nfbody.details = vinResponseNotFound.Body.details;
                nfbodies[0] = nfbody;
                vinResponse.Body = nfbodies;
            }
            else //Good response
            {
                vinResponse = JsonConvert.DeserializeObject<Rapa2VinResponseDto>(vsr);
                vinResponse.Body[0].message = "Found";
            }

            return (vinResponse);
        }

        //public VeriskOptions GetVeriskLimits(Dictionary<string, string> RapaParmDict)
        //{
        //    VeriskOptions options = new VeriskOptions();

        //    if (RapaParmDict.ContainsKey("policy")) { options.Policy = RapaParmDict["policy"]; }
        //    if (RapaParmDict.ContainsKey("quoteid")) { options.QuoteId = RapaParmDict["quoteid"]; }
        //    if (RapaParmDict.ContainsKey("progRevId")) { options.ProgRevId = RapaParmDict["progRevId"]; }
        //    if (RapaParmDict.ContainsKey("rateBook")) { options.RateBook = RapaParmDict["rateBook"]; }
        //    if (RapaParmDict.ContainsKey("Renewal")) { options.RunRenewalLogic = (RapaParmDict["Renewal"] == "true"); }
        //}

        public VINResultDto Rapa2RenewalProcessing(VINResultDto thisResult, string make, string model, int HDRId)
        {
            //This isn't a knife fight, so there are rules.  The point is to only send one row back to the 400 as this call comes
            //from a batch job.  
            //1.If different Makes, eliminate record(s) that don’t match the AS400 Make
            //2.If different Basic Models, eliminate record(s) that don’t match the AS400 Model(need to talk about spelling)(using first 10 of full model here)
            //2.1415926 The 400 may have short model or full model depending on when and where it got model data. (using short model here) 
            //3.If different Engine Types, eliminate record(s) where Engine Type is H(hybrid) or T(turbo)
            //4.If different Four-Wheel Drive Indicators, eliminate record(s) where Four-Wheel Drive Indicator = 4
            //5.Choose record with lowest BI Symbol Factor, if the same go to next step
            //6.Choose record with lowest PD Symbol Factor, if the same go to next step
            //7.Choose record with lowest COL Symbol Factor, if the same go to next step
            //8.Pick the first record as a last resort.

            int makecount = 0;
            int modelcount = 0;
            int shortmodelcount = 0;
            int engineTypecount = 0;
            int fourWheelDrivecount = 0;
            int vinresultRowCount = thisResult.VinItems.Count;

            foreach (VINItemDto vinItem in thisResult.VinItems) //Count mismatches on the make and model from the 400 along with engine type H and T and the 4x4 indicator
            {
                if (vinItem.MakeCode.ToUpper() != make.ToUpper()) { makecount++; }
                if (vinItem.FullModel.Substring(0, 10).ToUpper() != model.ToUpper()) { modelcount++; }
                if (vinItem.ShortModel.ToUpper() != model.ToUpper()) { shortmodelcount++; }
                if (vinItem.EngineType == "H" || vinItem.EngineType == "T") { engineTypecount++; }
                if (vinItem.FourWheelDriveInd == "4") { fourWheelDrivecount++; }
            }
            //Rule 1
            if (vinresultRowCount - makecount == 1) //If match math leaves one row, delete the rows that don't match and return            
            {
                foreach (VINItemDto vinItem in thisResult.VinItems.ToList())
                {
                    if (vinItem.MakeCode != make.ToUpper())
                    {
                        thisResult.VinItems.Remove(vinItem);
                    }
                }
                RAPA2Repository.Rapa2UpdateSelectedRecord(HDRId, thisResult.VinItems[0].seq);
                return (thisResult);
            }
            //Rule 2
            if (vinresultRowCount - modelcount == 1) //If first 10 of the full model match math leaves one row, delete the rows that don't match and return
            {
                foreach (VINItemDto vinItem in thisResult.VinItems.ToList())
                {
                    if (vinItem.FullModel.Substring(0, 10).ToUpper() != model.ToUpper())
                    {
                        thisResult.VinItems.Remove(vinItem);
                    }
                }
                RAPA2Repository.Rapa2UpdateSelectedRecord(HDRId, thisResult.VinItems[0].seq);
                return (thisResult);
            }
            //Rule 2.1415926
            if (vinresultRowCount - shortmodelcount == 1) //If model match math leaves one row, delete the rows that don't match and return
            {
                foreach (VINItemDto vinItem in thisResult.VinItems.ToList())
                {
                    if (vinItem.ShortModel.ToUpper() != model.ToUpper())
                    {
                        thisResult.VinItems.Remove(vinItem);
                    }
                }
                RAPA2Repository.Rapa2UpdateSelectedRecord(HDRId, thisResult.VinItems[0].seq);
                return (thisResult);
            }
            //Rule 3
            if (vinresultRowCount - engineTypecount == 1) //If engine type match math leaves one row, delete the rows that don't match and return
            {
                foreach (VINItemDto vinItem in thisResult.VinItems.ToList())
                {
                    if (vinItem.EngineType == "H" || vinItem.EngineType == "T")
                    {
                        thisResult.VinItems.Remove(vinItem);
                    }
                }
                RAPA2Repository.Rapa2UpdateSelectedRecord(HDRId, thisResult.VinItems[0].seq);
                return (thisResult);
            }
            //Rule 4
            if (vinresultRowCount - fourWheelDrivecount == 1) //If 4x4 match math leaves one row, delete the rows that don't match and return
            {
                foreach (VINItemDto vinItem in thisResult.VinItems.ToList())
                {
                    if (vinItem.FourWheelDriveInd == "4")
                    {
                        thisResult.VinItems.Remove(vinItem);
                    }
                }
                RAPA2Repository.Rapa2UpdateSelectedRecord(HDRId, thisResult.VinItems[0].seq);
                return (thisResult);
            }
            //Now, about those factors.  The alphabetic order of the symbols is the same as the numeric order of the factors.  So we can sort on the symbols
            //we have vs going to the 400 and find the Trexis assigned Factors from the rate rev.  WooHoo!!
            int BICount = 0;
            int PDCount = 0;
            int COLCount = 0;
            string BIsave = string.Empty;
            string PDsave = string.Empty;
            string COLSave = string.Empty;
            //Rule 5
            thisResult.VinItems.Sort((p, q) => p.BiSymbol.CompareTo(q.BiSymbol)); //sort by BI symbol
            BIsave = thisResult.VinItems[0].BiSymbol; //Get the lowest BI symbol
            foreach (VINItemDto vinItem in thisResult.VinItems) //Count the instances of the lowest BI symbol
            {
                if (vinItem.BiSymbol == BIsave) { BICount++; }
            }
            if (BICount == 1) //If there is but one, return it
            {
                foreach (VINItemDto vinItem in thisResult.VinItems.ToList())
                {
                    if (vinItem.BiSymbol != BIsave)
                    {
                        thisResult.VinItems.Remove((VINItemDto)vinItem);
                    }
                }
                RAPA2Repository.Rapa2UpdateSelectedRecord(HDRId, thisResult.VinItems[0].seq);
                return (thisResult);
            }
            //Rule 6
            thisResult.VinItems.Sort((p, q) => p.PDSymbol.CompareTo(q.PDSymbol));
            PDsave = thisResult.VinItems[0].PDSymbol;
            foreach (VINItemDto vinItem in thisResult.VinItems) //Count the instances of the lowest PD symbol
            {
                if (vinItem.PDSymbol == PDsave) { PDCount++; }
            }
            if (PDCount == 1)//Just one?  Return it
            {
                foreach (VINItemDto vinItem in thisResult.VinItems.ToList())
                {
                    if (vinItem.PDSymbol != PDsave)
                    {
                        thisResult.VinItems.Remove((VINItemDto)vinItem);
                    }
                }
                RAPA2Repository.Rapa2UpdateSelectedRecord(HDRId, thisResult.VinItems[0].seq);
                return (thisResult);
            }
            //Rule 7
            thisResult.VinItems.Sort((p, q) => p.CollSymbol.CompareTo(q.CollSymbol));
            COLSave = thisResult.VinItems[0].CollSymbol;
            foreach (VINItemDto vinItem in thisResult.VinItems) //Count the instances of the lowest Col symbol
            {
                if (vinItem.CollSymbol == COLSave) { COLCount++; }
            }
            if (COLCount == 1) //If one is the lowest, send it back
            {
                foreach (VINItemDto vinItem in thisResult.VinItems.ToList())
                {
                    if (vinItem.CollSymbol != COLSave)
                    {
                        thisResult.VinItems.Remove((VINItemDto)vinItem);
                    }
                }
                RAPA2Repository.Rapa2UpdateSelectedRecord(HDRId, thisResult.VinItems[0].seq);
                return (thisResult);
            }
            //and finally, of we still have more than one row, whack the ones that aren't seq == 1
            //Rule 9... NO wait!  Rule 8
            foreach (VINItemDto vinItem in thisResult.VinItems.ToList())
            {
                if (vinItem.seq != 1)
                {
                    thisResult.VinItems.Remove((VINItemDto)vinItem);
                }
            }
            RAPA2Repository.Rapa2UpdateSelectedRecord(HDRId, thisResult.VinItems[0].seq);
            return (thisResult);
        }

        public void SetSelectedRapa2Vin(int quoteid, string vin, int seq)
        {
            RAPA2Repository.SetSelectedRapa2Vin(quoteid, vin, seq);
        }
    }
}
