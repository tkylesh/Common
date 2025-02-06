using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using CommonAPICommon.Dto;
using CommonAPICommon;
using LexisNexisService;
using CommonAPICommon.Dto;
//using CommonAPICommon.Dto;

namespace CommonAPIDAL.DataAccess.Mapping
{
    public class LexisNexisMapper
    {
        #region "Serializers"
        public string SerializeISO(ISO isoobj)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(CommonAPICommon.Dto.ISO));
            StringWriter sww = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            xsSubmit.Serialize(writer, isoobj);
            var xml = sww.ToString();
            return xml;
        }
        public string Serialize(ClientResponse request)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(ClientResponse));
            StringWriter sww = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            xsSubmit.Serialize(writer, request);
            var xml = sww.ToString();
            return xml;
        }
        public string SerializeRequest(Request request)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(Request));
            StringWriter sww = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            xsSubmit.Serialize(writer, request);
            var requestXml = sww.ToString();
            return requestXml;
        }
        public string SerializeResponse(ClientResponse response)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(ClientResponse));
            StringWriter sww = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            xsSubmit.Serialize(writer, response);
            var responesXml = sww.ToString();
            return responesXml;
        }
        public ClientResponse DeserializeLN(string xml)
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
        #endregion

        #region "Mappers"
        public ISO mapPrefill(ClientResponse response, int quoteID, int isoMasterId)
        {
            ISO _iso = new ISO();
            if (response != null)
            {
                _iso.PassportSvcRs = new ISOPassportSvcRs();
                _iso.PassportSvcRs.Status = new ISOPassportSvcRsStatus();
                var PrefillResults = response.ProductResults.AutoDataPrefillResults;
                var LNresponse = response.SearchBy.Subjects[0];
                var transactionDetailEx = response?.TransactionDetailsEx;
                _iso.PassportSvcRs.RequestId = transactionDetailEx?.TransactionId;
                _iso.PassportSvcRs.Status.StatusDesc = transactionDetailEx?.ProcessingStatus;

                if (transactionDetailEx.ProcessingStatus.ToUpper() == "COMPLETE SUCCESS")
                {
                    _iso.PassportSvcRs.Status.StatusCd = "8";
                }
                List<ISOPassportSvcRsReport> ISOPassportSvcRsReports = new List<ISOPassportSvcRsReport>();
                ISOPassportSvcRsReports.Add(CreateReportHeader(PrefillResults.First(), transactionDetailEx));
                ISOPassportSvcRsReports.Add(CreateUdiReport(PrefillResults.First(), transactionDetailEx));
                ISOPassportSvcRsReports.Add(CreateCoverageVerifierReport(PrefillResults.First(), transactionDetailEx, quoteID, isoMasterId));
                ISOPassportSvcRsReports.Add(CreatePolkReport(PrefillResults.First(), transactionDetailEx));

                _iso.PassportSvcRs.Reports = ISOPassportSvcRsReports.ToArray();
            }
            return _iso;
        }
        #endregion

        #region "Map Report Header"
        private ISOPassportSvcRsReport CreateReportHeader(AutoDataPrefillResult PrefillResult, TransactionDetailsEx transactionDetailEx)
        {
            ISOPassportSvcRsReport reportheader = new ISOPassportSvcRsReport();
            reportheader.Status = new ISOPassportSvcRsReportStatus();
            reportheader.ReportData = new ISOPassportSvcRsReportReportData();
            reportheader.ProductCd = "LN_ADPF_PREFILL";
            reportheader.ReportName = "LN_ADPF_PREFILL REPORT";
            var driverDiscoveryStatus = PrefillResult.Summary.DriverDiscoveryReport.Status;
            var NumberOfReportedDriverDisovery = PrefillResult.Summary.DriverDiscoveryReport?.NumberOfReportedDriverDisovery;

            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.DriverDiscoveryreported)
            {
                reportheader.Status.StatusDesc = "Driver Discovery reported";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.NoDriverDiscoveryreported)
            {
                reportheader.Status.StatusDesc = "No Driver Discovery reported";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.notprocessedinvalidLexisNexisAccountNumber)
            {
                reportheader.Status.StatusDesc = "not processed; invalid LexisNexis Account Number";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.notprocessedinsufficientsearchdata)
            {
                reportheader.Status.StatusDesc = "not processed; insufficient search data";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.DriverDiscoverysearchunavailable)
            {
                reportheader.Status.StatusDesc = "Driver Discovery search unavailable";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.DriverDiscoverysearchnotrequested)
            {
                reportheader.Status.StatusDesc = "Driver Discovery search not requested";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.DriverDiscoverystateunavailable)
            {
                reportheader.Status.StatusDesc = "Driver Discovery  state unavailable";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.RequestedstatenotonDriverDiscoverydatabase)
            {
                reportheader.Status.StatusDesc = "Requested state not on Driver Discovery database";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.Searchnotprocessedstatelimitation)
            {
                reportheader.Status.StatusDesc = "Search not processed; state limitation";
            }
            var transactionDateOrdered = transactionDetailEx.TransactionDetails.DateTimeReceived;
            var transactionDateComplete = transactionDetailEx.TransactionDetails.DateTimeCompleted;

            reportheader.OrderTimeStamp = transactionDateOrdered.Year.ToString() + "/" + transactionDateOrdered.Month.ToString("00") + "/" + transactionDateOrdered.Day.ToString("00");
            reportheader.CompletedTimeStamp = transactionDateComplete.Year.ToString() + "/" + transactionDateComplete.Month.ToString("00") + "/" + transactionDateComplete.Day.ToString("00");
            reportheader.ReportData.ConditionalOrdering = new ISOPassportSvcRsReportReportDataConditionalOrdering();
            reportheader.ReportData.ConditionalOrdering.ResponseSummary = new ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummary();

            List<ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS> _ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS = new List<ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS>();
            if (PrefillResult.DriverDiscovery?.CurrentCarrier?.Attach?.DiscoveredVehicles != null)
            {
                foreach (var vehicle in PrefillResult.DriverDiscovery.CurrentCarrier.Attach.DiscoveredVehicles.Where(w => w.VehicleDetail.DecodedVin.Type.ToUpper() != "MOTORCYCLE").ToList())
                {
                    {
                        ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS vins = new ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS();

                        vins.VIN = vehicle.VehicleDetail.VIN;
                        vins.Year = vehicle.VehicleDetail.Year.ToString();
                        vins.Make = vehicle.VehicleDetail.Make;
                        vins.Model = vehicle.VehicleDetail.Model;
                        vins.Source = "";

                        _ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS.Add(vins);
                    };
                }
            }
            if (PrefillResult.VehicleIndentification?.DiscoveredVehicles != null)
            {
                if (PrefillResult.DriverDiscovery?.CurrentCarrier?.Attach?.DiscoveredVehicles != null)
                {
                    foreach (var VINvehicle in PrefillResult.VehicleIndentification?.DiscoveredVehicles)
                    {
                        // This is to prevent duplicates from CurrentCarrier DiscoveredVehicles and motorcycles
                        var DuplicateOrMotorcycle = PrefillResult.DriverDiscovery.CurrentCarrier.Attach.DiscoveredVehicles.Where(w => w.VehicleDetail.VIN == VINvehicle.VIN || VINvehicle.DecodedVin.Type.ToUpper() == "MOTORCYCLE").ToList();
                        if (DuplicateOrMotorcycle.Count == 0)
                        {
                            ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS vins = new ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS();
                            vins.VIN = VINvehicle.VIN;
                            vins.Make = VINvehicle.Make;
                            vins.Model = VINvehicle.Model;
                            vins.Year = VINvehicle.Year.ToString();

                            _ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS.Add(vins);
                        }
                    }
                };
            }
            if (PrefillResult.VehicleIndentification?.DiscoveredVehicles != null)
            {
                if (PrefillResult.DriverDiscovery?.CurrentCarrier?.Attach?.DiscoveredVehicles == null)
                {
                    foreach (var VINvehicle in PrefillResult.VehicleIndentification?.DiscoveredVehicles)
                    {
                        // This is to prevent motorcycles
                        if (VINvehicle.DecodedVin.Type.ToUpper() != "MOTORCYCLE")
                        {
                            ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS vins = new ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS();
                            vins.VIN = VINvehicle.VIN;
                            vins.Make = VINvehicle.Make;
                            vins.Model = VINvehicle.Model;
                            vins.Year = VINvehicle.Year.ToString();

                            _ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS.Add(vins);
                        }
                    }
                };
            }
            if (_ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS.Count > 0)
            {
                reportheader.ReportData.ConditionalOrdering.ResponseSummary.VINS = _ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS.ToArray();
            }
            List<ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER> _ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER = new List<ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER>();
            if (PrefillResult.DriverDiscovery?.CurrentCarrier?.Attach?.DiscoveredSubjects != null)
            {
                foreach (var item in PrefillResult.DriverDiscovery.CurrentCarrier.Attach.DiscoveredSubjects)
                {
                    if (item.DriversLicense != null)
                    {
                        foreach (var subject in item.DriversLicense)
                        {
                            {
                                ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER Driver = new ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER();
                                Driver.FirstName = item.Name.First;
                                Driver.MiddleName = item.Name.Middle;
                                Driver.LastName = item.Name.Last;
                                Driver.DLNumber = subject.DriversLicenseNumber;
                                Driver.DLState = subject.DriversLicenseState;
                                Driver.DateOfBirth = item.BirthDate?.Year.ToString() + item.BirthDate?.Month.ToString("00") + item.BirthDate?.Day.ToString("00");
                                //Driver.SSN = item.SSN;
                                Driver.Source = "";

                                _ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER.Add(Driver);
                            };
                        }
                    }
                    else
                    {
                        {
                            ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER Driver = new ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER();
                            Driver.FirstName = item.Name.First;
                            Driver.MiddleName = item.Name.Middle;
                            Driver.LastName = item.Name.Last;
                            //Driver.DLNumber = subject.DriversLicenseNumber;
                            //Driver.DLState = subject.DriversLicenseState;
                            Driver.DateOfBirth = item.BirthDate?.Year.ToString() + item.BirthDate?.Month.ToString("00") + item.BirthDate?.Day.ToString("00");
                            //Driver.SSN = item.SSN;
                            Driver.Source = "";

                            _ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER.Add(Driver);
                        };
                    }
                }
            }
            if (_ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER.Count > 0)
            {
                reportheader.ReportData.ConditionalOrdering.ResponseSummary.DRIVER = _ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER.ToArray();
            }
            return reportheader;
        }
        #endregion

        #region "Map UDI Report"
        private ISOPassportSvcRsReport CreateUdiReport(AutoDataPrefillResult PrefillResult, TransactionDetailsEx transactionDetailEx)
        {
            ISOPassportSvcRsReport UdiReport = new ISOPassportSvcRsReport();
            UdiReport.Status = new ISOPassportSvcRsReportStatus();
            UdiReport.ReportData = new ISOPassportSvcRsReportReportData();
            UdiReport.ReportData.CoverageVerifier = new ISOPassportSvcRsReportReportDataCoverageVerifier();
            UdiReport.ReportData.ACXIOM = new ISOPassportSvcRsReportReportDataACXIOM();
            UdiReport.ReportData.ACXIOM.PRODRESHEAD = new ISOPassportSvcRsReportReportDataACXIOMPRODRESHEAD();
            UdiReport.ReportData.ACXIOM.REQGRPHEAD = new ISOPassportSvcRsReportReportDataACXIOMREQGRPHEAD();
            UdiReport.ReportData.ACXIOM.SUPADDRLAYOUT = new ISOPassportSvcRsReportReportDataACXIOMSUPADDRLAYOUT();
            UdiReport.ReportData.ACXIOM.SUPINDIVLAYOUT = new ISOPassportSvcRsReportReportDataACXIOMSUPINDIVLAYOUT();
            UdiReport.ReportData.ACXIOM.RETRESPONSE = new ISOPassportSvcRsReportReportDataACXIOMRETRESPONSE();
            UdiReport.ReportData.ACXIOM.RETRESPONSE.RETADDRLAYOUT = new ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUT();
            List<ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUT> _ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUT = new List<ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUT>();
            List<ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT> _ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT = new List<ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT>();

            if (PrefillResult.SearchDataSet?.Subjects[0].BirthDate != null || PrefillResult.SearchDataSet?.Subjects[0].DriversLicense != null || PrefillResult.SearchDataSet?.Subjects[0].SSN != "000000000")
            {
                foreach (var subject in PrefillResult.SearchDataSet.Subjects)
                {
                    {
                        ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUT RetIndAddress = new ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUT();
                        ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT RetIndLayout = new ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT();

                        RetIndLayout.RI_FIRSTNAME = subject.Name.First;
                        RetIndLayout.RI_MIDDLEINITIAL = subject.Name.Middle;
                        RetIndLayout.RI_LASTNAME = subject.Name.Last;
                        RetIndLayout.RI_DLNUMBER = subject.DriversLicense?[0].DriversLicenseNumber;
                        RetIndLayout.RI_DLSTATECODE = subject.DriversLicense?[0].DriversLicenseState;
                        RetIndLayout.RI_DOB = subject.BirthDate?.Year.ToString() + subject.BirthDate?.Month.ToString("00") + subject.BirthDate?.Day.ToString("00");
                        //RetIndLayout.RI_SSN = subject?.SSN;
                        var streetNumber = subject.Address[0].StreetNumber;
                        var streetAddress = subject.Address[0].StreetAddress1;
                        RetIndAddress.RA_ADRESS = streetNumber + " " + streetAddress;
                        RetIndAddress.RA_CITY = subject.Address[0].City;
                        RetIndAddress.RA_STATE = subject.Address[0].State;
                        RetIndAddress.RA_ZIPCODE = subject.Address[0].Zip5;

                        if (subject.Gender == GenderEnum.Female)
                        {
                            RetIndLayout.RI_GENDER = "F";
                        }
                        if (subject.Gender == GenderEnum.Male)
                        {
                            RetIndLayout.RI_GENDER = "M";
                        }
                        _ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUT.Add(RetIndAddress);
                        _ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT.Add(RetIndLayout);
                    };
                }
            }
            if (PrefillResult.DriverDiscovery?.AdditionalDriverDiscovery != null)
            {
                foreach (var additionalDriver in PrefillResult.DriverDiscovery.AdditionalDriverDiscovery)
                {
                    {
                        ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT RetIndLayout = new ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT();

                        RetIndLayout.RI_FIRSTNAME = additionalDriver.Name.First;
                        RetIndLayout.RI_MIDDLEINITIAL = additionalDriver.Name.Middle;
                        RetIndLayout.RI_LASTNAME = additionalDriver.Name.Last;
                        RetIndLayout.RI_DLNUMBER = additionalDriver.DriversLicense?[0].DriversLicenseNumber;
                        RetIndLayout.RI_DLSTATECODE = additionalDriver.DriversLicense?[0].DriversLicenseState;
                        RetIndLayout.RI_DOB = additionalDriver.BirthDate?.Year.ToString() + additionalDriver.BirthDate?.Month.ToString("00") + additionalDriver.BirthDate?.Day.ToString("00");

                        if (additionalDriver.Gender == GenderEnum.Female)
                        {
                            RetIndLayout.RI_GENDER = "F";
                        }
                        if (additionalDriver.Gender == GenderEnum.Male)
                        {
                            RetIndLayout.RI_GENDER = "M";
                        }
                        _ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT.Add(RetIndLayout);
                    };
                }
            }
            if (_ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUT.Count > 0)
            {
                UdiReport.ReportData.ACXIOM.RETRESPONSE.RETADDRLAYOUT = _ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUT[0];
            }
            if (_ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT.Count > 0)
            {
                UdiReport.ReportData.ACXIOM.RETRESPONSE.RETADDRLAYOUT.RETINDIVLAYOUT = _ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT.ToArray();
            }
            UdiReport.ProductCd = "LN_ADPF_UDI";
            UdiReport.ReportName = "LN_ADPF_UDI REPORT";

            var driverDiscoveryStatus = PrefillResult.Summary.DriverDiscoveryReport.Status;
            var NumberOfReportedDriverDisovery = PrefillResult.Summary.DriverDiscoveryReport?.NumberOfReportedDriverDisovery;

            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.DriverDiscoveryreported)
            {
                UdiReport.Status.StatusDesc = "Driver Discovery reported";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.NoDriverDiscoveryreported)
            {
                UdiReport.Status.StatusDesc = "No Driver Discovery reported";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.notprocessedinvalidLexisNexisAccountNumber)
            {
                UdiReport.Status.StatusDesc = "not processed; invalid LexisNexis Account Number";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.notprocessedinsufficientsearchdata)
            {
                UdiReport.Status.StatusDesc = "not processed; insufficient search data";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.DriverDiscoverysearchunavailable)
            {
                UdiReport.Status.StatusDesc = "Driver Discovery search unavailable";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.DriverDiscoverysearchnotrequested)
            {
                UdiReport.Status.StatusDesc = "Driver Discovery search not requested";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.DriverDiscoverystateunavailable)
            {
                UdiReport.Status.StatusDesc = "Driver Discovery  state unavailable";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.RequestedstatenotonDriverDiscoverydatabase)
            {
                UdiReport.Status.StatusDesc = "Requested state not on Driver Discovery database";
            }
            if (driverDiscoveryStatus == AutoDataPrefillADDStatus.Searchnotprocessedstatelimitation)
            {
                UdiReport.Status.StatusDesc = "Search not processed; state limitation";
            }
            var transactionDateOrdered = transactionDetailEx.TransactionDetails.DateTimeReceived;
            var transactionDateComplete = transactionDetailEx.TransactionDetails.DateTimeCompleted;
            UdiReport.OrderTimeStamp = transactionDateOrdered.Year.ToString() + "/" + transactionDateOrdered.Month.ToString() + "/" + transactionDateOrdered.Day.ToString();
            UdiReport.CompletedTimeStamp = transactionDateComplete.Year.ToString() + "/" + transactionDateComplete.Month.ToString() + "/" + transactionDateComplete.Day.ToString();

            return UdiReport;
        }
        #endregion
        #region "Map Coverage Verifier Report"
        private ISOPassportSvcRsReport CreateCoverageVerifierReport(AutoDataPrefillResult PrefillResult, TransactionDetailsEx transactionDetailEx, int quoteID, int isoMasterId)
        {
            ISOPassportSvcRsReport reportCV = new ISOPassportSvcRsReport();
            reportCV.Status = new ISOPassportSvcRsReportStatus();
            reportCV.ReportData = new ISOPassportSvcRsReportReportData();
            reportCV.ReportData.CoverageVerifier = new ISOPassportSvcRsReportReportDataCoverageVerifier();
            reportCV.ReportData.CoverageVerifier.StaticInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierStaticInfo();
            reportCV.ReportData.CoverageVerifier.Policy = new List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicy>() { new ISOPassportSvcRsReportReportDataCoverageVerifierPolicy() }.ToArray();
            reportCV.ReportData.CoverageVerifier.Policy[0].HistorySection = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySection();
            reportCV.ReportData.CoverageVerifier.Policy[0].DetailSection = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSection();
            reportCV.ReportData.CoverageVerifier.HeaderInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierHeaderInfo();
            reportCV.ReportData.CoverageVerifier.SummarySection = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySection();
            reportCV.ReportData.CoverageVerifier.TrailerInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierTrailerInfo();
            List<ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo> _ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo = new List<ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo>();
            List<ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2> _ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2 = new List<ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2>();
            List<ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval> _ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval = new List<ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval>();
            List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicy> _ISOPassportSvcRsReportReportDataCoverageVerifierPolicy = new List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicy>();
            ISOPassportSvcRsReportReportDataCoverageVerifierSummarySection iSOPassportSvcRsReportReportDataCoverageVerifierSummarySection = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySection();
            ISOPassportSvcRsReportReportDataCoverageVerifierSummarySection _ISOPassportSvcRsReportReportDataCoverageVerifierSummarySection = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySection();
            List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfoGarageAddress> _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfoGarageAddress = new List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfoGarageAddress>();
            List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo> _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo = new List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo>();
            List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfoSubjectName> _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfoSubjectName = new List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfoSubjectName>();
            List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionTransactionInfo> _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionTransactionInfo = new List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionTransactionInfo>();
            List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySection> _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySection = new List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySection>();
            ISOPassportSvcRsReportReportDataCoverageVerifierSummarySection summary = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySection();

            var currentCarrierAdmin = PrefillResult.DriverDiscovery.CurrentCarrier.CurrentCarrierReport.Admin;
            var currentCarrierReport = PrefillResult.DriverDiscovery.CurrentCarrier.CurrentCarrierReport.Report;
            var currentCarrierReportStatus = PrefillResult.Summary.CurrentCarrierReport.Status;
            var transactionDateOrdered = transactionDetailEx.TransactionDetails.DateTimeReceived;
            var transactionDateComplete = transactionDetailEx.TransactionDetails.DateTimeCompleted;
            reportCV.Status.StatusDesc = currentCarrierReportStatus;
            reportCV.CriticalResult = currentCarrierReportStatus;
            reportCV.OrderTimeStamp = transactionDateOrdered.Year.ToString() + "/" + transactionDateOrdered.Month.ToString() + "/" + transactionDateOrdered.Day.ToString();
            reportCV.CompletedTimeStamp = transactionDateComplete.Year.ToString() + "/" + transactionDateComplete.Month.ToString() + "/" + transactionDateComplete.Day.ToString();
            reportCV.ProductCd = "LN_ADPF_CV";
            reportCV.ReportName = "LN_ADPF_COVERAGE VERIFIER";

            if (PrefillResult.SearchDataSet?.Subjects[0].BirthDate != null || PrefillResult.SearchDataSet?.Subjects[0].DriversLicense != null || PrefillResult.SearchDataSet?.Subjects[0].SSN != "000000000")
            {
                foreach (var subject in PrefillResult.SearchDataSet.Subjects)
                {
                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo Driver = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo();
                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfoSubjectName Name = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfoSubjectName();
                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfoGarageAddress Address = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfoGarageAddress();
                    Name.FirstName = subject.Name.First;
                    Name.MiddleName = subject.Name.Middle;
                    Name.LastName = subject.Name.Last;
                    var streetNumber = subject.Address[0].StreetNumber;
                    var streetAddress = subject.Address[0].StreetAddress1;
                    Address.AddressLine1 = streetNumber + " " + streetAddress;
                    Address.City = subject.Address[0].City;
                    Address.State = subject.Address[0].State;
                    Address.Zip = subject.Address[0].Zip5;
                    Driver.SubjectName = Name;
                    Driver.DLNumber = subject.DriversLicense?[0].DriversLicenseNumber;
                    Driver.DLState = subject.DriversLicense?[0].DriversLicenseState;
                    Driver.DateOfBirth = subject.BirthDate?.Year.ToString() + subject.BirthDate?.Month.ToString("00") + subject.BirthDate?.Day.ToString("00");
                    //Driver.SSN = subject?.SSN;
                    if (Driver.Gender != null)
                    {
                        if (subject.Gender == GenderEnum.Female)
                        {
                            Driver.Gender = "F";
                        }
                        if (subject.Gender == GenderEnum.Male)
                        {
                            Driver.Gender = "M";
                        }
                    }
                    _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo.Add(Driver);
                }
            }
            if (PrefillResult.DriverDiscovery?.AdditionalDriverDiscovery != null)
            {
                foreach (var additionalDriver in PrefillResult.DriverDiscovery.AdditionalDriverDiscovery)
                {
                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo Driver = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo();
                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfoSubjectName Name = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfoSubjectName();
                    Name.FirstName = additionalDriver.Name.First;
                    Name.MiddleName = additionalDriver.Name.Middle;
                    Name.LastName = additionalDriver.Name.Last;
                    Driver.SubjectName = Name;
                    Driver.DLNumber = additionalDriver.DriversLicense?[0].DriversLicenseNumber;
                    Driver.DLState = additionalDriver.DriversLicense?[0].DriversLicenseState;
                    Driver.DateOfBirth = additionalDriver.BirthDate?.Year.ToString() + additionalDriver.BirthDate?.Month.ToString("00") + additionalDriver.BirthDate?.Day.ToString("00");
                    //Driver.SSN = additionalDriver?.SSN;

                    if (additionalDriver.Gender == GenderEnum.Female)
                    {
                        Driver.Gender = "F";
                    }
                    if (additionalDriver.Gender == GenderEnum.Male)
                    {
                        Driver.Gender = "M";
                    }
                    _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo.Add(Driver);
                }
            }
            if (PrefillResult.DriverDiscovery?.CurrentCarrier?.CurrentCarrierReport?.Report?.PolicyInformation != null)
            {
                //// This saves all prefill policy info 
                PrefillDataAccess prefillDataAccess = new PrefillDataAccess();
                prefillDataAccess.SavePrefillPolicyData(quoteID, isoMasterId, PrefillResult);

                foreach (var PolInfo in PrefillResult.DriverDiscovery.CurrentCarrier.CurrentCarrierReport.Report.PolicyInformation)
                {
                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCarrierInfo IsoPolicyCarrierInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCarrierInfo();
                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCarrierInfo _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCarrierInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCarrierInfo();
                    List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionVehicleInfo> _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionVehicleInfo = new List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionVehicleInfo>();
                    List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo> _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo = new List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo>();
                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfo _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfo();
                    List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfo> _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfo = new List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfo>();
                    ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2 iSOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2 = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2();
                    ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval iSOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval();
                    ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo iSOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo();
                    List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfo> _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfo = new List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfo>();
                    List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfo> _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfo = new List<ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfo>();
                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicy iSOPassportSvcRsReportReportDataCoverageVerifierPolicy = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicy();
                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSection detail = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSection();
                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySection history = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySection();
                    detail.CarrierInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCarrierInfo();
                    detail.PolicyDetailInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfo();
                    detail.CoverageInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfo();
                    history.CarrierInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionCarrierInfo();
                    history.CoverageInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionCoverageInfo();
                    summary.PolicySearchInfo = new List<ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo>() { new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo() }.ToArray();
                    summary.RequestLevelInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionRequestLevelInfo();
                    summary.CustomerInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCustomerInfo();
                    summary.CoverageLapse2 = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2();
                    summary.CoverageLapse2.CoverageInterval = new List<ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval>() { new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval() }.ToArray();
                    IsoPolicyCarrierInfo.CarrierName = PolInfo.CarrierName;
                    IsoPolicyCarrierInfo.AMBest = PolInfo.AmbestNumber;
                    Date FromDate = null;
                    Date ToDate = null;
                    var primaryDriver = PolInfo.PolicyHoldersList.Where(w => w.PolicyHolderRelationship.ToString().ToUpper() == "PRIMARY");

                    if (primaryDriver != null)
                    {
                        foreach (var item in primaryDriver)
                        {
                            FromDate = item.PolicyFromDate;
                            ToDate = item.PolicyToDate;
                        }
                    }
                    var _inceptionYear = PolInfo.InceptionDate.Year.ToString() + PolInfo.InceptionDate.Month.ToString("00") + PolInfo.InceptionDate.Day.ToString("00");
                    var covFromDate = FromDate.Year.ToString() + FromDate.Month.ToString("00") + FromDate.Day.ToString("00");
                    var coveToDate = ToDate.Year.ToString() + ToDate.Month.ToString("00") + ToDate.Day.ToString("00");

                    _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCarrierInfo = IsoPolicyCarrierInfo;

                    if (PolInfo.CoveredVehiclesList != null)
                    {
                        if (PrefillResult.VehicleIndentification?.DiscoveredVehicles != null)
                        {
                            foreach (var vehicle in PolInfo.CoveredVehiclesList)
                            {
                                var veh = PrefillResult.VehicleIndentification?.DiscoveredVehicles.Where(w => w.VIN == vehicle.VINNumber && w.DecodedVin.Type.ToUpper() != "MOTORCYCLE").SingleOrDefault();
                                if (veh != null)
                                {
                                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfo IsoCoverageInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfo();
                                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionVehicleInfo IsoCoveredVechile = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionVehicleInfo();
                                    ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo IsoAddressCoverageInfoA = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo();
                                    IsoCoveredVechile.VIN = vehicle.VINNumber;
                                    IsoCoveredVechile.Year = vehicle.VehicleYear;
                                    IsoCoveredVechile.Make = vehicle.VehicleMake;
                                    IsoAddressCoverageInfoA.IndividualLimit = vehicle.AutocoverageLimitsInfo.BodilyInjuryLimit;
                                    IsoAddressCoverageInfoA.OccuranceLimit = vehicle.AutocoverageLimitsInfo.OccurrenceLimit;
                                    foreach (var autoCovInfo in vehicle.AutocoverageInfo)
                                    {
                                        if (autoCovInfo.ToString().ToUpper() == "BODILYINJURY")
                                        {
                                            IsoAddressCoverageInfoA.CoverageCode = "BodilyInjury";
                                            IsoAddressCoverageInfoA.CoverageCodeDesc = "BodilyInjury";
                                        }
                                    }
                                    IsoAddressCoverageInfoA.FromDate = covFromDate;
                                    IsoAddressCoverageInfoA.ToDate = coveToDate;

                                    _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo.Add(IsoAddressCoverageInfoA);
                                    _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionVehicleInfo.Add(IsoCoveredVechile);
                                }
                            }
                        }
                    }
                    if (_ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionVehicleInfo.Count > 0)
                    {
                        detail.VehicleInfo = _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionVehicleInfo.ToArray();
                    }
                    if (_ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo.Count > 0)
                    {
                        detail.CoverageInfo.AddressCoverageInfo = _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo.ToArray();
                    }
                    if (PolInfo.PolicyHoldersList != null)
                    {
                        foreach (var policyholder in PolInfo.PolicyHoldersList)
                        {
                            ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo IsoCoveredDriver =
                                    _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo.Where(w => w.SubjectName != null && w.SubjectName.FirstName == policyholder.PolicyHolderName.First && w.SubjectName.LastName == policyholder.PolicyHolderName.Last).SingleOrDefault();
                            ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfo IsoCoverageInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfo();
                            ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo IsoAddressCoverageInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo();
                            ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfo IsoPolicyInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfo();
                            ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfo IsoSubjectInfoHistory = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfo();
                            IsoSubjectInfoHistory.SubjectName = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfoSubjectName();
                            var relation = policyholder.PolicyHolderRelationship.ToString();

                            //SubjectInfo 
                            if (IsoCoveredDriver != null)
                            {
                                IsoCoveredDriver.SubjectName.FirstName = policyholder.PolicyHolderName.First;
                                IsoCoveredDriver.SubjectName.LastName = policyholder.PolicyHolderName.Last;
                                IsoCoveredDriver.FromDate = policyholder.PolicyFromDate.Year.ToString() + policyholder.PolicyFromDate.Month.ToString("00") + policyholder.PolicyFromDate.Day.ToString("00");
                                IsoCoveredDriver.ToDate = policyholder.PolicyToDate.Year.ToString() + policyholder.PolicyToDate.Month.ToString("00") + policyholder.PolicyToDate.Day.ToString("00");
                                IsoCoveredDriver.SubjectNo = policyholder.SubjectUnitNumber;
                                if (policyholder.PolicyHolderRelationship.ToString().ToUpper().Contains("PRIMARY"))
                                {
                                    IsoCoveredDriver.RelationCode = "PP";
                                    IsoCoveredDriver.RelationCodeDesc = "Primary";
                                }
                                if (policyholder.PolicyHolderRelationship.ToString().ToUpper().Contains("LISTED"))
                                {
                                    IsoCoveredDriver.RelationCode = "LD";
                                    IsoCoveredDriver.RelationCodeDesc = "Listed Driver";
                                }
                                if (policyholder.PolicyHolderRelationship.ToString().ToUpper().Contains("EXCLUDED"))
                                {
                                    IsoCoveredDriver.RelationCode = "EX";
                                    IsoCoveredDriver.RelationCodeDesc = "Excluded";
                                }
                            }
                            //Subject History
                            IsoSubjectInfoHistory.SubjectName.FirstName = policyholder.PolicyHolderName.First;
                            IsoSubjectInfoHistory.SubjectName.LastName = policyholder.PolicyHolderName.Last;
                            IsoSubjectInfoHistory.FromDate = policyholder.PolicyFromDate.Year.ToString() + policyholder.PolicyFromDate.Month.ToString("00") + policyholder.PolicyFromDate.Day.ToString("00");
                            IsoSubjectInfoHistory.ToDate = policyholder.PolicyToDate.Year.ToString() + policyholder.PolicyToDate.Month.ToString("00") + policyholder.PolicyToDate.Day.ToString("00");

                            //PolicyDetailInfo
                            IsoPolicyInfo.PolicyStatus = PolInfo.PolicyStatus.ToString();
                            IsoPolicyInfo.PolicyNumber = PolInfo.PolicyNumber;
                            IsoPolicyInfo.InceptionDate = PolInfo.InceptionDate.Year.ToString() + PolInfo.InceptionDate.Month.ToString("00") + PolInfo.InceptionDate.Day.ToString("00");
                            IsoPolicyInfo.CancellationReason = PolInfo.CancellationReasonCode;
                            IsoPolicyInfo.PolicyType = PolInfo.PolicyType.ToString();
                            IsoPolicyInfo.RiskClassCodeDesc = PolInfo.RiskType.ToString();
                            IsoPolicyInfo.TermEffectiveDate = policyholder.PolicyFromDate.Year.ToString() + policyholder.PolicyFromDate.Month.ToString("00") + policyholder.PolicyFromDate.Day.ToString("00");
                            IsoPolicyInfo.TermExpirationDate = policyholder.PolicyToDate.Year.ToString() + policyholder.PolicyToDate.Month.ToString("00") + policyholder.PolicyToDate.Day.ToString("00");
                            if (policyholder.LastCancelDate != null)
                            {
                                IsoPolicyInfo.CancellationDate = policyholder.LastCancelDate.Year.ToString() + policyholder.LastCancelDate.Month.ToString("00") + policyholder.LastCancelDate.Day.ToString("00");
                            }

                            detail.PolicyDetailInfo.TermEffectiveDate = policyholder.PolicyFromDate.Year.ToString() + policyholder.PolicyFromDate.Month.ToString("00") + policyholder.PolicyFromDate.Day.ToString("00");
                            detail.PolicyDetailInfo.TermExpirationDate = policyholder.PolicyToDate.Year.ToString() + policyholder.PolicyToDate.Month.ToString("00") + policyholder.PolicyToDate.Day.ToString("00");
                            if (policyholder.LastCancelDate != null)
                            {
                                detail.PolicyDetailInfo.CancellationDate = policyholder.LastCancelDate.Year.ToString() + policyholder.LastCancelDate.Month.ToString("00") + policyholder.LastCancelDate.Day.ToString("00");
                            }
                            _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfo = IsoPolicyInfo;
                            _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfo.Add(IsoSubjectInfoHistory);
                        }
                    }
                    //Summary Section Policy Search
                    ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2 IsoCovLapse = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2();
                    var policyInformation = PrefillResult.DriverDiscovery.CurrentCarrier.CurrentCarrierReport.Report.PolicyInformation;
                    var transactionDate = transactionDetailEx.TransactionDetails.DateTimeCompleted;

                    foreach (var policyholderList in PolInfo.PolicyHoldersList)
                    {
                        ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval IsoCovLapseInterval = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval();
                        ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo IsoPolicySearch = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo();
                        ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfoPolicyHolder1 IsoPolicyHolder1 = new ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfoPolicyHolder1();

                        if (policyholderList.PolicyHolderRelationship.ToString().ToUpper().SafeTrim() == "PRIMARY")
                        {
                            IsoPolicyHolder1.FirstName = policyholderList.PolicyHolderName.First;
                            IsoPolicyHolder1.LastName = policyholderList.PolicyHolderName.Last;
                            IsoPolicySearch.PolicyHolder1 = IsoPolicyHolder1;
                            IsoPolicySearch.ReportAsOfDate = transactionDate.Year.ToString() + transactionDate.Month.ToString("00") + transactionDate.Day.ToString("00");
                            IsoPolicySearch.PolicyNumber = PolInfo.PolicyNumber;
                            IsoPolicySearch.LastRptTermEffDate = policyholderList.PolicyFromDate.Year.ToString() + policyholderList.PolicyFromDate.Month.ToString("00") + policyholderList.PolicyFromDate.Day.ToString("00");
                            IsoPolicySearch.LastRptTermExpDate = policyholderList.PolicyToDate.Year.ToString() + policyholderList.PolicyToDate.Month.ToString("00") + policyholderList.PolicyToDate.Day.ToString("00");

                            //Summary Coverage Lapse
                            IsoCovLapse.FullName = policyholderList.PolicyHolderName.First + " " + policyholderList.PolicyHolderName.Last;
                            IsoCovLapseInterval.CoName = PolInfo.CarrierName;
                            IsoCovLapseInterval.AMBest = PolInfo.AmbestNumber;
                            IsoCovLapseInterval.FromDate = policyholderList.PolicyFromDate.Year.ToString() + "/" + policyholderList.PolicyFromDate.Month.ToString("00") + "/" + policyholderList.PolicyFromDate.Day.ToString("00");
                            IsoCovLapseInterval.ToDate = policyholderList.PolicyToDate.Year.ToString() + "/" + policyholderList.PolicyToDate.Month.ToString("00") + "/" + policyholderList.PolicyToDate.Day.ToString("00");

                            iSOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2 = IsoCovLapse;
                            iSOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval = IsoCovLapseInterval;
                            iSOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo = IsoPolicySearch;

                            //history section
                            ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfo IsoHistoryInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfo();
                            IsoHistoryInfo.PolicyType = PolInfo.PolicyType.ToString();
                            IsoHistoryInfo.FromDate = policyholderList.PolicyFromDate.Year.ToString() + policyholderList.PolicyFromDate.Month.ToString("00") + policyholderList.PolicyFromDate.Day.ToString("00");
                            IsoHistoryInfo.ToDate = policyholderList.PolicyToDate.Year.ToString() + policyholderList.PolicyToDate.Month.ToString("00") + policyholderList.PolicyToDate.Day.ToString("00");
                            IsoHistoryInfo.PolicyNumber = PolInfo.PolicyNumber;

                            if (PolInfo.RiskType == CCPolicyRiskType.Standard)
                            {
                                IsoHistoryInfo.RiskClassCodeDesc = "Standard";
                                IsoHistoryInfo.RiskClassCode = "S";
                            }
                            if (PolInfo.RiskType == CCPolicyRiskType.NonStandard)
                            {
                                IsoHistoryInfo.RiskClassCodeDesc = "NonStandard";
                                IsoHistoryInfo.RiskClassCode = "N";
                            }
                            if (PolInfo.RiskType == CCPolicyRiskType.Preferred)
                            {
                                IsoHistoryInfo.RiskClassCodeDesc = "Preferred";
                                IsoHistoryInfo.RiskClassCode = "S";
                            }
                            if (PolInfo.RiskType == CCPolicyRiskType.Assigned)
                            {
                                IsoHistoryInfo.RiskClassCodeDesc = "Assigned";
                                IsoHistoryInfo.RiskClassCode = "N";
                            }
                            if (PolInfo.RiskType == CCPolicyRiskType.Mixed)
                            {
                                IsoHistoryInfo.RiskClassCodeDesc = "Mixed";
                                IsoHistoryInfo.RiskClassCode = "N";
                            }
                            if (PolInfo.RiskType == CCPolicyRiskType.Facility)
                            {
                                IsoHistoryInfo.RiskClassCodeDesc = "Facility";
                                IsoHistoryInfo.RiskClassCode = "N";
                            }
                            var polType = PolInfo.PolicyType.ToString();
                            if (polType == "Automobile")
                            {
                                IsoHistoryInfo.PolicyType = "AU";
                                IsoHistoryInfo.PolicyTypeDesc = "Automobile";
                            }
                            else
                            {
                                IsoHistoryInfo.PolicyType = PolInfo.PolicyType.ToString();
                                IsoHistoryInfo.PolicyTypeDesc = PolInfo.PolicyType.ToString();
                            }
                            _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfo.Add(IsoHistoryInfo);

                            //Garage Section
                            ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfo IsoGarageInfo = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfo();
                            ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfoGarageAddress IsoGarageAddress = new ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfoGarageAddress();
                            IsoGarageAddress.State = PolInfo.PolicyStateCode;
                            IsoGarageInfo.GarageAddress = IsoGarageAddress;

                            _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfo.Add(IsoGarageInfo);
                        }
                    }
                    if (iSOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo != null)
                    {
                        _ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo.Add(iSOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo);
                    }
                    if (IsoCovLapse != null)
                    {
                        summary.CoverageLapse2 = IsoCovLapse;
                    }
                    if (iSOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval != null)
                    {
                        _ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval.Add(iSOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval);
                    }
                    if (_ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfo.Count > 0)
                    {
                        detail.GarageLocationInfo = _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfo.ToArray();
                    }
                    if (_ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo.Count > 0)
                    {
                        detail.SubjectInfo = _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo.ToArray();
                    }
                    if (_ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfo != null)
                    {
                        detail.PolicyDetailInfo = _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfo;
                    }
                    if (_ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCarrierInfo != null)
                    {
                        detail.CarrierInfo = _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCarrierInfo;
                    }
                    if (_ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfo.Count > 0)
                    {
                        history.SubjectInfo = _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfo.ToArray();
                    }
                    if (_ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfo.Count > 0)
                    {
                        history.PolicyHistoryInfo = _ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfo.ToArray();
                    }
                    iSOPassportSvcRsReportReportDataCoverageVerifierPolicy.HistorySection = history;
                    iSOPassportSvcRsReportReportDataCoverageVerifierPolicy.DetailSection = detail;
                    _ISOPassportSvcRsReportReportDataCoverageVerifierPolicy.Add(iSOPassportSvcRsReportReportDataCoverageVerifierPolicy);
                }
                if (_ISOPassportSvcRsReportReportDataCoverageVerifierPolicy.Count > 0)
                {
                    reportCV.ReportData.CoverageVerifier.Policy = _ISOPassportSvcRsReportReportDataCoverageVerifierPolicy.ToArray();
                }

                if (_ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo.Count > 0)
                {
                    summary.PolicySearchInfo = _ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo.ToArray();
                }
                //if (_ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2.Count > 0)
                //{
                //    summary.CoverageLapse2 = _ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2.ToArray();
                //}
                if (_ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval.Count > 0)
                {
                    summary.CoverageLapse2.CoverageInterval = _ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval.ToArray();
                }
                if (_ISOPassportSvcRsReportReportDataCoverageVerifierSummarySection != null)
                {
                    reportCV.ReportData.CoverageVerifier.SummarySection = summary;
                }
            }
            return reportCV;
        }
        #endregion

        #region "Map Polk Report"
        private ISOPassportSvcRsReport CreatePolkReport(AutoDataPrefillResult PrefillResult, TransactionDetailsEx transactionDetailEx)
        {
            ISOPassportSvcRsReport polkReport = new ISOPassportSvcRsReport();
            polkReport.Status = new ISOPassportSvcRsReportStatus();
            polkReport.ReportData = new ISOPassportSvcRsReportReportData();
            polkReport.ReportData.PolkResponse = new ISOPassportSvcRsReportReportDataPolkResponse();
            List<ISOPassportSvcRsReportReportDataPolkResponsePolkMatch> _ISOPassportSvcRsReportReportDataPolkResponsePolkMatch = new List<ISOPassportSvcRsReportReportDataPolkResponsePolkMatch>();
            polkReport.ProductCd = "LN_ADPF_POLK";
            polkReport.ReportName = "LN_ADPF_POLK";

            var currentCarrierReportStatus = PrefillResult.DriverDiscovery.CurrentCarrier.CurrentCarrierReport.Admin.Status;
            var VINStatus = PrefillResult.Summary.VehicleIdentificationReport.Status;
            var vehicleIdentificationReportStatusSpecified = PrefillResult.Summary.VehicleIdentificationReport.StatusSpecified;

            if (VINStatus == AutoDataPrefillVINStatus.Completed)
            {
                polkReport.Status.StatusDesc = "Completed";
            }
            if (VINStatus == AutoDataPrefillVINStatus.NoHit)
            {
                polkReport.Status.StatusDesc = "NoHit";
            }
            if (VINStatus == AutoDataPrefillVINStatus.OrderRejected)
            {
                polkReport.Status.StatusDesc = "OrderRejected";
            }
            if (VINStatus == AutoDataPrefillVINStatus.StateUnavailable)
            {
                polkReport.Status.StatusDesc = "StateUnavailable";
            }
            if (VINStatus == AutoDataPrefillVINStatus.Unreturned)
            {
                polkReport.Status.StatusDesc = "Unreturned";
            }
            var transactionDateOrdered = transactionDetailEx.TransactionDetails.DateTimeReceived;
            var transactionDateComplete = transactionDetailEx.TransactionDetails.DateTimeCompleted;
            polkReport.OrderTimeStamp = transactionDateOrdered.Year.ToString() + "/" + transactionDateOrdered.Month.ToString() + "/" + transactionDateOrdered.Day.ToString();
            polkReport.CompletedTimeStamp = transactionDateComplete.Year.ToString() + "/" + transactionDateComplete.Month.ToString() + "/" + transactionDateComplete.Day.ToString();

            if (PrefillResult.DriverDiscovery?.CurrentCarrier?.Attach?.DiscoveredVehicles != null)
            {
                foreach (var vehicle in PrefillResult.DriverDiscovery.CurrentCarrier.Attach.DiscoveredVehicles.Where(w => w.VehicleDetail.DecodedVin.Type != "Motorcycle").ToList())
                {
                    ISOPassportSvcRsReportReportDataPolkResponsePolkMatch PolkResponse = new ISOPassportSvcRsReportReportDataPolkResponsePolkMatch();
                    PolkResponse.VIN = vehicle.VehicleDetail.VIN;
                    PolkResponse.YearModel = vehicle.VehicleDetail.Year.ToString();
                    PolkResponse.Make = vehicle.VehicleDetail.Make;

                    _ISOPassportSvcRsReportReportDataPolkResponsePolkMatch.Add(PolkResponse);
                }
            }
            if (_ISOPassportSvcRsReportReportDataPolkResponsePolkMatch.Count > 0)
            {
                polkReport.ReportData.PolkResponse.Matches = _ISOPassportSvcRsReportReportDataPolkResponsePolkMatch.ToArray();
            }
            return polkReport;
        }
    }
    #endregion
}