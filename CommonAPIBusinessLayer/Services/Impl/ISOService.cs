using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon.Dto;
using CommonAPICommon;
using CommonAPIDAL.DataAccess;
using CommonAPIDAL.Repository.Impl;
using CommonAPIDAL.Repository.Interface;
using CommonAPIDAL.VisionAppModels;
using LexisNexisService;
using System.Xml.Serialization;
using System.Configuration;
using CommonAPIBusinessLayer.Services.Clients;
using CommonAPIBusinessLayer.Configuration;
using Microsoft.Extensions.Options;


namespace CommonAPIBusinessLayer.Services.Impl
{
    public class ISOService : IISOService
    {
        private static readonly log2sql log = new log2sql(nameof (ISOService));
        public enum ISOType { CV, Prefill }
        public IISORepository IsoRepository { get; set; }

        private readonly ISOServiceClient _isoClient;
        private readonly ISOConfig _isoConfig;


        private int QuoteId { get; set; }
        private int CarrierId { get; set; }

        private string CarrierName
        {
            get
            {
                switch (CarrierId)
                {
                    case 11:
                        return ConfigurationManager.AppSettings["CompanyNameTrexis"];
                    case 12:
                        return ConfigurationManager.AppSettings["CompanyNameHomestate"];
                    case 14:
                        return ConfigurationManager.AppSettings["CompanyNameTrexisOne"];
                    default:
                        return string.Empty;
                }
            }
        }

        private DateTime EffectiveDate { get; set; }
        private string PolicyHolderName { get; set; }
        private IList<PolicyDataDto> PolicyDatas { get; set; }
        private IList<PolicyCoverageIntervalDto> CoverageIntervals { get; set; }
        private IList<PrefillDriverDto> PrefillDrivers { get; set; }
        private IList<PrefillVehicleDto> PrefillVehicles { get; set; }

        public ISOService(ISOServiceClient isoClient, IOptions<ISOConfig> isoConfig)
        {
            IsoRepository = new ISORepository();
            _isoClient = isoClient;
            _isoConfig = isoConfig.Value;
        }

        public ISOService(IISORepository isoRepository, ISOServiceClient isoClient, IOptions<ISOConfig> isoConfig)
        {
            IsoRepository = isoRepository;
            _isoClient = isoClient;
            _isoConfig = isoConfig.Value;
        }

        public CoverageVerifierResponseDto CallISOCV(CoverageVerifierDto covVer, int quoteId)
        {
            var cvr = new CoverageVerifierResponseDto();
            CarrierId = covVer.CarrierId;
            EffectiveDate = covVer.EffDate;
            PolicyHolderName = System.String.Format("{0} {1}", covVer.Applicant.InsFirstName, covVer.Applicant.InsLastName);
            QuoteId = quoteId;
            covVer.Applicant.QuoteID = QuoteId;
            cvr.LapseDays = 999;

            try
            {
                PopulateData(covVer.Applicant, cvr, null, ISOType.CV, CarrierId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return cvr;
        }

        public PrefillResponseDto CallISOPrefill(ApplicantDto applicant, int quoteId)
        {
            var prefillResponse = new PrefillResponseDto();
            QuoteId = quoteId;
            PolicyHolderName = System.String.Format("{0} {1}", applicant.InsFirstName, applicant.InsLastName);
            applicant.QuoteID = quoteId;

            try
            {
                PopulateData(applicant, null, prefillResponse, ISOType.Prefill, CarrierId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            return prefillResponse;
        }

        /// <summary>
        /// This determines which supplier data is recieved from 
        /// and populates objects for Prefill and Coverage Verifier
        /// </summary>
        /// <param name="applicant"></param>
        /// <param name="cvr"></param>
        /// <param name="pfr"></param>
        /// <param name="isoType"></param>
        private void PopulateData(ApplicantDto applicant, CoverageVerifierResponseDto cvr, PrefillResponseDto pfr, ISOType isoType, int CarrierId)
        {
            //populating data for lexisnexis request
            var programMasterDataAccess = new ProgramMasterDataAccess();
            var prefillDataAccess = new PrefillDataAccess();
            var _LNcoverageValidationWorker = new LNcoverageValidationWorker();
            var prefillCallMaxTries = Convert.ToInt16(ConfigurationManager.AppSettings["PrefillCallMaxTries"]);
            applicant.GarZip = updateZip(applicant.GarZip); // this removes the dash from zipcode to prevent duplicate orders of prefill
            var isoMasterId = IsoRepository.GetExistingISOMasterId(applicant);
            var programMaster = programMasterDataAccess.GetProgramMaster(QuoteId);
            var supplierID = programMaster.PrefillSupplierID;
            var polType = programMaster.PolType;
            var _isoType = isoType.ToString();
            var _isoTypeLong = "";
            var Prefill_CV_complete = false;

            if (_isoType == "Prefill" && supplierID == 2)
            {
                _isoType = "LN_Prefill";
                _isoTypeLong = "Lexis Nexis Prefill";
            }
            if (_isoType == "CV" && supplierID == 2)
            {
                _isoType = "LN_Coverage Validation";
                _isoTypeLong = "Lexis Nexis Coverage Validation";
            }
            // this will call Prefill up to 3 times in case of an error
            for (int i = 0; i < prefillCallMaxTries; i++)
            {
                try
                {
                    if (isoMasterId == 0)
                    {
                        var includePrefill = IsoRepository.GetISOproduct(QuoteId);
                        var productToOrder = includePrefill ? "QFLN0000" : "EUANCVA0"; // old Verisk CV productCd CVAN0I00
                        var startTime = DateTime.Now;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                        string[] xmlStrings = null;

                        //2 == LexisNexis
                        //1 == Verisk
                        if (polType != "FR" && supplierID == 2)
                        {
                            var LNdataAccess = new LexisNexisDataAccess(applicant.InsFirstName, applicant.InsLastName, applicant.InsMidName, applicant.GarAddress,
                                        applicant.GarCity, applicant.GarState, applicant.GarZip, programMaster
                                        );

                            var iso = LNdataAccess.GetLexisNexisResponseDataAsync(QuoteId, _isoType, applicant);
                            xmlStrings = iso.Result;
                            productToOrder = supplierID == 2 ? _isoType : productToOrder;
                        }
                        else
                        {
                            //ISOBasic will be a dll
                            //TODO Find where the DLL is 
                            //var iso = new ISOBasic.ISOBasic(applicant.InsFirstName, applicant.InsLastName, applicant.InsMidName, applicant.GarAddress,
                            //                               applicant.GarCity, applicant.GarState, applicant.GarZip, applicant.InsPhone1,
                            //                               UserId, Password, ISOOrgId, ISOListenerLocation, productToOrder);

                            //xmlStrings = iso.GetISOResponse();
                        }
                        var ts = new TimeSpan(DateTime.Now.Ticks - startTime.Ticks);//Capturing Time Span to get a response from ISO 

                        // Lexis Nexis request/ response xml to database
                        if (xmlStrings != null && xmlStrings.Length > 0 && supplierID == 2)
                        {
                            if (xmlStrings[0] == "send email") // not a good call
                            {
                                var emailService = new EmailService();
                                emailService.SendEmail(_isoType, QuoteId, xmlStrings[1], xmlStrings[2]);
                            }
                            if (xmlStrings.Length > 3) // good call
                            {
                                var mappedToIsoLexisNexisObject = DeserializeISO(xmlStrings[3]);
                                var unmappedLexisNexisObject = DeserializeLN(xmlStrings[4]);
                                var LNcurrentCarrierInfo = unmappedLexisNexisObject.ProductResults.AutoDataPrefillResults[0].DriverDiscovery.CurrentCarrier.CurrentCarrierReport.Report;
                                var LNdiscoveredVehicles = unmappedLexisNexisObject.ProductResults.AutoDataPrefillResults[0].VehicleIndentification.DiscoveredVehicles;
                                var LNsearchDataSet = unmappedLexisNexisObject.ProductResults.AutoDataPrefillResults[0].SearchDataSet;
                                isoMasterId = Convert.ToInt32(xmlStrings[5]);
                                PersistToDatabase(isoMasterId, applicant, supplierID, mappedToIsoLexisNexisObject, LNcurrentCarrierInfo, LNdiscoveredVehicles);
                                Prefill_CV_complete = true;
                            }
                        }
                        // Iso Verisk request/ response xml to database
                        if (xmlStrings != null && xmlStrings.Length > 1 && supplierID == 1)
                        {
                            if (!string.IsNullOrWhiteSpace(xmlStrings[1]))
                            {
                                var isoObject = DeserializeISO(xmlStrings[1]);
                                var requestId = isoObject.PassportSvcRs.RequestId;
                                var status = isoObject.PassportSvcRs.Status.StatusDesc;

                                isoMasterId = IsoRepository.StoreRawAndMaster(QuoteId, xmlStrings[1], ts.Ticks, status, xmlStrings[0], requestId, productToOrder, applicant, null);
                                PersistToDatabase(isoMasterId, applicant, supplierID, isoObject, null, null);
                                Prefill_CV_complete = true;
                            }
                            else
                            {
                                IsoRepository.StoreRawOnly(QuoteId, ts.TotalMilliseconds, "Failure", xmlStrings[0], string.Empty, productToOrder);
                            }
                        }
                    }
                    else
                    {
                        if (supplierID == 2)
                        {
                            prefillDataAccess.SaveLNPrefillOrderHistory(QuoteId, isoMasterId);      // this saves all calls to Lexis Nexis and reuses of data from previous calls
                            PolicyDatas = ToPolicyDataDtos(IsoRepository.GetPolicyDatas(isoMasterId));
                            CoverageIntervals = _LNcoverageValidationWorker.CalculatePolicyCoverageIntervals(CarrierName, QuoteId, isoMasterId, supplierID, applicant, PolicyDatas.ToList());
                        }
                        if (supplierID == 1)
                        {
                            PolicyDatas = ToPolicyDataDtos(IsoRepository.GetPolicyDatas(isoMasterId));
                            CoverageIntervals = ToPolicyCoverageIntervalDtos(IsoRepository.GetCoverageIntervals(isoMasterId));
                        }
                        PrefillDrivers = ToPrefillDriverDtos(isoMasterId, IsoRepository.GetPrefillDrivers(isoMasterId));
                        PrefillVehicles = ToPrefillVehicleDtos(isoMasterId, IsoRepository.GetPrefillVehicles(isoMasterId));
                        Prefill_CV_complete = true;
                    }
                    if (isoMasterId != 0)  // it should not be by this point.
                    {
                        switch (isoType)
                        {
                            case ISOType.CV:
                                PopulateCVProperties(cvr, supplierID);
                                break;
                            case ISOType.Prefill:
                                PopulatePrefillProperties(pfr);
                                break;
                        }
                    }
                    if (Prefill_CV_complete == true)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    prefillDataAccess = new PrefillDataAccess();
                    var emailService = new EmailService();
                    var innerEx = ex.Message;
                    innerEx += ex.InnerException != null ? " InnerExeption: " + ex.InnerException.ToString() : string.Empty;

                    if (supplierID == 2)
                    {
                        prefillDataAccess.LogPrefillErrors(QuoteId, null, _isoType, _isoTypeLong + " Communication Failure", null, innerEx);
                    }
                    switch (isoType)
                    {
                        case ISOType.CV:
                            log.Error("Coverage Validation Error. QuoteID: " + QuoteId, ex);
                            emailService.SendEmail(_isoType, QuoteId, innerEx, null);
                            cvr.LastTermExpirationDate = DateTime.MinValue;
                            cvr.LapseDays = 999;
                            break;

                        case ISOType.Prefill:
                            log.Error("Prefill Error. QuoteID: " + QuoteId, ex);
                            emailService.SendEmail(_isoType, QuoteId, innerEx, null);
                            break;
                    }
                }
            }
        }

        //private async Task PopulateDataAsync(ApplicantDto applicant, CoverageVerifierResponseDto cvr, PrefillResponseDto pfr, ISOType isoType, int carrierId)
        //{
        //    try
        //    {
        //        var productToOrder = "QFLN0000"; // Default product code
        //        var requestXml = GenerateISORequestXml(applicant, productToOrder);

        //        // 🔹 Use ISOServiceClient for SOAP Calls Instead of ISOBasic.dll
        //        var responseXml = await _isoClient.CallISOAsync(requestXml);
        //        var isoResponse = DeserializeISO(responseXml);

        //        if (isoResponse != null)
        //        {
        //            ProcessISOResponse(isoResponse, applicant);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error during ISO SOAP request.");
        //        throw;
        //    }
        //}

        private string GenerateISORequestXml(ApplicantDto applicant, string productCode)
        {
            return $@"<SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/'>
                <SOAP-ENV:Header/>
                <SOAP-ENV:Body>
                    <ISORequest>
                        <UserId>{_isoConfig.ISOUN}</UserId>
                        <Password>{_isoConfig.ISOPWD}</Password>
                        <OrgId>{_isoConfig.ISOOrgID}</OrgId>
                        <ProductCode>{productCode}</ProductCode>
                        <FirstName>{applicant.InsFirstName}</FirstName>
                        <LastName>{applicant.InsLastName}</LastName>
                        <Address>{applicant.GarAddress}</Address>
                        <City>{applicant.GarCity}</City>
                        <State>{applicant.GarState}</State>
                        <Zip>{applicant.GarZip}</Zip>
                    </ISORequest>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>";
        }

        private ISO DeserializeISO(string xml)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ISO));
                using (StringReader reader = new StringReader(xml))
                {
                    return (ISO)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deserialize ISO response.");
                return null;
            }
        }

        /// <summary>
        /// this removes the dash from zipcode to prevent duplicate orders of prefill
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        public string updateZip(string zip)
        {
            zip = zip != null ? zip.Replace("-", "") : zip;
            return zip;
        }

        private ISO DeserializeISO(string xml)
        {
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer mySerializer = new XmlSerializer(typeof(ISO));
            // Call the Deserialize method and cast to the object type.
            using (var reader = new StringReader(xml))
            {
                return (ISO)mySerializer.Deserialize(reader);
            }
        }

        private ClientResponse DeserializeLN(string xml)
        {
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer mySerializer = new XmlSerializer(typeof(ClientResponse));
            // Call the Deserialize method and cast to the object type.
            using (var reader = new StringReader(xml))
            {
                return (ClientResponse)mySerializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// This puts Lexis Nexis or Verisk data in 
        /// reports for Prefill or Coverage Verifier process
        /// 
        /// </summary>
        /// <param name="isoMasterId"></param>
        /// <param name="isoDto"></param>
        private void PersistToDatabase(int isoMasterId, ApplicantDto applicant, int? supplierID, ISO isoDto, InsCurrentCarrierReportSection currentCarrier, ADPResultVehicleType[] discoveredVehicles)
        {
            //SupplierID
            //2 == LexisNexis
            //1 == Verisk
            if (supplierID == 2)
            {
                var prefillReport = isoDto.PassportSvcRs.Reports.SingleOrDefault(r => r.ProductCd == "LN_ADPF_PREFILL");
                var udiReport = isoDto.PassportSvcRs.Reports.SingleOrDefault(r => r.ProductCd == "LN_ADPF_UDI");
                var cvReport = isoDto.PassportSvcRs.Reports.SingleOrDefault(r => r.ProductCd == "LN_ADPF_CV");
                var polkReport = isoDto.PassportSvcRs.Reports.SingleOrDefault(r => r.ProductCd == "LN_ADPF_POLK");
                ProcessPrefill(prefillReport, udiReport, cvReport, polkReport, isoMasterId, supplierID, currentCarrier ?? null, discoveredVehicles ?? null);
                ProcessCV(applicant, isoMasterId, supplierID, cvReport);
            }
            else
            {
                var prefillReport = isoDto.PassportSvcRs.Reports.SingleOrDefault(r => r.ProductCd == "QFLN0000");
                var udiReport = isoDto.PassportSvcRs.Reports.SingleOrDefault(r => r.ProductCd == "ACXNX100");
                var cvReport = isoDto.PassportSvcRs.Reports.SingleOrDefault(r => r.ProductCd == "EUANCVA0");  // old Verisk CV productCd CVAN0I00
                var polkReport = isoDto.PassportSvcRs.Reports.SingleOrDefault(r => r.ProductCd == "POLNNP01");
                ProcessPrefill(prefillReport, udiReport, cvReport, polkReport, isoMasterId, supplierID, null, null);
                ProcessCV(applicant, isoMasterId, supplierID, cvReport);
            }
        }
        /// <summary>
        /// This processes report data from
        /// Lexis Nexis and Verisk for Prefill
        /// </summary>
        /// <param name="prefillReport"></param>
        /// <param name="udiReport"></param>
        /// <param name="cvReport"></param>
        /// <param name="polkReport"></param>
        /// <param name="masterId"></param>
        /// <param name="supplierID"></param>
        private void ProcessPrefill(ISOPassportSvcRsReport prefillReport, ISOPassportSvcRsReport udiReport, ISOPassportSvcRsReport cvReport, ISOPassportSvcRsReport polkReport, int masterId, int? supplierID, LexisNexisService.InsCurrentCarrierReportSection currentCarrier, LexisNexisService.ADPResultVehicleType[] discoveredVehicles)
        {
            var prefillworker = new PrefillWorker();
            var driver = new PrefillDriverDto();
            var drivers = new List<PrefillDriverDto>();
            var vehicle = new PrefillVehicleDto();
            var vehicles = new List<PrefillVehicleDto>();
            var lienVehicles = new List<PrefillVehicleDto>();
            var inceptionDate = new DateTime();
            var inceptionDates = new List<DateTime>();
            var _raterID = prefillworker.GetRaterID(QuoteId);
            var addressChanged = prefillworker.GetAddressChange(QuoteId, masterId);
            var _date = "";

            #region lienholders
            if (currentCarrier != null && discoveredVehicles != null)
            {
                // lienholders should only come from most recent policy
                if (currentCarrier.PolicyInformation != null && discoveredVehicles.Length > 0)
                {
                    foreach (var _policy in currentCarrier.PolicyInformation)
                    {
                        _date = _policy.InceptionDate.Year.ToString() + "/" + _policy.InceptionDate.Month.ToString("00") + "/" + _policy.InceptionDate.Day.ToString("00");
                        inceptionDate = Convert.ToDateTime(_date);
                        inceptionDates.Add(inceptionDate);
                    }
                    var _inceptionDates = inceptionDates.ToArray();
                    Array.Sort(_inceptionDates);
                    var mostRecentPolicy = _inceptionDates.LastOrDefault();

                    foreach (var policy in currentCarrier.PolicyInformation)
                    {
                        // this is to ensure lienholders only come from most recent policy
                        if (mostRecentPolicy != null && policy.InceptionDate != null)
                        {
                            _date = policy.InceptionDate.Year.ToString() + "/" + policy.InceptionDate.Month.ToString("00") + "/" + policy.InceptionDate.Day.ToString("00");
                            inceptionDate = Convert.ToDateTime(_date);
                            if (inceptionDate == mostRecentPolicy)
                            {
                                if (policy.CoveredVehiclesList != null)
                                {
                                    foreach (var VehInfo in policy.CoveredVehiclesList)
                                    {
                                        // This is to ensure there's a four digit model year
                                        var veh = discoveredVehicles.Where(w => w.VIN == VehInfo.VINNumber && w.DecodedVin.Type != "Motorcycle").SingleOrDefault();
                                        if (veh != null)
                                        {
                                            VehInfo.VehicleYear = veh.Year.ToString();
                                        }
                                        if (VehInfo.LienInfoList != null)
                                        {
                                            if (VehInfo.LienInfoList.Length == 1)
                                            {
                                                foreach (var item in VehInfo.LienInfoList)
                                                {
                                                    // This is to ensure there's no motorcycles added to lienvehicles
                                                    var discVehs = discoveredVehicles.Where(w => w.VIN == VehInfo.VINNumber && w.DecodedVin.Type != "Motorcycle").ToList();
                                                    if (discVehs.Count > 0)
                                                    {
                                                        var lienVehicle = new PrefillVehicleDto();
                                                        lienVehicle.LienHolderName = item.LienHolderName;
                                                        lienVehicle.LienHolderStreetAddress = item.LienHolderStreetAddress;
                                                        string[] LhAddress = item.LienHolderCityStateZip.Split(',');
                                                        lienVehicle.LienHolderCity = LhAddress[0].SafeTrim();
                                                        lienVehicle.LienHolderState = LhAddress[1].Substring(1, 2).SafeTrim();
                                                        lienVehicle.LienHolderZipCode = LhAddress[1].Substring(4, 5).SafeTrim();
                                                        lienVehicle.LienHolderSequenceNumber = item.LienHolderSequenceNumber;
                                                        lienVehicle.VIN = VehInfo.VINNumber;
                                                        lienVehicles.Add(lienVehicle);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region PrefillReport
            //// This assigns lienholders to vehicles
            if (prefillReport != null)
            {
                if (prefillReport.ReportData.ConditionalOrdering != null &&
                    prefillReport.ReportData.ConditionalOrdering.ResponseSummary != null &&
                    prefillReport.ReportData.ConditionalOrdering.ResponseSummary.VINS != null &&
                    prefillReport.ReportData.ConditionalOrdering.ResponseSummary.VINS.Any())
                {
                    foreach (var veh in prefillReport.ReportData.ConditionalOrdering.ResponseSummary.VINS)
                    {
                        if (lienVehicles != null && lienVehicles.Any())
                        {
                            foreach (var lienVehicle in lienVehicles)
                            {
                                if (veh.VIN == lienVehicle.VIN)
                                {
                                    vehicle.LienHolderSequenceNumber = lienVehicle.LienHolderSequenceNumber;
                                    vehicle.LienHolderName = lienVehicle.LienHolderName;
                                    vehicle.LienHolderStreetAddress = lienVehicle.LienHolderStreetAddress;
                                    vehicle.LienHolderCity = lienVehicle.LienHolderCity;
                                    vehicle.LienHolderState = lienVehicle.LienHolderState;
                                    vehicle.LienHolderZipCode = lienVehicle.LienHolderZipCode;

                                    AddVehicle(masterId, _raterID, addressChanged, vehicles, veh.VIN.SafeTrimU(), veh.Year.SafeTrim(), veh.Make.SafeTrimU(), veh.Model.SafeTrimU(), vehicle.LienHolderSequenceNumber.SafeTrimU(), vehicle.LienHolderName.SafeTrimU(), vehicle.LienHolderStreetAddress.SafeTrimU(), vehicle.LienHolderCity.SafeTrimU(), vehicle.LienHolderState.SafeTrim(), vehicle.LienHolderZipCode.SafeTrim());
                                }
                                else
                                {
                                    AddVehicle(masterId, _raterID, addressChanged, vehicles, veh.VIN.SafeTrimU(), veh.Year.SafeTrim(), veh.Make.SafeTrimU(), veh.Model.SafeTrimU(), null, null, null, null, null, null);
                                }
                            }
                        }
                        else
                        {
                            AddVehicle(masterId, _raterID, addressChanged, vehicles, veh.VIN.SafeTrimU(), veh.Year.SafeTrim(), veh.Make.SafeTrimU(), veh.Model.SafeTrimU(), null, null, null, null, null, null);
                        }
                        if (prefillReport.ReportData.ConditionalOrdering != null &&
                            prefillReport.ReportData.ConditionalOrdering.ResponseSummary != null &&
                            prefillReport.ReportData.ConditionalOrdering.ResponseSummary.DRIVER != null &&
                            prefillReport.ReportData.ConditionalOrdering.ResponseSummary.DRIVER.Any())
                        {
                            foreach (var drv in prefillReport.ReportData.ConditionalOrdering.ResponseSummary.DRIVER)
                            {
                                AddDriver(masterId, _raterID,
                                            addressChanged,
                                            drivers,
                                            drv.FirstName.SafeTrimU(),
                                            drv.MiddleName.SafeTrimU(),
                                            drv.LastName.SafeTrimU(),
                                            drv.DLNumber.SafeTrimU(),
                                            drv.DateOfBirth.yyyyMMddToDateTime() ?? DateTime.MinValue,
                                            drv.SSN.IsReal(),
                                            string.Empty);
                            }
                        }
                    }
                }
            }
            #endregion
            #region UdiReport
            if (udiReport != null)
            {
                if (udiReport.ReportData != null &&
                    udiReport.ReportData.ACXIOM != null &&
                    udiReport.ReportData.ACXIOM.RETRESPONSE != null &&
                    udiReport.ReportData.ACXIOM.RETRESPONSE.RETADDRLAYOUT != null &&
                    udiReport.ReportData.ACXIOM.RETRESPONSE.RETADDRLAYOUT.RETINDIVLAYOUT != null &&
                    udiReport.ReportData.ACXIOM.RETRESPONSE.RETADDRLAYOUT.RETINDIVLAYOUT.Any())
                {
                    var retAddress = udiReport.ReportData.ACXIOM.RETRESPONSE.RETADDRLAYOUT;
                    var address = retAddress?.RA_ADRESS?.ToString();
                    var city = retAddress?.RA_CITY?.ToString();
                    var state = retAddress?.RA_STATE?.ToString();
                    var zipCode = retAddress?.RA_ZIPCODE?.ToString();

                    foreach (var drv in udiReport.ReportData.ACXIOM.RETRESPONSE.RETADDRLAYOUT.RETINDIVLAYOUT)
                    {
                        AddDriver(masterId,
                                    _raterID,
                                    addressChanged,
                                    drivers,
                                    drv.RI_FIRSTNAME.SafeTrimU(),
                                    drv.RI_MIDDLEINITIAL.SafeTrimU(),
                                    drv.RI_LASTNAME.SafeTrimU(),
                                    drv.RI_DLNUMBER.SafeTrimU(),
                                    drv.RI_DOB.yyyyMMddToDateTime() ?? DateTime.MinValue,
                                    drv.RI_SSN.IsReal(),
                                    drv.RI_GENDER.SafeTrimU(),
                                    driver.Address = address,
                                    driver.City = city,
                                    driver.State = state,
                                    driver.ZipCode = zipCode);
                    }
                }
            }
            #endregion
            #region cvReport
            if (cvReport != null)
            {
                if (cvReport.ReportData.CoverageVerifier != null &&
                    cvReport.ReportData.CoverageVerifier.Policy != null &&
                    cvReport.ReportData.CoverageVerifier.Policy.Any())
                {
                    foreach (var policy in cvReport.ReportData.CoverageVerifier.Policy)
                    {
                        if (policy.DetailSection.SubjectInfo != null &&
                            policy.DetailSection.SubjectInfo.Any())
                        {
                            foreach (var drv in policy.DetailSection.SubjectInfo)
                            {
                                AddDriver(masterId,
                                        _raterID,
                                        addressChanged,
                                        drivers,
                                        drv.SubjectName.FirstName.SafeTrimU(),
                                        drv.SubjectName.MiddleName == null ? string.Empty : drv.SubjectName.MiddleName.SafeTrimU(),
                                        drv.SubjectName.LastName.SafeTrimU(),
                                        drv.DLNumber.SafeTrimU(),
                                        drv.DateOfBirth.yyyyMMddToDateTime() ?? DateTime.MinValue,
                                        drv.SSN.IsReal(),
                                        drv.Gender.SafeTrimU());
                            }
                        }
                        if (policy.DetailSection.VehicleInfo != null &&
                            policy.DetailSection.VehicleInfo.Any())
                        {
                            foreach (var veh in policy.DetailSection.VehicleInfo)
                            {
                                AddVehicle(masterId, _raterID, addressChanged, vehicles, veh.VIN.SafeTrimU(), veh.Year.SafeTrim(), veh.Make.SafeTrimU(), veh.Model.SafeTrimU(), null, null, null, null, null, null);
                            }
                        }
                    }
                }
            }
            #endregion
            #region polkReport
            if (polkReport != null)
            {
                if (polkReport != null &&
                    polkReport.ReportData != null &&
                    polkReport.ReportData.PolkResponse != null &&
                    polkReport.ReportData.PolkResponse.Matches != null &&
                    polkReport.ReportData.PolkResponse.Matches.Any())
                {
                    foreach (var veh in polkReport.ReportData.PolkResponse.Matches)
                    {
                        AddVehicle(masterId, _raterID, addressChanged, vehicles, veh.VIN.SafeTrimU(), veh.YearModel.SafeTrim(), veh.Make.SafeTrimU(), string.Empty, null, null, null, null, null, null);
                    }
                }
            }
            #endregion
            IsoRepository.SavePrefill(masterId, QuoteId, drivers.ToArray(), vehicles.ToArray(), supplierID);
            PrefillDrivers = drivers;
            PrefillVehicles = vehicles;
        }
        /// <summary>
        /// This processes report from Verisk 
        /// for Coverage Verifier
        /// </summary>
        /// <param name="cvReport"></param>
        /// <param name="masterId"></param>
        /// <param name="supplierID"></param>
        private void ProcessCV(ApplicantDto applicant, int masterId, int? supplierID, ISOPassportSvcRsReport cvReport)
        {
            if (cvReport != null)
            {
                var biCovInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo();
                var policyCoverageIntervals = new List<PolicyCoverageIntervalDto>();
                var policyDatas = new List<PolicyDataDto>();
                var IntervalPolicyDatas = new List<PolicyDataDto>();

                if (cvReport.ReportData.CoverageVerifier != null &&
                    cvReport.ReportData.CoverageVerifier.Policy != null &&
                    cvReport.ReportData.CoverageVerifier.Policy.Any())
                {
                    var policies = cvReport.ReportData.CoverageVerifier.Policy;
                    foreach (var policy in policies)
                    {
                        if (policy.DetailSection != null && policy.HistorySection != null &&
                            policy.DetailSection.GarageLocationInfo != null && policy.DetailSection.SubjectInfo != null &&
                            policy.DetailSection.PolicyDetailInfo != null && policy.DetailSection.CarrierInfo != null &&
                            policy.HistorySection.SubjectInfo != null && policy.HistorySection.PolicyHistoryInfo != null)
                        {
                            var policyData = new PolicyDataDto();
                            if (policy.DetailSection.CoverageInfo != null)
                            {
                                if (policy.DetailSection.CoverageInfo.AddressCoverageInfo != null)
                                {
                                    if (supplierID == 2)
                                    {
                                        biCovInfo = policy.DetailSection.CoverageInfo.AddressCoverageInfo.OrderByDescending(w => w.IndividualLimit).FirstOrDefault();
                                    }
                                    else
                                    {
                                        biCovInfo = policy.DetailSection.CoverageInfo.AddressCoverageInfo.SingleOrDefault(ac => ac.CoverageCode == "BodilyInjury");
                                    }
                                    if (biCovInfo != null)
                                    {
                                        if (biCovInfo.CoverageCode?.ToUpper() == "BODILYINJURY")
                                        {
                                            policyData.BIIndividualLimit = biCovInfo.IndividualLimit.ToInt() ?? 0;
                                            policyData.BIOccuranceLimit = biCovInfo.OccuranceLimit.ToInt() ?? 0;
                                            policyData.CSILimit = biCovInfo.CSILimit.ToInt() ?? 0;
                                        }
                                    }
                                }
                            }
                            if (policy.DetailSection.CarrierInfo != null)
                            {
                                policyData.CarrierName = policy.DetailSection.CarrierInfo.CarrierName;
                            }
                            if (policy.DetailSection.PolicyDetailInfo != null)
                            {
                                policyData.PolicyNumber = policy.DetailSection.PolicyDetailInfo.PolicyNumber;
                                policyData.PolicyStatus = policy.DetailSection.PolicyDetailInfo.PolicyStatus;
                                policyData.InceptionDate = policy.DetailSection.PolicyDetailInfo.InceptionDate.yyyyMMddToDateTime() ?? DateTime.MinValue;
                                policyData.CoverageFromDate = policy.DetailSection.PolicyDetailInfo.TermEffectiveDate.yyyyMMddToDateTime() ?? DateTime.MinValue;
                                policyData.CoverageToDate = policy.DetailSection.PolicyDetailInfo.TermExpirationDate.yyyyMMddToDateTime() ?? DateTime.MinValue;
                                policyData.LastReportedTermEffectiveDate = policy.DetailSection.PolicyDetailInfo.TermEffectiveDate.yyyyMMddToDateTime() ?? DateTime.MinValue;
                                policyData.LastReportedTermExpirationDate = policy.DetailSection.PolicyDetailInfo.TermExpirationDate.yyyyMMddToDateTime() ?? DateTime.MinValue;
                                policyData.LastCancelDate = policy.DetailSection.PolicyDetailInfo.CancellationDate.yyyyMMddToDateTime();
                                policyData.CancelReason = policy.DetailSection.PolicyDetailInfo.CancellationReasonDesc;
                                policyData.NAIC = policy.DetailSection.CarrierInfo.NAICCode;
                            }
                            if (policy.HistorySection.TransactionInfo != null)
                            {
                                policyData.NumberOfCancellations = policy.HistorySection.TransactionInfo.Count(ti => ti.TransactionDesc.ToUpper().Contains("CANCEL"));
                                policyData.NumberOfRenewals = policy.HistorySection.TransactionInfo.Count(ti => ti.TransactionDesc.ToUpper().Contains("RENEWAL"));
                            }
                            var polSearchInfo = cvReport.ReportData.CoverageVerifier.SummarySection.PolicySearchInfo;
                            var sumSection = polSearchInfo != null ? cvReport.ReportData.CoverageVerifier.SummarySection.PolicySearchInfo.FirstOrDefault(ps => ps.PolicyNumber == policyData.PolicyNumber) : null;
                            if (sumSection != null)
                            {
                                policyData.ReportAsOfDate = sumSection.ReportAsOfDate.yyyyMMddToDateTime() ?? DateTime.MinValue;
                                if (supplierID == 1)
                                {
                                    if ((sumSection.LastRptTermEffDate.yyyyMMddToDateTime() ?? DateTime.MinValue) != policyData.LastReportedTermEffectiveDate)
                                        throw new Exception("Check this out: Summary LastRptTermEffDate does not match Detail section");

                                    if ((sumSection.LastRptTermExpDate.yyyyMMddToDateTime() ?? DateTime.MinValue) != policyData.LastReportedTermExpirationDate)
                                        throw new Exception("Check this out: Summary LastRptTermExpDate does not match Detail section");

                                    if ((sumSection.NumOfCancellations.ToInt() ?? 0) != policyData.NumberOfCancellations)
                                        throw new Exception("Check this out: Summary NumOfCancellations does not match History section Transaction Info");

                                    if ((sumSection.NumOfRenewals.ToInt() ?? 0) != policyData.NumberOfRenewals)
                                        throw new Exception("Check this out: Summary NumOfRenewals does not match History section Transaction Info");
                                }
                                policyData.PolicyHolderName = string.Format("{0} {1}", sumSection.PolicyHolder1.FirstName, sumSection.PolicyHolder1.LastName);
                            }
                            var polHistInfo = policy.HistorySection.PolicyHistoryInfo;
                            var policyHistoryInfo = polHistInfo != null ? policy.HistorySection.PolicyHistoryInfo.FirstOrDefault() : null;
                            if (policyHistoryInfo != null)
                            {
                                policyData.IsStandardPolicy = policyHistoryInfo.RiskClassCode != "N";
                                policyData.PolicyType = policyHistoryInfo.PolicyTypeDesc;
                            }
                            if (policy.DetailSection.GarageLocationInfo != null)
                            {
                                var garageAddr = policy.DetailSection.GarageLocationInfo.FirstOrDefault();
                                if (garageAddr != null)
                                {
                                    policyData.PolicyState = garageAddr.GarageAddress.State;
                                }
                            }
                            var policySubjects = new List<PolicySubjectDto>();
                            if (policy.DetailSection.SubjectInfo != null)
                            {
                                foreach (var sInfo in policy.DetailSection.SubjectInfo)
                                {
                                    var subject = new PolicySubjectDto();
                                    subject.Name = string.Format("{0} {1}", sInfo.SubjectName.FirstName, sInfo.SubjectName.LastName);
                                    subject.RelationCode = !string.IsNullOrWhiteSpace(sInfo.RelationCode) ? sInfo.RelationCode : string.Empty;
                                    subject.RelationCodeDesc = !string.IsNullOrWhiteSpace(sInfo.RelationCodeDesc) ? sInfo.RelationCodeDesc : string.Empty;
                                    subject.SubjectId = !string.IsNullOrWhiteSpace(sInfo.SubjectNo) ? sInfo.SubjectNo : string.Empty;
                                    subject.FromDate = sInfo.FromDate.yyyyMMddToDateTime() ?? DateTime.MinValue;
                                    subject.ToDate = sInfo.ToDate.yyyyMMddToDateTime() ?? DateTime.MinValue;

                                    var policySubjectHistories = new List<PolicySubjectHistoryDto>();
                                    if (policy.HistorySection.SubjectInfo != null)
                                    {
                                        foreach (var history in policy.HistorySection.SubjectInfo.Where(si => si.SubjectName.FirstName == sInfo.SubjectName.FirstName && si.SubjectName.LastName == sInfo.SubjectName.LastName))
                                        {
                                            var subjectHistory = new PolicySubjectHistoryDto();
                                            subjectHistory.FromDate = history.FromDate.yyyyMMddToDateTime() ?? DateTime.MinValue;
                                            subjectHistory.ToDate = history.ToDate.yyyyMMddToDateTime() ?? DateTime.MinValue;
                                            if (subjectHistory.FromDate != DateTime.MinValue)
                                            {
                                                policySubjectHistories.Add(subjectHistory);
                                            }
                                        }
                                        subject.PolicySubjectHistories = policySubjectHistories;
                                    }
                                    if (subject.FromDate != DateTime.MinValue)
                                    {
                                        policySubjects.Add(subject);
                                    }
                                }
                            }
                            policyData.PolicySubjects = policySubjects;
                            IntervalPolicyDatas.Add(policyData);    // IntervalPolicyDatas is for CalculatePolicyCoverageIntervals logic
                            policyDatas.Add(policyData);
                        }
                    }
                }
                if (supplierID == 2)
                {
                    var _LNcoverageValidationWorker = new LNcoverageValidationWorker();
                    policyCoverageIntervals = _LNcoverageValidationWorker.CalculatePolicyCoverageIntervals(CarrierName, QuoteId, masterId, supplierID, applicant, IntervalPolicyDatas);
                }
                if (supplierID == 1)
                {
                    if (cvReport.ReportData.CoverageVerifier.SummarySection != null &&
                            cvReport.ReportData.CoverageVerifier.SummarySection.CoverageLapse2 != null &&
                            cvReport.ReportData.CoverageVerifier.SummarySection.CoverageLapse2.CoverageInterval != null &&
                            cvReport.ReportData.CoverageVerifier.SummarySection.CoverageLapse2.CoverageInterval.Any())
                    {
                        var lapses = cvReport.ReportData.CoverageVerifier.SummarySection.CoverageLapse2.CoverageInterval;

                        foreach (var lapse in lapses)
                        {
                            policyCoverageIntervals.Add(new PolicyCoverageIntervalDto()
                            {
                                Company = lapse.CoName.SafeTrim(),
                                AMBest = lapse.AMBest.SafeTrim(),
                                InceptionDate = DateTime.MinValue,
                                FromDate = lapse.FromDate.ToDateTime() ?? DateTime.MinValue,
                                ToDate = lapse.ToDate.ToDateTime() ?? DateTime.MinValue,
                                CoverageDays = lapse.CoverageDays.ToInt() ?? 0,
                                BreakFromPrior = lapse.BreakFromPrior.SafeTrim() ?? "",
                                LapsedDays = lapse.LapsedDays.ToInt() ?? 0,
                                LapseReason = null,
                                PolicyHolderName = null,
                                PolicyHolderRelationship = null,
                                SubjectUnitNumber = null,
                                PolicyNumber = null,
                                PolicyStatus = null
                            });
                        }
                    }
                    IsoRepository.SaveCV(masterId, supplierID, policyCoverageIntervals.ToArray(), policyDatas.ToArray());
                }
                PolicyDatas = policyDatas;
                CoverageIntervals = policyCoverageIntervals;
            }
        }
        private void AddDriver(int isoMasterID, int _raterID, bool _addressChanged, List<PrefillDriverDto> drivers, string firstName, string middleName, string lastName, string dlNumber, DateTime dob, string ssn, string gender)
        {
            var prefillDataAccess = new PrefillDataAccess();
            var prefillWorker = new PrefillWorker();
            var prefillMatch = new List<Staging_Driver>();
            var previousPrefillDrivers = prefillDataAccess.GetExistingPrefillDrivers(QuoteId);
            var existingFullMatch = drivers.FirstOrDefault(d => d.FirstName == firstName &&
                                                            d.LastName == lastName && (
                                                            d.DateOfBirth == dob || d.DLNumber == dlNumber || d.SSN == ssn));

            // PrefillMatch is to is to prevent duplicate prefill drivers
            if (previousPrefillDrivers.Count > 1 || (_raterID > 0 && previousPrefillDrivers.Count > 0))
            {
                foreach (var driver in previousPrefillDrivers)
                {
                    if (driver.DOB != DateTime.MinValue.ToString() && driver.DOB.Length < 19)
                    {
                        driver.DOB = driver.DOB.Insert(4, "/");
                        driver.DOB = driver.DOB.Insert(7, "/");
                    }
                }
                if (dob != DateTime.MinValue)
                {
                    prefillMatch = previousPrefillDrivers.Where(w => w.DrvFirstName == firstName &&
                       w.DrvLastName == lastName && Convert.ToDateTime(w.DOB) == dob).ToList();
                }
                else
                {
                    prefillMatch = previousPrefillDrivers.Where(w => w.DrvFirstName == firstName &&
                      w.DrvLastName == lastName).ToList();
                }
                if (prefillMatch.Count > 0)
                {
                    var _rejectDriver = prefillMatch[0];
                    var _rejectionType = prefillWorker.GetRejectionType(_addressChanged, _raterID, false);
                    prefillDataAccess.InsertDriverRejectType(QuoteId, isoMasterID, _rejectDriver, _rejectionType);
                }
            }
            if (existingFullMatch != null) // driver exists and matches on a key field
            {
                if (string.IsNullOrWhiteSpace(existingFullMatch.MiddleName) && !string.IsNullOrWhiteSpace(middleName))
                    existingFullMatch.MiddleName = middleName;
                if (string.IsNullOrWhiteSpace(existingFullMatch.DLNumber) && !string.IsNullOrWhiteSpace(dlNumber))
                    existingFullMatch.DLNumber = dlNumber;
                if (existingFullMatch.DateOfBirth == DateTime.MinValue && dob != DateTime.MinValue)
                    existingFullMatch.DateOfBirth = dob;
                if (string.IsNullOrWhiteSpace(existingFullMatch.SSN) && !string.IsNullOrWhiteSpace(ssn))
                    existingFullMatch.SSN = ssn;
                if (string.IsNullOrWhiteSpace(existingFullMatch.Gender) && !string.IsNullOrWhiteSpace(gender))
                    existingFullMatch.Gender = gender;
                return;
            }
            existingFullMatch = drivers.FirstOrDefault(d => d.FirstName == firstName &&
                                                            d.LastName == lastName &&
                                                            d.DateOfBirth == DateTime.MinValue &&
                                                            string.IsNullOrWhiteSpace(d.DLNumber) &&
                                                            string.IsNullOrWhiteSpace(d.SSN));

            if (existingFullMatch != null)  // driver exists and all compare fields empty or mins
            {
                if (string.IsNullOrWhiteSpace(existingFullMatch.MiddleName) && !string.IsNullOrWhiteSpace(middleName))
                    existingFullMatch.MiddleName = middleName;
                if (string.IsNullOrWhiteSpace(existingFullMatch.DLNumber) && !string.IsNullOrWhiteSpace(dlNumber))
                    existingFullMatch.DLNumber = dlNumber;
                if (existingFullMatch.DateOfBirth == DateTime.MinValue && dob != DateTime.MinValue)
                    existingFullMatch.DateOfBirth = dob;
                if (string.IsNullOrWhiteSpace(existingFullMatch.SSN) && !string.IsNullOrWhiteSpace(ssn))
                    existingFullMatch.SSN = ssn;
                if (string.IsNullOrWhiteSpace(existingFullMatch.Gender) && !string.IsNullOrWhiteSpace(gender))
                    existingFullMatch.Gender = gender;
                return;
            }
            //if we are still here and not found a reason to update and existing so we are going to add the new.
            if (prefillMatch.Count.Equals(0))
            {
                drivers.Add(new PrefillDriverDto()
                {
                    DateOfBirth = dob,
                    DLNumber = dlNumber,
                    FirstName = firstName,
                    Gender = gender,
                    LastName = lastName,
                    MiddleName = middleName,
                    SSN = ssn
                });
            }
        }
        private void AddDriver(int isoMasterID, int _raterID, bool _addressChanged, List<PrefillDriverDto> drivers, string firstName, string middleName, string lastName, string dlNumber, DateTime dob, string ssn, string gender, string address, string city, string state, string zipCode)
        {
            var prefillDataAccess = new PrefillDataAccess();
            var prefillWorker = new PrefillWorker();
            var prefillMatch = new List<Staging_Driver>();
            var previousPrefillDrivers = prefillDataAccess.GetExistingPrefillDrivers(QuoteId);
            var existingFullMatch = drivers.FirstOrDefault(d => d.FirstName == firstName &&
                                                            d.LastName == lastName && (
                                                            d.DateOfBirth == dob || d.DLNumber == dlNumber || d.SSN == ssn));

            // PrefillMatch is to is to prevent duplicate prefill drivers
            if (previousPrefillDrivers.Count > 1 || (_raterID > 0 && previousPrefillDrivers.Count > 0))
            {
                foreach (var driver in previousPrefillDrivers)
                {
                    if (driver.DOB != DateTime.MinValue.ToString() && driver.DOB.Length < 19)
                    {
                        driver.DOB = driver.DOB.Insert(4, "/");
                        driver.DOB = driver.DOB.Insert(7, "/");
                    }
                }
                if (dob != DateTime.MinValue)
                {
                    prefillMatch = previousPrefillDrivers.Where(w => w.DrvFirstName == firstName &&
                       w.DrvLastName == lastName && Convert.ToDateTime(w.DOB) == dob).ToList();
                }
                else
                {
                    prefillMatch = previousPrefillDrivers.Where(w => w.DrvFirstName == firstName &&
                      w.DrvLastName == lastName).ToList();
                }
                if (prefillMatch.Count > 0)
                {
                    var _rejectDriver = prefillMatch[0];
                    var _rejectionType = prefillWorker.GetRejectionType(_addressChanged, _raterID, false);
                    prefillDataAccess.InsertDriverRejectType(QuoteId, isoMasterID, _rejectDriver, _rejectionType);
                }
            }
            if (existingFullMatch != null) // driver exists and matches on a key field
            {
                if (string.IsNullOrWhiteSpace(existingFullMatch.MiddleName) && !string.IsNullOrWhiteSpace(middleName))
                    existingFullMatch.MiddleName = middleName;
                if (string.IsNullOrWhiteSpace(existingFullMatch.DLNumber) && !string.IsNullOrWhiteSpace(dlNumber))
                    existingFullMatch.DLNumber = dlNumber;
                if (existingFullMatch.DateOfBirth == DateTime.MinValue && dob != DateTime.MinValue)
                    existingFullMatch.DateOfBirth = dob;
                if (string.IsNullOrWhiteSpace(existingFullMatch.SSN) && !string.IsNullOrWhiteSpace(ssn))
                    existingFullMatch.SSN = ssn;
                if (string.IsNullOrWhiteSpace(existingFullMatch.Gender) && !string.IsNullOrWhiteSpace(gender))
                    existingFullMatch.Gender = gender;
                if (string.IsNullOrWhiteSpace(existingFullMatch.Address) && !string.IsNullOrWhiteSpace(address))
                    existingFullMatch.Address = address;
                if (string.IsNullOrWhiteSpace(existingFullMatch.City) && !string.IsNullOrWhiteSpace(city))
                    existingFullMatch.City = city;
                if (string.IsNullOrWhiteSpace(existingFullMatch.State) && !string.IsNullOrWhiteSpace(state))
                    existingFullMatch.State = state;
                if (string.IsNullOrWhiteSpace(existingFullMatch.ZipCode) && !string.IsNullOrWhiteSpace(zipCode))
                    existingFullMatch.ZipCode = zipCode;
                return;
            }
            existingFullMatch = drivers.FirstOrDefault(d => d.FirstName == firstName &&
                                                            d.LastName == lastName &&
                                                            d.DateOfBirth == DateTime.MinValue &&
                                                            string.IsNullOrWhiteSpace(d.DLNumber) &&
                                                            string.IsNullOrWhiteSpace(d.SSN));

            if (existingFullMatch != null)  // driver exists and all compare fields empty or mins
            {
                if (string.IsNullOrWhiteSpace(existingFullMatch.MiddleName) && !string.IsNullOrWhiteSpace(middleName))
                    existingFullMatch.MiddleName = middleName;
                if (string.IsNullOrWhiteSpace(existingFullMatch.DLNumber) && !string.IsNullOrWhiteSpace(dlNumber))
                    existingFullMatch.DLNumber = dlNumber;
                if (existingFullMatch.DateOfBirth == DateTime.MinValue && dob != DateTime.MinValue)
                    existingFullMatch.DateOfBirth = dob;
                if (string.IsNullOrWhiteSpace(existingFullMatch.SSN) && !string.IsNullOrWhiteSpace(ssn))
                    existingFullMatch.SSN = ssn;
                if (string.IsNullOrWhiteSpace(existingFullMatch.Gender) && !string.IsNullOrWhiteSpace(gender))
                    existingFullMatch.Gender = gender;
                if (string.IsNullOrWhiteSpace(existingFullMatch.Address) && !string.IsNullOrWhiteSpace(address))
                    existingFullMatch.Address = address;
                if (string.IsNullOrWhiteSpace(existingFullMatch.City) && !string.IsNullOrWhiteSpace(city))
                    existingFullMatch.City = city;
                if (string.IsNullOrWhiteSpace(existingFullMatch.State) && !string.IsNullOrWhiteSpace(state))
                    existingFullMatch.State = state;
                if (string.IsNullOrWhiteSpace(existingFullMatch.ZipCode) && !string.IsNullOrWhiteSpace(zipCode))
                    existingFullMatch.ZipCode = zipCode;
                return;
            }
            //if we are still here and not found a reason to update and existing so we are going to add the new.
            if (prefillMatch.Count.Equals(0))
            {
                drivers.Add(new PrefillDriverDto()
                {
                    DateOfBirth = dob,
                    DLNumber = dlNumber,
                    FirstName = firstName,
                    Gender = gender,
                    LastName = lastName,
                    MiddleName = middleName,
                    SSN = ssn,
                    Address = address,
                    City = city,
                    State = state,
                    ZipCode = zipCode
                });
            }
        }
        private void AddVehicle(int isoMasterID, int _raterID, bool _addressChanged, List<PrefillVehicleDto> vehicles, string vin, string modelYear, string make, string model, string lienHolderSequenceNumber, string lienHolderName, string lienHolderStreetAddress, string lienHolderCity, string lienHolderState, string lienHolderZipCode)
        {
            var prefillDataAccess = new PrefillDataAccess();
            var prefillWorker = new PrefillWorker();
            var prefillMatch = new List<ISOVehicles>();
            var raterPrefillMatch = new List<ISOVehicles>();
            var existingVehicle = vehicles.SingleOrDefault(v => v.VIN == vin);
            var previousPrefillVehicles = prefillDataAccess.GetPreviousPrefillVehicles(QuoteId);
            var vinMerge = false;

            //// prefillMatch is to is to prevent duplicate prefill drivers
            if (previousPrefillVehicles.Count > 0)
            {
                if (_raterID > 0)
                {
                    // if vehicle's from comparitive rater with no vin, merge vin from prefill
                    raterPrefillMatch = previousPrefillVehicles.Where(w => w.VIN == vin).ToList();
                    if (raterPrefillMatch.Count > 0)
                    {
                        prefillMatch = raterPrefillMatch;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(modelYear))
                        {
                            prefillMatch = previousPrefillVehicles.Where(w => w.ModelYear == Convert.ToInt32(modelYear) && w.Make == make && w.ShortModelName == model).ToList();
                            if (prefillMatch.Count > 0)
                            {
                                prefillMatch[0].VIN = vin;
                                vinMerge = true;
                            }
                        }
                    }
                }
                else
                {
                    prefillMatch = previousPrefillVehicles.Where(w => w.VIN == vin).ToList();
                }
            }
            if (prefillMatch.Count > 0)
            {
                var _rejectVehicle = prefillMatch[0];
                var _rejectType = prefillWorker.GetRejectionType(_addressChanged, _raterID, vinMerge);
                prefillDataAccess.InsertVehicleRejectType(QuoteId, isoMasterID, _rejectVehicle, _rejectType);
            }
            if (existingVehicle == null && prefillMatch.Count.Equals(0))
            {
                vehicles.Add(new PrefillVehicleDto()
                {
                    VIN = vin,
                    ModelYear = modelYear,
                    Make = make,
                    ShortModel = model,
                    LienHolderSequenceNumber = lienHolderSequenceNumber,
                    LienHolderName = lienHolderName,
                    LienHolderStreetAddress = lienHolderStreetAddress,
                    LienHolderCity = lienHolderCity,
                    LienHolderState = lienHolderState,
                    LienHolderZipCode = lienHolderZipCode
                });
            }
            else if (prefillMatch.Count.Equals(0))
            {
                //Update values on Existing vehicles if they were blank/null only
                if (string.IsNullOrWhiteSpace(existingVehicle.Make))
                {
                    existingVehicle.Make = make;
                }
                if (string.IsNullOrWhiteSpace(existingVehicle.ShortModel))
                {
                    existingVehicle.ShortModel = model;
                }
                if (string.IsNullOrWhiteSpace(existingVehicle.LienHolderSequenceNumber))
                {
                    existingVehicle.LienHolderSequenceNumber = lienHolderSequenceNumber;
                }
                if (string.IsNullOrWhiteSpace(existingVehicle.LienHolderName))
                {
                    existingVehicle.LienHolderName = lienHolderName;
                }
                if (string.IsNullOrWhiteSpace(existingVehicle.LienHolderStreetAddress))
                {
                    existingVehicle.LienHolderStreetAddress = lienHolderStreetAddress;
                }
                if (string.IsNullOrWhiteSpace(existingVehicle.LienHolderCity))
                {
                    existingVehicle.LienHolderCity = lienHolderCity;
                }
                if (string.IsNullOrWhiteSpace(existingVehicle.LienHolderState))
                {
                    existingVehicle.LienHolderState = lienHolderState;
                }
                if (string.IsNullOrWhiteSpace(existingVehicle.LienHolderZipCode))
                {
                    existingVehicle.LienHolderZipCode = lienHolderZipCode;
                }
            }
        }
        private IList<PolicyDataDto> ToPolicyDataDtos(IList<dynamic> polDatas)
        {
            var policyDatas = new List<PolicyDataDto>();
            foreach (var polData in polDatas)
            {
                var policyData = new PolicyDataDto();
                policyData.Id = polData.Id;
                policyData.ISOMasterId = polData.ISOMasterId;
                policyData.BIIndividualLimit = polData.BIIndividualLimit;
                policyData.BIOccuranceLimit = polData.BIOccuranceLimit;
                policyData.CancelReason = polData.CancelReason;
                policyData.CarrierName = polData.CarrierName;
                policyData.CoverageFromDate = polData.CoverageFromDate;
                policyData.CoverageToDate = polData.CoverageToDate;
                policyData.CSILimit = polData.CSILimit;
                policyData.InceptionDate = polData.InceptionDate;
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
                var policySubjects = new List<PolicySubjectDto>();
                foreach (var polSub in polData.PolicySubjects)
                {
                    var policySubject = new PolicySubjectDto();
                    policySubject.Id = polSub.Id;
                    policySubject.PolicyId = polSub.PolicyId;
                    policySubject.FromDate = polSub.FromDate;
                    policySubject.Name = polSub.Name;
                    policySubject.RelationCode = polSub.RelationCode;
                    policySubject.RelationCodeDesc = polSub.RelationCodeDesc;
                    policySubject.ToDate = polSub.ToDate;
                    var policySubjectHistories = new List<PolicySubjectHistoryDto>();
                    foreach (var polSubHist in polSub.PolicySubjectHistories)
                    {
                        var policySubjectHistory = new PolicySubjectHistoryDto();
                        policySubjectHistory.Id = polSubHist.Id;
                        policySubjectHistory.PolicySubjectId = polSubHist.PolicySubjectId;
                        policySubjectHistory.FromDate = polSubHist.FromDate;
                        policySubjectHistory.ToDate = polSubHist.ToDate;
                        policySubjectHistories.Add(policySubjectHistory);
                    }
                    policySubject.PolicySubjectHistories = policySubjectHistories;
                    policySubjects.Add(policySubject);
                }
                policyData.PolicySubjects = policySubjects;
                policyDatas.Add(policyData);
            }
            return policyDatas;
        }
        private IList<PolicyCoverageIntervalDto> ToPolicyCoverageIntervalDtos(IList<dynamic> intervals)
        {
            var policyIntervals = new List<PolicyCoverageIntervalDto>();
            foreach (var interval in intervals)
            {
                var policyInterval = new PolicyCoverageIntervalDto();
                policyInterval.AMBest = interval.AMBest;
                policyInterval.BreakFromPrior = interval.BreakFromPrior;
                policyInterval.Company = interval.Company;
                policyInterval.CoverageDays = interval.CoverageDays;
                policyInterval.InceptionDate = interval.InceptionDate != null ? interval.InceptionDate : DateTime.MinValue;
                policyInterval.FromDate = interval.FromDate;
                policyInterval.Id = interval.Id;
                policyInterval.ISOMasterId = interval.ISOMasterId;
                policyInterval.LapsedDays = interval.LapsedDays;
                policyInterval.ToDate = interval.ToDate;
                policyIntervals.Add(policyInterval);
            }
            return policyIntervals;
        }
        private IList<PrefillDriverDto> ToPrefillDriverDtos(int _isoMasterId, IList<dynamic> drivers)
        {
            DateTime dateValue;
            var enUS = new CultureInfo("en-US");
            var prefillDataAccess = new PrefillDataAccess();
            var prefillWorker = new PrefillWorker();
            var prefillDrivers = new List<PrefillDriverDto>();
            var previousPrefillDrivers = prefillDataAccess.GetPreviousPrefillDrivers(QuoteId);
            var _addressChanged = prefillWorker.GetAddressChange(QuoteId, _isoMasterId);
            var _raterID = prefillWorker.GetRaterID(QuoteId);

            if (previousPrefillDrivers.Count > 1 || (_raterID > 0 && previousPrefillDrivers.Count > 0))
            {
                foreach (var prefillDrv in previousPrefillDrivers)
                {
                    if (prefillDrv.DOB.Length < 19)
                    {
                        prefillDrv.DOB = prefillDrv.DOB.Insert(4, "/");
                        prefillDrv.DOB = prefillDrv.DOB.Insert(7, "/");
                    }
                }
            }
            foreach (var driver in drivers)
            {
                var prefillMatch = new List<Staging_Driver>();
                var driverDOB = DateTime.TryParseExact(driver.DateOfBirth, "yyyyMMdd", enUS, DateTimeStyles.None, out dateValue) ? DateTime.ParseExact(driver.DateOfBirth, "yyyyMMdd", enUS, DateTimeStyles.None) : DateTime.MinValue;

                if (previousPrefillDrivers.Count > 1 || (_raterID > 0 && previousPrefillDrivers.Count > 0))
                {
                    // PrefillMatch is to is to prevent duplicate prefill drivers
                    if (string.IsNullOrEmpty(driver.DateOfBirth) != true)
                    {
                        prefillMatch = previousPrefillDrivers.Where(w => w.DrvFirstName == driver.FirstName &&
                        w.DrvLastName == driver.LastName && Convert.ToDateTime(w.DOB) == driverDOB).ToList();
                    }
                    else
                    {
                        prefillMatch = previousPrefillDrivers.Where(w => w.DrvFirstName == driver.FirstName &&
                        w.DrvLastName == driver.LastName).ToList();
                    }
                    if (prefillMatch.Count > 0)
                    {
                        var _rejectDriver = prefillMatch[0];
                        var _rejectionType = prefillWorker.GetRejectionType(_addressChanged, _raterID, false);
                        prefillDataAccess.InsertDriverRejectType(QuoteId, _isoMasterId, _rejectDriver, _rejectionType);
                    }
                }
                if (prefillMatch.Count.Equals(0) && driver.FirstName != null && driver.LastName != null)
                {
                    var prefillDriver = new PrefillDriverDto();
                    prefillDriver.quoteID = QuoteId;
                    prefillDriver.ISOMasterID = driver.ISOMasterID;
                    prefillDriver.DateOfBirth = driverDOB;
                    prefillDriver.DLNumber = driver.DLNumber;
                    prefillDriver.FirstName = driver.FirstName;
                    prefillDriver.Gender = driver.Gender;
                    prefillDriver.LastName = driver.LastName;
                    prefillDriver.MiddleName = driver.MiddleName;
                    prefillDriver.SSN = driver.SSN;
                    prefillDriver.Address = driver.Address;
                    prefillDriver.City = driver.City;
                    prefillDriver.State = driver.State;
                    prefillDriver.ZipCode = driver.ZipCode;
                    prefillDriver.rowID = driver.rowID;
                    prefillDrivers.Add(prefillDriver);
                }
            }
            return prefillDrivers;
        }
        private IList<PrefillVehicleDto> ToPrefillVehicleDtos(int _isoMasterId, IList<dynamic> vehicles)
        {
            var prefillDataAccess = new PrefillDataAccess();
            var prefillWorker = new PrefillWorker();
            var prefillVehicles = new List<PrefillVehicleDto>();
            var previousPrefillVehicles = prefillDataAccess.GetPreviousPrefillVehicles(QuoteId);
            var addressChanged = prefillWorker.GetAddressChange(QuoteId, _isoMasterId);
            var raterID = prefillWorker.GetRaterID(QuoteId);

            foreach (var vehicle in vehicles)
            {
                // prefillMatch is to is to prevent duplicate prefill vehicles
                var prefillMatch = new List<ISOVehicles>();

                if (previousPrefillVehicles.Count > 0)
                {
                    prefillMatch = previousPrefillVehicles.Where(w => w.VIN == vehicle.VIN).ToList();
                }
                if (prefillMatch.Count > 0)
                {
                    var _rejectVehicle = prefillMatch[0];
                    var _rejectType = prefillWorker.GetRejectionType(addressChanged, raterID, false);
                    prefillDataAccess.InsertVehicleRejectType(QuoteId, _isoMasterId, _rejectVehicle, _rejectType);
                }
                if (prefillMatch.Count.Equals(0))
                {
                    if (string.IsNullOrWhiteSpace(vehicle.Make) || string.IsNullOrWhiteSpace(vehicle.ShortModel))
                    {
                        VINResultDto vinDto = new VINResultDto();
                        VehicleLookupRAPAService lookUp = new VehicleLookupRAPAService("NA");
                        vinDto = lookUp.GetVINResults(vehicle.VIN);
                        if (vinDto.ISODirectMatch == true)
                        {
                            vehicle.Make = vinDto.VinItems[0].Make;
                            vehicle.ShortModel = vinDto.VinItems[0].ShortModel;
                        }
                    }
                    var prefillVehicle = new PrefillVehicleDto();
                    prefillVehicle.ISOMasterID = vehicle.ISOMasterID;
                    prefillVehicle.ModelYear = vehicle.ModelYear;
                    prefillVehicle.quoteID = vehicle.quoteID;
                    prefillVehicle.VIN = vehicle.VIN;
                    prefillVehicle.rowID = vehicle.rowID;
                    prefillVehicle.Make = vehicle.Make;
                    prefillVehicle.ShortModel = vehicle.ShortModel;
                    prefillVehicle.LienHolderSequenceNumber = vehicle.LienHolderSequenceNumber.ToString();
                    prefillVehicle.LienHolderName = vehicle.LienHolderName;
                    prefillVehicle.LienHolderStreetAddress = vehicle.LienHolderStreetAddress;
                    prefillVehicle.LienHolderCity = vehicle.LienHolderCity;
                    prefillVehicle.LienHolderState = vehicle.LienHolderState;
                    prefillVehicle.LienHolderZipCode = vehicle.LienHolderZipCode;
                    prefillVehicles.Add(prefillVehicle);
                }
            }
            return prefillVehicles;
        }

        private void PopulateCVProperties(CoverageVerifierResponseDto cvr, int? supplierID)
        {
            if (supplierID == 2)
            {
                var programMasterDataAccess = new ProgramMasterDataAccess();
                var programMaster = programMasterDataAccess.GetProgramMaster(QuoteId);
                var Lapesdayslimit = Convert.ToInt16(programMaster.CVLapsedDaysLimit);
                var LNcvw = new LNcoverageValidationWorker();

                LNcvw.coverageValidationWorker(PolicyDatas, CoverageIntervals);
                LNcvw.Solve(PolicyHolderName, CarrierName, EffectiveDate, Lapesdayslimit, QuoteId);
                cvr.LastTermExpirationDate = LNcvw.LastExpirationDate;
                cvr.LapseDays = LNcvw.LaspseDays;
                cvr.LapseDaysLimit = Lapesdayslimit;
                cvr.IndividualLimit = LNcvw.IndividualLimit.HasValue ? LNcvw.IndividualLimit.Value : 0;
                cvr.OccurranceLimit = LNcvw.OccurranceLimit.HasValue ? LNcvw.OccurranceLimit.Value : 0;
                cvr.PolicyIsStandard = LNcvw.PolicyIsStandard.HasValue && LNcvw.PolicyIsStandard.Value;
                cvr.PolicyState = LNcvw.PolicyState;
                cvr.PriorWasAsic = LNcvw.LatestIsAsic;
                cvr.PriorWasAvic = LNcvw.LatestIsAvic;
                cvr.PriorWasHS = LNcvw.LatestIsHS;
            }
            else
            {
                var programMasterDataAccess = new ProgramMasterDataAccess();
                var programMaster = programMasterDataAccess.GetProgramMaster(QuoteId);
                var Lapesdayslimit = Convert.ToInt16(programMaster.CVLapsedDaysLimit);
                var cvw = new CoverageVerifierWorker(PolicyDatas, CoverageIntervals);

                cvw.Solve(PolicyHolderName, CarrierName, EffectiveDate);
                cvr.LastTermExpirationDate = cvw.LastExpirationDate;
                cvr.LapseDays = cvw.LaspseDays;
                cvr.LapseDaysLimit = Lapesdayslimit;
                cvr.IndividualLimit = cvw.IndividualLimit.HasValue ? cvw.IndividualLimit.Value : 0;
                cvr.OccurranceLimit = cvw.OccurranceLimit.HasValue ? cvw.OccurranceLimit.Value : 0;
                cvr.PolicyIsStandard = cvw.PolicyIsStandard.HasValue && cvw.PolicyIsStandard.Value;
                cvr.PolicyState = cvw.PolicyState;
                cvr.PriorWasAsic = cvw.LatestIsAsic;
                cvr.PriorWasAvic = cvw.LatestIsAvic;
                cvr.PriorWasHS = cvw.LatestIsHS;
            }
        }
        private void PopulatePrefillProperties(PrefillResponseDto pfr)
        {
            if (PrefillDrivers == null || PrefillVehicles == null)
            {
                throw new Exception("Prefill Response Failure!");
            }
            pfr.PrefillDrivers = MaskFields(PrefillDrivers);
            pfr.PrefillVehicles = PrefillVehicles;
        }
        private IList<PrefillDriverDto> MaskFields(IList<PrefillDriverDto> prefillDrivers)
        {
            var safeList = new List<PrefillDriverDto>();
            //Want drivers with DOBs before ones without and want drivers in order from oldest to youngest.
            foreach (var drv in prefillDrivers.Where(pd => pd.DateOfBirth != DateTime.MinValue).OrderBy(pd => pd.DateOfBirth))
            {
                safeList.Add(new PrefillDriverDto()
                {
                    DateOfBirth = drv.DateOfBirth, // may omit at later date
                    DLNumber = drv.DLNumber.SafeTrimU(), // may omit at later date
                    FirstName = drv.FirstName.SafeTrimU(),
                    Gender = drv.Gender.SafeTrimU(),
                    ISOMasterID = drv.ISOMasterID,
                    LastName = drv.LastName.SafeTrimU(),
                    MaskedDLNumber = drv.DLNumber.SafeTrimU().MaskedDL(),
                    MaskedDOB = drv.DateOfBirth.MaskedDOB(),
                    MaskedSSN = drv.SSN.SafeTrim().MaskedSSN(),
                    MiddleName = drv.MiddleName.SafeTrimU(),
                    Address = drv.Address.SafeTrimU(),
                    City = drv.City.SafeTrimU(),
                    State = drv.State.SafeTrimU(),
                    ZipCode = drv.ZipCode.SafeTrimU(),
                    quoteID = QuoteId,
                    rowID = drv.rowID,
                    SSN = drv.SSN.SafeTrimU()
                });
            }
            //now drivers with no dob
            foreach (var drv in prefillDrivers.Where(pd => pd.DateOfBirth == DateTime.MinValue))
            {
                safeList.Add(new PrefillDriverDto()
                {
                    FirstName = drv.FirstName,
                    Gender = drv.Gender,
                    ISOMasterID = drv.ISOMasterID,
                    LastName = drv.LastName,
                    MaskedDLNumber = drv.DLNumber.SafeTrimU().MaskedDL(),
                    MaskedDOB = drv.DateOfBirth.MaskedDOB(),
                    MaskedSSN = drv.SSN.SafeTrim().MaskedSSN(),
                    MiddleName = drv.MiddleName,
                    Address = drv.Address.SafeTrimU(),
                    City = drv.City.SafeTrimU(),
                    State = drv.State.SafeTrimU(),
                    ZipCode = drv.ZipCode.SafeTrimU(),
                    quoteID = QuoteId,
                    rowID = drv.rowID
                });
            }
            return safeList;
        }
    }
}
