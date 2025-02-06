namespace CommonAPICommon.Dto
{
    /// <remarks>
    /// Classes created using XSD 
    /// </remarks>

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ISO
    {

        private ISOPassportSvcRs passportSvcRsField;

        /// <remarks/>
        public ISOPassportSvcRs PassportSvcRs
        {
            get
            {
                return this.passportSvcRsField;
            }
            set
            {
                this.passportSvcRsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRs
    {

        private ISOPassportSvcRsStatus statusField;

        private ISOPassportSvcRsErrorMsgs errorMsgsField;

        private string sPNameField;

        private string requestIdField;

        private ISOPassportSvcRsReport[] reportsField;

        /// <remarks/>
        public ISOPassportSvcRsStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsErrorMsgs ErrorMsgs
        {
            get
            {
                return this.errorMsgsField;
            }
            set
            {
                this.errorMsgsField = value;
            }
        }

        /// <remarks/>
        public string SPName
        {
            get
            {
                return this.sPNameField;
            }
            set
            {
                this.sPNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string RequestId
        {
            get
            {
                return this.requestIdField;
            }
            set
            {
                this.requestIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Report", IsNullable = false)]
        public ISOPassportSvcRsReport[] Reports
        {
            get
            {
                return this.reportsField;
            }
            set
            {
                this.reportsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsStatus
    {

        private string statusCdField;

        private string statusDescField;

        /// <remarks/>
        public string StatusCd
        {
            get
            {
                return this.statusCdField;
            }
            set
            {
                this.statusCdField = value;
            }
        }

        /// <remarks/>
        public string StatusDesc
        {
            get
            {
                return this.statusDescField;
            }
            set
            {
                this.statusDescField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsErrorMsgs
    {

        private ISOPassportSvcRsErrorMsgsErrorMsg errorMsgField;

        /// <remarks/>
        public ISOPassportSvcRsErrorMsgsErrorMsg ErrorMsg
        {
            get
            {
                return this.errorMsgField;
            }
            set
            {
                this.errorMsgField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsErrorMsgsErrorMsg
    {

        private string errorCdField;

        private string errorDescField;

        /// <remarks/>
        public string ErrorCd
        {
            get
            {
                return this.errorCdField;
            }
            set
            {
                this.errorCdField = value;
            }
        }

        /// <remarks/>
        public string ErrorDesc
        {
            get
            {
                return this.errorDescField;
            }
            set
            {
                this.errorDescField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReport
    {

        private ISOPassportSvcRsReportStatus statusField;

        private string productCdField;

        private string reportNameField;

        private string criticalResultField;

        private string orderTimeStampField;

        private string completedTimeStampField;

        private ISOPassportSvcRsReportReportData reportDataField;

        /// <remarks/>
        public ISOPassportSvcRsReportStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string ProductCd
        {
            get
            {
                return this.productCdField;
            }
            set
            {
                this.productCdField = value;
            }
        }

        /// <remarks/>
        public string ReportName
        {
            get
            {
                return this.reportNameField;
            }
            set
            {
                this.reportNameField = value;
            }
        }

        /// <remarks/>
        public string CriticalResult
        {
            get
            {
                return this.criticalResultField;
            }
            set
            {
                this.criticalResultField = value;
            }
        }

        /// <remarks/>
        public string OrderTimeStamp
        {
            get
            {
                return this.orderTimeStampField;
            }
            set
            {
                this.orderTimeStampField = value;
            }
        }

        /// <remarks/>
        public string CompletedTimeStamp
        {
            get
            {
                return this.completedTimeStampField;
            }
            set
            {
                this.completedTimeStampField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportData ReportData
        {
            get
            {
                return this.reportDataField;
            }
            set
            {
                this.reportDataField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportStatus
    {

        private string statusCdField;

        private string statusDescField;

        /// <remarks/>
        public string StatusCd
        {
            get
            {
                return this.statusCdField;
            }
            set
            {
                this.statusCdField = value;
            }
        }

        /// <remarks/>
        public string StatusDesc
        {
            get
            {
                return this.statusDescField;
            }
            set
            {
                this.statusDescField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportData
    {

        private ISOPassportSvcRsReportReportDataPolkResponse polkResponseField;

        private ISOPassportSvcRsReportReportDataCoverageVerifier coverageVerifierField;

        private ISOPassportSvcRsReportReportDataACXIOM aCXIOMField;

        private ISOPassportSvcRsReportReportDataConditionalOrdering conditionalOrderingField;

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataPolkResponse PolkResponse
        {
            get
            {
                return this.polkResponseField;
            }
            set
            {
                this.polkResponseField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifier CoverageVerifier
        {
            get
            {
                return this.coverageVerifierField;
            }
            set
            {
                this.coverageVerifierField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataACXIOM ACXIOM
        {
            get
            {
                return this.aCXIOMField;
            }
            set
            {
                this.aCXIOMField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataConditionalOrdering ConditionalOrdering
        {
            get
            {
                return this.conditionalOrderingField;
            }
            set
            {
                this.conditionalOrderingField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataPolkResponse
    {

        private string quoteBackIdField;

        private string totalVehicleSetsField;

        private ISOPassportSvcRsReportReportDataPolkResponsePolkMatch[] matchesField;

        /// <remarks/>
        public string QuoteBackId
        {
            get
            {
                return this.quoteBackIdField;
            }
            set
            {
                this.quoteBackIdField = value;
            }
        }

        /// <remarks/>
        public string TotalVehicleSets
        {
            get
            {
                return this.totalVehicleSetsField;
            }
            set
            {
                this.totalVehicleSetsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PolkMatch", IsNullable = false)]
        public ISOPassportSvcRsReportReportDataPolkResponsePolkMatch[] Matches
        {
            get
            {
                return this.matchesField;
            }
            set
            {
                this.matchesField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataPolkResponsePolkMatch
    {

        private string restrictedStateIndicatorField;

        private string matchSameIndividualField;

        private string surnameMatchField;

        private string addressMatchField;

        private string vinCompareIndField;

        private string vinCompareDescField;

        private string processingTypeCdField;

        private string vinIndicatorField;

        private string vehicleSeqNumField;

        private string vINField;

        private string yearModelField;

        private string makeField;

        private string bodyStyleCdField;

        private string bodyStyleDescField;

        private string seriesCdField;

        private string registrationStateCdField;

        private string registrationStateDescField;

        private string transactionDateField;

        private string expirationDateField;

        private string activityDateField;

        private string plateTypeCdField;

        private string plateTypeDescField;

        private string lesseeLessorCdField;

        private string lesseeLessorDescField;

        private string brandedIndField;

        private string documentTypeCdField;

        private string documentTypeDescField;

        private string vinChangeIndField;

        private object vinChangeDescField;

        private string cityField;

        private string stateCdField;

        private string zipCdField;

        private string zipPlus4CdField;

        private string addressTypeCdField;

        private string addressTypeDescField;

        private string houseNumberField;

        private object houseNumberFractionField;

        private object streetPrefixDirectionField;

        private string streetNameField;

        private string streetSuffixAbreviationField;

        private object streetSuffixDirectionField;

        private object unitDesignatorOneField;

        private object unitNumberOrQualifierOneField;

        private string nameTitleCd1Field;

        private string nameTitleDesc1Field;

        private string surname1Field;

        private string firstName1Field;

        private string middleInitial1Field;

        private object nameSuffix1Field;

        private string nameTitleCd2Field;

        private string nameTitleDesc2Field;

        private string nameCd2Field;

        private string nameDesc2Field;

        private string surname2Field;

        private string firstName2Field;

        private string middleInitial2Field;

        private object nameSuffix2Field;

        /// <remarks/>
        public string RestrictedStateIndicator
        {
            get
            {
                return this.restrictedStateIndicatorField;
            }
            set
            {
                this.restrictedStateIndicatorField = value;
            }
        }

        /// <remarks/>
        public string MatchSameIndividual
        {
            get
            {
                return this.matchSameIndividualField;
            }
            set
            {
                this.matchSameIndividualField = value;
            }
        }

        /// <remarks/>
        public string SurnameMatch
        {
            get
            {
                return this.surnameMatchField;
            }
            set
            {
                this.surnameMatchField = value;
            }
        }

        /// <remarks/>
        public string AddressMatch
        {
            get
            {
                return this.addressMatchField;
            }
            set
            {
                this.addressMatchField = value;
            }
        }

        /// <remarks/>
        public string VinCompareInd
        {
            get
            {
                return this.vinCompareIndField;
            }
            set
            {
                this.vinCompareIndField = value;
            }
        }

        /// <remarks/>
        public string VinCompareDesc
        {
            get
            {
                return this.vinCompareDescField;
            }
            set
            {
                this.vinCompareDescField = value;
            }
        }

        /// <remarks/>
        public string ProcessingTypeCd
        {
            get
            {
                return this.processingTypeCdField;
            }
            set
            {
                this.processingTypeCdField = value;
            }
        }

        /// <remarks/>
        public string VinIndicator
        {
            get
            {
                return this.vinIndicatorField;
            }
            set
            {
                this.vinIndicatorField = value;
            }
        }

        /// <remarks/>
        public string VehicleSeqNum
        {
            get
            {
                return this.vehicleSeqNumField;
            }
            set
            {
                this.vehicleSeqNumField = value;
            }
        }

        /// <remarks/>
        public string VIN
        {
            get
            {
                return this.vINField;
            }
            set
            {
                this.vINField = value;
            }
        }

        /// <remarks/>
        public string YearModel
        {
            get
            {
                return this.yearModelField;
            }
            set
            {
                this.yearModelField = value;
            }
        }

        /// <remarks/>
        public string Make
        {
            get
            {
                return this.makeField;
            }
            set
            {
                this.makeField = value;
            }
        }

        /// <remarks/>
        public string BodyStyleCd
        {
            get
            {
                return this.bodyStyleCdField;
            }
            set
            {
                this.bodyStyleCdField = value;
            }
        }

        /// <remarks/>
        public string BodyStyleDesc
        {
            get
            {
                return this.bodyStyleDescField;
            }
            set
            {
                this.bodyStyleDescField = value;
            }
        }

        /// <remarks/>
        public string SeriesCd
        {
            get
            {
                return this.seriesCdField;
            }
            set
            {
                this.seriesCdField = value;
            }
        }

        /// <remarks/>
        public string RegistrationStateCd
        {
            get
            {
                return this.registrationStateCdField;
            }
            set
            {
                this.registrationStateCdField = value;
            }
        }

        /// <remarks/>
        public string RegistrationStateDesc
        {
            get
            {
                return this.registrationStateDescField;
            }
            set
            {
                this.registrationStateDescField = value;
            }
        }

        /// <remarks/>
        public string TransactionDate
        {
            get
            {
                return this.transactionDateField;
            }
            set
            {
                this.transactionDateField = value;
            }
        }

        /// <remarks/>
        public string ExpirationDate
        {
            get
            {
                return this.expirationDateField;
            }
            set
            {
                this.expirationDateField = value;
            }
        }

        /// <remarks/>
        public string ActivityDate
        {
            get
            {
                return this.activityDateField;
            }
            set
            {
                this.activityDateField = value;
            }
        }

        /// <remarks/>
        public string PlateTypeCd
        {
            get
            {
                return this.plateTypeCdField;
            }
            set
            {
                this.plateTypeCdField = value;
            }
        }

        /// <remarks/>
        public string PlateTypeDesc
        {
            get
            {
                return this.plateTypeDescField;
            }
            set
            {
                this.plateTypeDescField = value;
            }
        }

        /// <remarks/>
        public string LesseeLessorCd
        {
            get
            {
                return this.lesseeLessorCdField;
            }
            set
            {
                this.lesseeLessorCdField = value;
            }
        }

        /// <remarks/>
        public string LesseeLessorDesc
        {
            get
            {
                return this.lesseeLessorDescField;
            }
            set
            {
                this.lesseeLessorDescField = value;
            }
        }

        /// <remarks/>
        public string BrandedInd
        {
            get
            {
                return this.brandedIndField;
            }
            set
            {
                this.brandedIndField = value;
            }
        }

        /// <remarks/>
        public string DocumentTypeCd
        {
            get
            {
                return this.documentTypeCdField;
            }
            set
            {
                this.documentTypeCdField = value;
            }
        }

        /// <remarks/>
        public string DocumentTypeDesc
        {
            get
            {
                return this.documentTypeDescField;
            }
            set
            {
                this.documentTypeDescField = value;
            }
        }

        /// <remarks/>
        public string VinChangeInd
        {
            get
            {
                return this.vinChangeIndField;
            }
            set
            {
                this.vinChangeIndField = value;
            }
        }

        /// <remarks/>
        public object VinChangeDesc
        {
            get
            {
                return this.vinChangeDescField;
            }
            set
            {
                this.vinChangeDescField = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string StateCd
        {
            get
            {
                return this.stateCdField;
            }
            set
            {
                this.stateCdField = value;
            }
        }

        /// <remarks/>
        public string ZipCd
        {
            get
            {
                return this.zipCdField;
            }
            set
            {
                this.zipCdField = value;
            }
        }

        /// <remarks/>
        public string ZipPlus4Cd
        {
            get
            {
                return this.zipPlus4CdField;
            }
            set
            {
                this.zipPlus4CdField = value;
            }
        }

        /// <remarks/>
        public string AddressTypeCd
        {
            get
            {
                return this.addressTypeCdField;
            }
            set
            {
                this.addressTypeCdField = value;
            }
        }

        /// <remarks/>
        public string AddressTypeDesc
        {
            get
            {
                return this.addressTypeDescField;
            }
            set
            {
                this.addressTypeDescField = value;
            }
        }

        /// <remarks/>
        public string HouseNumber
        {
            get
            {
                return this.houseNumberField;
            }
            set
            {
                this.houseNumberField = value;
            }
        }

        /// <remarks/>
        public object HouseNumberFraction
        {
            get
            {
                return this.houseNumberFractionField;
            }
            set
            {
                this.houseNumberFractionField = value;
            }
        }

        /// <remarks/>
        public object StreetPrefixDirection
        {
            get
            {
                return this.streetPrefixDirectionField;
            }
            set
            {
                this.streetPrefixDirectionField = value;
            }
        }

        /// <remarks/>
        public string StreetName
        {
            get
            {
                return this.streetNameField;
            }
            set
            {
                this.streetNameField = value;
            }
        }

        /// <remarks/>
        public string StreetSuffixAbreviation
        {
            get
            {
                return this.streetSuffixAbreviationField;
            }
            set
            {
                this.streetSuffixAbreviationField = value;
            }
        }

        /// <remarks/>
        public object StreetSuffixDirection
        {
            get
            {
                return this.streetSuffixDirectionField;
            }
            set
            {
                this.streetSuffixDirectionField = value;
            }
        }

        /// <remarks/>
        public object UnitDesignatorOne
        {
            get
            {
                return this.unitDesignatorOneField;
            }
            set
            {
                this.unitDesignatorOneField = value;
            }
        }

        /// <remarks/>
        public object UnitNumberOrQualifierOne
        {
            get
            {
                return this.unitNumberOrQualifierOneField;
            }
            set
            {
                this.unitNumberOrQualifierOneField = value;
            }
        }

        /// <remarks/>
        public string NameTitleCd1
        {
            get
            {
                return this.nameTitleCd1Field;
            }
            set
            {
                this.nameTitleCd1Field = value;
            }
        }

        /// <remarks/>
        public string NameTitleDesc1
        {
            get
            {
                return this.nameTitleDesc1Field;
            }
            set
            {
                this.nameTitleDesc1Field = value;
            }
        }

        /// <remarks/>
        public string Surname1
        {
            get
            {
                return this.surname1Field;
            }
            set
            {
                this.surname1Field = value;
            }
        }

        /// <remarks/>
        public string FirstName1
        {
            get
            {
                return this.firstName1Field;
            }
            set
            {
                this.firstName1Field = value;
            }
        }

        /// <remarks/>
        public string MiddleInitial1
        {
            get
            {
                return this.middleInitial1Field;
            }
            set
            {
                this.middleInitial1Field = value;
            }
        }

        /// <remarks/>
        public object NameSuffix1
        {
            get
            {
                return this.nameSuffix1Field;
            }
            set
            {
                this.nameSuffix1Field = value;
            }
        }

        /// <remarks/>
        public string NameTitleCd2
        {
            get
            {
                return this.nameTitleCd2Field;
            }
            set
            {
                this.nameTitleCd2Field = value;
            }
        }

        /// <remarks/>
        public string NameTitleDesc2
        {
            get
            {
                return this.nameTitleDesc2Field;
            }
            set
            {
                this.nameTitleDesc2Field = value;
            }
        }

        /// <remarks/>
        public string NameCd2
        {
            get
            {
                return this.nameCd2Field;
            }
            set
            {
                this.nameCd2Field = value;
            }
        }

        /// <remarks/>
        public string NameDesc2
        {
            get
            {
                return this.nameDesc2Field;
            }
            set
            {
                this.nameDesc2Field = value;
            }
        }

        /// <remarks/>
        public string Surname2
        {
            get
            {
                return this.surname2Field;
            }
            set
            {
                this.surname2Field = value;
            }
        }

        /// <remarks/>
        public string FirstName2
        {
            get
            {
                return this.firstName2Field;
            }
            set
            {
                this.firstName2Field = value;
            }
        }

        /// <remarks/>
        public string MiddleInitial2
        {
            get
            {
                return this.middleInitial2Field;
            }
            set
            {
                this.middleInitial2Field = value;
            }
        }

        /// <remarks/>
        public object NameSuffix2
        {
            get
            {
                return this.nameSuffix2Field;
            }
            set
            {
                this.nameSuffix2Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifier
    {

        private ISOPassportSvcRsReportReportDataCoverageVerifierStaticInfo staticInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierHeaderInfo headerInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierSummarySection summarySectionField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicy[] policyField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierTrailerInfo trailerInfoField;

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierStaticInfo StaticInfo
        {
            get
            {
                return this.staticInfoField;
            }
            set
            {
                this.staticInfoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierHeaderInfo HeaderInfo
        {
            get
            {
                return this.headerInfoField;
            }
            set
            {
                this.headerInfoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierSummarySection SummarySection
        {
            get
            {
                return this.summarySectionField;
            }
            set
            {
                this.summarySectionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Policy")]
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicy[] Policy
        {
            get
            {
                return this.policyField;
            }
            set
            {
                this.policyField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierTrailerInfo TrailerInfo
        {
            get
            {
                return this.trailerInfoField;
            }
            set
            {
                this.trailerInfoField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierStaticInfo
    {

        private string productIdField;

        private string customerTypeField;

        private string organisingCompanyField;

        private string shippingCompanyField;

        private string productCodeField;

        private string subscriberIdField;

        private string userIdField;

        private string sequenceOrderNumberField;

        /// <remarks/>
        public string ProductId
        {
            get
            {
                return this.productIdField;
            }
            set
            {
                this.productIdField = value;
            }
        }

        /// <remarks/>
        public string CustomerType
        {
            get
            {
                return this.customerTypeField;
            }
            set
            {
                this.customerTypeField = value;
            }
        }

        /// <remarks/>
        public string OrganisingCompany
        {
            get
            {
                return this.organisingCompanyField;
            }
            set
            {
                this.organisingCompanyField = value;
            }
        }

        /// <remarks/>
        public string ShippingCompany
        {
            get
            {
                return this.shippingCompanyField;
            }
            set
            {
                this.shippingCompanyField = value;
            }
        }

        /// <remarks/>
        public string ProductCode
        {
            get
            {
                return this.productCodeField;
            }
            set
            {
                this.productCodeField = value;
            }
        }

        /// <remarks/>
        public string SubscriberId
        {
            get
            {
                return this.subscriberIdField;
            }
            set
            {
                this.subscriberIdField = value;
            }
        }

        /// <remarks/>
        public string UserId
        {
            get
            {
                return this.userIdField;
            }
            set
            {
                this.userIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string SequenceOrderNumber
        {
            get
            {
                return this.sequenceOrderNumberField;
            }
            set
            {
                this.sequenceOrderNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierHeaderInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string customerCodeField;

        private string transmissionDateField;

        private string transmissionTimeField;

        private string policyQuantityField;

        private string returnCodeField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string CustomerCode
        {
            get
            {
                return this.customerCodeField;
            }
            set
            {
                this.customerCodeField = value;
            }
        }

        /// <remarks/>
        public string TransmissionDate
        {
            get
            {
                return this.transmissionDateField;
            }
            set
            {
                this.transmissionDateField = value;
            }
        }

        /// <remarks/>
        public string TransmissionTime
        {
            get
            {
                return this.transmissionTimeField;
            }
            set
            {
                this.transmissionTimeField = value;
            }
        }

        /// <remarks/>
        public string PolicyQuantity
        {
            get
            {
                return this.policyQuantityField;
            }
            set
            {
                this.policyQuantityField = value;
            }
        }

        /// <remarks/>
        public string ReturnCode
        {
            get
            {
                return this.returnCodeField;
            }
            set
            {
                this.returnCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierSummarySection
    {

        private ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionRequestLevelInfo requestLevelInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCustomerInfo customerInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo[] policySearchInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse1 coverageLapse1Field;

        private ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2 coverageLapse2Field;

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionRequestLevelInfo RequestLevelInfo
        {
            get
            {
                return this.requestLevelInfoField;
            }
            set
            {
                this.requestLevelInfoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCustomerInfo CustomerInfo
        {
            get
            {
                return this.customerInfoField;
            }
            set
            {
                this.customerInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PolicySearchInfo")]
        public ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo[] PolicySearchInfo
        {
            get
            {
                return this.policySearchInfoField;
            }
            set
            {
                this.policySearchInfoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse1 CoverageLapse1
        {
            get
            {
                return this.coverageLapse1Field;
            }
            set
            {
                this.coverageLapse1Field = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2 CoverageLapse2
        {
            get
            {
                return this.coverageLapse2Field;
            }
            set
            {
                this.coverageLapse2Field = value;
            }
        }

    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionRequestLevelInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string reportIdField;

        private string lOBField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string ReportId
        {
            get
            {
                return this.reportIdField;
            }
            set
            {
                this.reportIdField = value;
            }
        }

        /// <remarks/>
        public string LOB
        {
            get
            {
                return this.lOBField;
            }
            set
            {
                this.lOBField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCustomerInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string subscriberIDField;

        private string sponsoringCompanyField;

        private string orderingCompanyField;

        private string userIDField;

        private string productCodeField;

        private string lookBackField;

        private string coveragePTPSWField;

        private string financePTPSWField;

        private string allPTPSWField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string SubscriberID
        {
            get
            {
                return this.subscriberIDField;
            }
            set
            {
                this.subscriberIDField = value;
            }
        }

        /// <remarks/>
        public string SponsoringCompany
        {
            get
            {
                return this.sponsoringCompanyField;
            }
            set
            {
                this.sponsoringCompanyField = value;
            }
        }

        /// <remarks/>
        public string OrderingCompany
        {
            get
            {
                return this.orderingCompanyField;
            }
            set
            {
                this.orderingCompanyField = value;
            }
        }

        /// <remarks/>
        public string UserID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
            }
        }

        /// <remarks/>
        public string ProductCode
        {
            get
            {
                return this.productCodeField;
            }
            set
            {
                this.productCodeField = value;
            }
        }

        /// <remarks/>
        public string LookBack
        {
            get
            {
                return this.lookBackField;
            }
            set
            {
                this.lookBackField = value;
            }
        }

        /// <remarks/>
        public string CoveragePTPSW
        {
            get
            {
                return this.coveragePTPSWField;
            }
            set
            {
                this.coveragePTPSWField = value;
            }
        }

        /// <remarks/>
        public string FinancePTPSW
        {
            get
            {
                return this.financePTPSWField;
            }
            set
            {
                this.financePTPSWField = value;
            }
        }

        /// <remarks/>
        public string AllPTPSW
        {
            get
            {
                return this.allPTPSWField;
            }
            set
            {
                this.allPTPSWField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string searchTypeField;

        private string policyNumberField;

        private string carrierNameField;

        private string policyStatusField;

        private string numOfCancellationsField;

        private string numOfRenewalsField;

        private string reportAsOfDateField;

        private string inceptionDateField;

        private string lastRptTermEffDateField;

        private string lastRptTermExpDateField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfoPolicyHolder1 policyHolder1Field;

        private object policyHolder2Field;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string SearchType
        {
            get
            {
                return this.searchTypeField;
            }
            set
            {
                this.searchTypeField = value;
            }
        }

        /// <remarks/>
        public string PolicyNumber
        {
            get
            {
                return this.policyNumberField;
            }
            set
            {
                this.policyNumberField = value;
            }
        }

        /// <remarks/>
        public string CarrierName
        {
            get
            {
                return this.carrierNameField;
            }
            set
            {
                this.carrierNameField = value;
            }
        }

        /// <remarks/>
        public string PolicyStatus
        {
            get
            {
                return this.policyStatusField;
            }
            set
            {
                this.policyStatusField = value;
            }
        }

        /// <remarks/>
        public string NumOfCancellations
        {
            get
            {
                return this.numOfCancellationsField;
            }
            set
            {
                this.numOfCancellationsField = value;
            }
        }

        /// <remarks/>
        public string NumOfRenewals
        {
            get
            {
                return this.numOfRenewalsField;
            }
            set
            {
                this.numOfRenewalsField = value;
            }
        }

        /// <remarks/>
        public string ReportAsOfDate
        {
            get
            {
                return this.reportAsOfDateField;
            }
            set
            {
                this.reportAsOfDateField = value;
            }
        }

        /// <remarks/>
        public string InceptionDate
        {
            get
            {
                return this.inceptionDateField;
            }
            set
            {
                this.inceptionDateField = value;
            }
        }

        /// <remarks/>
        public string LastRptTermEffDate
        {
            get
            {
                return this.lastRptTermEffDateField;
            }
            set
            {
                this.lastRptTermEffDateField = value;
            }
        }

        /// <remarks/>
        public string LastRptTermExpDate
        {
            get
            {
                return this.lastRptTermExpDateField;
            }
            set
            {
                this.lastRptTermExpDateField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfoPolicyHolder1 PolicyHolder1
        {
            get
            {
                return this.policyHolder1Field;
            }
            set
            {
                this.policyHolder1Field = value;
            }
        }

        /// <remarks/>
        public object PolicyHolder2
        {
            get
            {
                return this.policyHolder2Field;
            }
            set
            {
                this.policyHolder2Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionPolicySearchInfoPolicyHolder1
    {

        private string firstNameField;

        private string middleNameField;

        private string lastNameField;

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse1
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string searchRequestNumberField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse1PolicyHolder policyHolderField;

        private string possibleLapseSwitchField;

        private string currentInforceCoverageField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string SearchRequestNumber
        {
            get
            {
                return this.searchRequestNumberField;
            }
            set
            {
                this.searchRequestNumberField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse1PolicyHolder PolicyHolder
        {
            get
            {
                return this.policyHolderField;
            }
            set
            {
                this.policyHolderField = value;
            }
        }

        /// <remarks/>
        public string PossibleLapseSwitch
        {
            get
            {
                return this.possibleLapseSwitchField;
            }
            set
            {
                this.possibleLapseSwitchField = value;
            }
        }

        /// <remarks/>
        public string CurrentInforceCoverage
        {
            get
            {
                return this.currentInforceCoverageField;
            }
            set
            {
                this.currentInforceCoverageField = value;
            }
        }
    }




    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse1PolicyHolder
    {

        private string firstNameField;

        private string lastNameField;

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string searchRequestNumberField;

        private string fullNameField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval[] coverageIntervalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SearchRequestNumber
        {
            get
            {
                return this.searchRequestNumberField;
            }
            set
            {
                this.searchRequestNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FullName
        {
            get
            {
                return this.fullNameField;
            }
            set
            {
                this.fullNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CoverageInterval", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval[] CoverageInterval
        {
            get
            {
                return this.coverageIntervalField;
            }
            set
            {
                this.coverageIntervalField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.18020")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierSummarySectionCoverageLapse2CoverageInterval
    {

        private string coNameField;

        private string aMBestField;

        private string fromDateField;

        private string toDateField;

        private string coverageDaysField;

        private string breakFromPriorField;

        private string lapsedDaysField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CoName
        {
            get
            {
                return this.coNameField;
            }
            set
            {
                this.coNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string AMBest
        {
            get
            {
                return this.aMBestField;
            }
            set
            {
                this.aMBestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CoverageDays
        {
            get
            {
                return this.coverageDaysField;
            }
            set
            {
                this.coverageDaysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string BreakFromPrior
        {
            get
            {
                return this.breakFromPriorField;
            }
            set
            {
                this.breakFromPriorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LapsedDays
        {
            get
            {
                return this.lapsedDaysField;
            }
            set
            {
                this.lapsedDaysField = value;
            }
        }
    }


    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicy
    {

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSection detailSectionField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySection historySectionField;

        private string numberField;

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSection DetailSection
        {
            get
            {
                return this.detailSectionField;
            }
            set
            {
                this.detailSectionField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySection HistorySection
        {
            get
            {
                return this.historySectionField;
            }
            set
            {
                this.historySectionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSection
    {

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCarrierInfo carrierInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfo policyDetailInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionMailingAddressInfo mailingAddressInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfo coverageInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionVehicleInfo[] vehicleInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfo[] garageLocationInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo[] subjectInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionFinanceInfo financeInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionMatchBasisInfo matchBasisInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionDisputeStatementInfo disputeStatementInfoField;

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCarrierInfo CarrierInfo
        {
            get
            {
                return this.carrierInfoField;
            }
            set
            {
                this.carrierInfoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfo PolicyDetailInfo
        {
            get
            {
                return this.policyDetailInfoField;
            }
            set
            {
                this.policyDetailInfoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionMailingAddressInfo MailingAddressInfo
        {
            get
            {
                return this.mailingAddressInfoField;
            }
            set
            {
                this.mailingAddressInfoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfo CoverageInfo
        {
            get
            {
                return this.coverageInfoField;
            }
            set
            {
                this.coverageInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("VehicleInfo")]
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionVehicleInfo[] VehicleInfo
        {
            get
            {
                return this.vehicleInfoField;
            }
            set
            {
                this.vehicleInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GarageLocationInfo")]
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfo[] GarageLocationInfo
        {
            get
            {
                return this.garageLocationInfoField;
            }
            set
            {
                this.garageLocationInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SubjectInfo")]
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo[] SubjectInfo
        {
            get
            {
                return this.subjectInfoField;
            }
            set
            {
                this.subjectInfoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionFinanceInfo FinanceInfo
        {
            get
            {
                return this.financeInfoField;
            }
            set
            {
                this.financeInfoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionMatchBasisInfo MatchBasisInfo
        {
            get
            {
                return this.matchBasisInfoField;
            }
            set
            {
                this.matchBasisInfoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionDisputeStatementInfo DisputeStatementInfo
        {
            get
            {
                return this.disputeStatementInfoField;
            }
            set
            {
                this.disputeStatementInfoField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCarrierInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string aMBestField;

        private string carrierNameField;

        private string nAICCodeField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string AMBest
        {
            get
            {
                return this.aMBestField;
            }
            set
            {
                this.aMBestField = value;
            }
        }

        /// <remarks/>
        public string CarrierName
        {
            get
            {
                return this.carrierNameField;
            }
            set
            {
                this.carrierNameField = value;
            }
        }

        /// <remarks/>
        public string NAICCode
        {
            get
            {
                return this.nAICCodeField;
            }
            set
            {
                this.nAICCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string policyNumberField;

        private string inceptionDateField;

        private string policyTypeField;

        private string policyTypeDescField;

        private string termEffectiveDateField;

        private string termExpirationDateField;

        private string cancellationDateField;

        private string cancellationReasonField;

        private string cancellationReasonDescField;

        private string homePhoneField;

        private string businessPhoneField;

        private string businessExtensionField;

        private string cellPhoneField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfoPolicyHolder1 policyHolder1Field;

        private object policyHolder2Field;

        private string policyStatusField;

        private string riskClassCodeField;

        private string riskClassCodeDescField;

        private string policyPremiumField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string PolicyNumber
        {
            get
            {
                return this.policyNumberField;
            }
            set
            {
                this.policyNumberField = value;
            }
        }

        /// <remarks/>
        public string InceptionDate
        {
            get
            {
                return this.inceptionDateField;
            }
            set
            {
                this.inceptionDateField = value;
            }
        }

        /// <remarks/>
        public string PolicyType
        {
            get
            {
                return this.policyTypeField;
            }
            set
            {
                this.policyTypeField = value;
            }
        }

        /// <remarks/>
        public string PolicyTypeDesc
        {
            get
            {
                return this.policyTypeDescField;
            }
            set
            {
                this.policyTypeDescField = value;
            }
        }

        /// <remarks/>
        public string TermEffectiveDate
        {
            get
            {
                return this.termEffectiveDateField;
            }
            set
            {
                this.termEffectiveDateField = value;
            }
        }

        /// <remarks/>
        public string TermExpirationDate
        {
            get
            {
                return this.termExpirationDateField;
            }
            set
            {
                this.termExpirationDateField = value;
            }
        }

        /// <remarks/>
        public string CancellationDate
        {
            get
            {
                return this.cancellationDateField;
            }
            set
            {
                this.cancellationDateField = value;
            }
        }

        /// <remarks/>
        public string CancellationReason
        {
            get
            {
                return this.cancellationReasonField;
            }
            set
            {
                this.cancellationReasonField = value;
            }
        }

        /// <remarks/>
        public string CancellationReasonDesc
        {
            get
            {
                return this.cancellationReasonDescField;
            }
            set
            {
                this.cancellationReasonDescField = value;
            }
        }

        /// <remarks/>
        public string HomePhone
        {
            get
            {
                return this.homePhoneField;
            }
            set
            {
                this.homePhoneField = value;
            }
        }

        /// <remarks/>
        public string BusinessPhone
        {
            get
            {
                return this.businessPhoneField;
            }
            set
            {
                this.businessPhoneField = value;
            }
        }

        /// <remarks/>
        public string BusinessExtension
        {
            get
            {
                return this.businessExtensionField;
            }
            set
            {
                this.businessExtensionField = value;
            }
        }

        /// <remarks/>
        public string CellPhone
        {
            get
            {
                return this.cellPhoneField;
            }
            set
            {
                this.cellPhoneField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfoPolicyHolder1 PolicyHolder1
        {
            get
            {
                return this.policyHolder1Field;
            }
            set
            {
                this.policyHolder1Field = value;
            }
        }

        /// <remarks/>
        public object PolicyHolder2
        {
            get
            {
                return this.policyHolder2Field;
            }
            set
            {
                this.policyHolder2Field = value;
            }
        }

        /// <remarks/>
        public string PolicyStatus
        {
            get
            {
                return this.policyStatusField;
            }
            set
            {
                this.policyStatusField = value;
            }
        }

        /// <remarks/>
        public string RiskClassCode
        {
            get
            {
                return this.riskClassCodeField;
            }
            set
            {
                this.riskClassCodeField = value;
            }
        }

        /// <remarks/>
        public string RiskClassCodeDesc
        {
            get
            {
                return this.riskClassCodeDescField;
            }
            set
            {
                this.riskClassCodeDescField = value;
            }
        }

        /// <remarks/>
        public string PolicyPremium
        {
            get
            {
                return this.policyPremiumField;
            }
            set
            {
                this.policyPremiumField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionPolicyDetailInfoPolicyHolder1
    {

        private string firstNameField;

        private string middleNameField;

        private string lastNameField;

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionMailingAddressInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionMailingAddressInfoMailingAddress mailingAddressField;

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionMailingAddressInfoMailingAddress MailingAddress
        {
            get
            {
                return this.mailingAddressField;
            }
            set
            {
                this.mailingAddressField = value;
            }
        }

        /// <remarks/>
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionMailingAddressInfoMailingAddress
    {

        private string addressLine1Field;

        private string addressLine2Field;

        private bool addressLine2FieldSpecified;

        private string cityField;

        private string stateField;

        private string zipField;

        /// <remarks/>
        public string AddressLine1
        {
            get
            {
                return this.addressLine1Field;
            }
            set
            {
                this.addressLine1Field = value;
            }
        }

        /// <remarks/>
        public string AddressLine2
        {
            get
            {
                return this.addressLine2Field;
            }
            set
            {
                this.addressLine2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AddressLine2Specified
        {
            get
            {
                return this.addressLine2FieldSpecified;
            }
            set
            {
                this.addressLine2FieldSpecified = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public string Zip
        {
            get
            {
                return this.zipField;
            }
            set
            {
                this.zipField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private object propertyAddressField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo[] addressCoverageInfoField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public object PropertyAddress
        {
            get
            {
                return this.propertyAddressField;
            }
            set
            {
                this.propertyAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AddressCoverageInfo")]
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo[] AddressCoverageInfo
        {
            get
            {
                return this.addressCoverageInfoField;
            }
            set
            {
                this.addressCoverageInfoField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionCoverageInfoAddressCoverageInfo
    {

        private string coverageCodeField;

        private string coverageCodeDescField;

        private string individualLimitField;

        private string occuranceLimitField;

        private string cSILimitField;

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        public string CoverageCode
        {
            get
            {
                return this.coverageCodeField;
            }
            set
            {
                this.coverageCodeField = value;
            }
        }

        /// <remarks/>
        public string CoverageCodeDesc
        {
            get
            {
                return this.coverageCodeDescField;
            }
            set
            {
                this.coverageCodeDescField = value;
            }
        }

        /// <remarks/>
        public string IndividualLimit
        {
            get
            {
                return this.individualLimitField;
            }
            set
            {
                this.individualLimitField = value;
            }
        }

        /// <remarks/>
        public string OccuranceLimit
        {
            get
            {
                return this.occuranceLimitField;
            }
            set
            {
                this.occuranceLimitField = value;
            }
        }

        /// <remarks/>
        public string CSILimit
        {
            get
            {
                return this.cSILimitField;
            }
            set
            {
                this.cSILimitField = value;
            }
        }

        /// <remarks/>
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionVehicleInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string vehicleNoField;

        private string yearField;

        private string makeField;

        private string modelField;

        private string vINField;

        private string classCodeField;

        private string collDeductibleField;

        private bool collDeductibleFieldSpecified;

        private string compDeductibleField;

        private bool compDeductibleFieldSpecified;

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string VehicleNo
        {
            get
            {
                return this.vehicleNoField;
            }
            set
            {
                this.vehicleNoField = value;
            }
        }

        /// <remarks/>
        public string Year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
            }
        }

        /// <remarks/>
        public string Make
        {
            get
            {
                return this.makeField;
            }
            set
            {
                this.makeField = value;
            }
        }

        /// <remarks/>
        public string Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        public string VIN
        {
            get
            {
                return this.vINField;
            }
            set
            {
                this.vINField = value;
            }
        }

        /// <remarks/>
        public string ClassCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        public string CollDeductible
        {
            get
            {
                return this.collDeductibleField;
            }
            set
            {
                this.collDeductibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CollDeductibleSpecified
        {
            get
            {
                return this.collDeductibleFieldSpecified;
            }
            set
            {
                this.collDeductibleFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string CompDeductible
        {
            get
            {
                return this.compDeductibleField;
            }
            set
            {
                this.compDeductibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CompDeductibleSpecified
        {
            get
            {
                return this.compDeductibleFieldSpecified;
            }
            set
            {
                this.compDeductibleFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string vehicleNoField;

        private string vINField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfoGarageAddress garageAddressField;

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string VehicleNo
        {
            get
            {
                return this.vehicleNoField;
            }
            set
            {
                this.vehicleNoField = value;
            }
        }

        /// <remarks/>
        public string VIN
        {
            get
            {
                return this.vINField;
            }
            set
            {
                this.vINField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfoGarageAddress GarageAddress
        {
            get
            {
                return this.garageAddressField;
            }
            set
            {
                this.garageAddressField = value;
            }
        }

        /// <remarks/>
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionGarageLocationInfoGarageAddress
    {

        private string addressLine1Field;

        private string addressLine2Field;

        private bool addressLine2FieldSpecified;

        private string cityField;

        private string stateField;

        private string zipField;

        /// <remarks/>
        public string AddressLine1
        {
            get
            {
                return this.addressLine1Field;
            }
            set
            {
                this.addressLine1Field = value;
            }
        }

        /// <remarks/>
        public string AddressLine2
        {
            get
            {
                return this.addressLine2Field;
            }
            set
            {
                this.addressLine2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AddressLine2Specified
        {
            get
            {
                return this.addressLine2FieldSpecified;
            }
            set
            {
                this.addressLine2FieldSpecified = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public string Zip
        {
            get
            {
                return this.zipField;
            }
            set
            {
                this.zipField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string subjectNoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfoSubjectName subjectNameField;

        private string dateOfBirthField;

        private string sSNField;

        private string genderField;

        private string relationCodeField;

        private string relationCodeDescField;

        private string dLNumberField;

        private string dLStateField;

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string SubjectNo
        {
            get
            {
                return this.subjectNoField;
            }
            set
            {
                this.subjectNoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfoSubjectName SubjectName
        {
            get
            {
                return this.subjectNameField;
            }
            set
            {
                this.subjectNameField = value;
            }
        }

        /// <remarks/>
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }

        /// <remarks/>
        public string SSN
        {
            get
            {
                return this.sSNField;
            }
            set
            {
                this.sSNField = value;
            }
        }

        /// <remarks/>
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }

        /// <remarks/>
        public string RelationCode
        {
            get
            {
                return this.relationCodeField;
            }
            set
            {
                this.relationCodeField = value;
            }
        }

        /// <remarks/>
        public string RelationCodeDesc
        {
            get
            {
                return this.relationCodeDescField;
            }
            set
            {
                this.relationCodeDescField = value;
            }
        }

        /// <remarks/>
        public string DLNumber
        {
            get
            {
                return this.dLNumberField;
            }
            set
            {
                this.dLNumberField = value;
            }
        }

        /// <remarks/>
        public string DLState
        {
            get
            {
                return this.dLStateField;
            }
            set
            {
                this.dLStateField = value;
            }
        }

        /// <remarks/>
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionSubjectInfoSubjectName
    {

        private string firstNameField;

        private string middleNameField;

        private string lastNameField;

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionFinanceInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string numField;

        private string vINField;

        private object propertyAddressField;

        private string lienHolderNameField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionFinanceInfoFinanceCompanyAddress financeCompanyAddressField;

        private string aBANumberField;

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string Num
        {
            get
            {
                return this.numField;
            }
            set
            {
                this.numField = value;
            }
        }

        /// <remarks/>
        public string VIN
        {
            get
            {
                return this.vINField;
            }
            set
            {
                this.vINField = value;
            }
        }

        /// <remarks/>
        public object PropertyAddress
        {
            get
            {
                return this.propertyAddressField;
            }
            set
            {
                this.propertyAddressField = value;
            }
        }

        /// <remarks/>
        public string LienHolderName
        {
            get
            {
                return this.lienHolderNameField;
            }
            set
            {
                this.lienHolderNameField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionFinanceInfoFinanceCompanyAddress FinanceCompanyAddress
        {
            get
            {
                return this.financeCompanyAddressField;
            }
            set
            {
                this.financeCompanyAddressField = value;
            }
        }

        /// <remarks/>
        public string ABANumber
        {
            get
            {
                return this.aBANumberField;
            }
            set
            {
                this.aBANumberField = value;
            }
        }

        /// <remarks/>
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionFinanceInfoFinanceCompanyAddress
    {

        private string addressLine1Field;

        private string cityField;

        private string stateField;

        private string zipField;

        /// <remarks/>
        public string AddressLine1
        {
            get
            {
                return this.addressLine1Field;
            }
            set
            {
                this.addressLine1Field = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public string Zip
        {
            get
            {
                return this.zipField;
            }
            set
            {
                this.zipField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionMatchBasisInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string matchScoreField;

        private string matchReason1Field;

        private string matchReason2Field;

        private string matchReason3Field;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string MatchScore
        {
            get
            {
                return this.matchScoreField;
            }
            set
            {
                this.matchScoreField = value;
            }
        }

        /// <remarks/>
        public string MatchReason1
        {
            get
            {
                return this.matchReason1Field;
            }
            set
            {
                this.matchReason1Field = value;
            }
        }

        /// <remarks/>
        public string MatchReason2
        {
            get
            {
                return this.matchReason2Field;
            }
            set
            {
                this.matchReason2Field = value;
            }
        }

        /// <remarks/>
        public string MatchReason3
        {
            get
            {
                return this.matchReason3Field;
            }
            set
            {
                this.matchReason3Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyDetailSectionDisputeStatementInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySection
    {

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionCarrierInfo carrierInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfo[] policyHistoryInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionTransactionInfo[] transactionInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionMailingAddressInfo[] mailingAddressInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionCoverageInfo coverageInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionVehicleInfo[] vehicleInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionGarageLocationInfo[] garageLocationInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionFinanceInfo financeInfoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfo[] subjectInfoField;

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionCarrierInfo CarrierInfo
        {
            get
            {
                return this.carrierInfoField;
            }
            set
            {
                this.carrierInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PolicyHistoryInfo")]
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfo[] PolicyHistoryInfo
        {
            get
            {
                return this.policyHistoryInfoField;
            }
            set
            {
                this.policyHistoryInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TransactionInfo")]
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionTransactionInfo[] TransactionInfo
        {
            get
            {
                return this.transactionInfoField;
            }
            set
            {
                this.transactionInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("MailingAddressInfo")]
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionMailingAddressInfo[] MailingAddressInfo
        {
            get
            {
                return this.mailingAddressInfoField;
            }
            set
            {
                this.mailingAddressInfoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionCoverageInfo CoverageInfo
        {
            get
            {
                return this.coverageInfoField;
            }
            set
            {
                this.coverageInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("VehicleInfo")]
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionVehicleInfo[] VehicleInfo
        {
            get
            {
                return this.vehicleInfoField;
            }
            set
            {
                this.vehicleInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GarageLocationInfo")]
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionGarageLocationInfo[] GarageLocationInfo
        {
            get
            {
                return this.garageLocationInfoField;
            }
            set
            {
                this.garageLocationInfoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionFinanceInfo FinanceInfo
        {
            get
            {
                return this.financeInfoField;
            }
            set
            {
                this.financeInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SubjectInfo")]
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfo[] SubjectInfo
        {
            get
            {
                return this.subjectInfoField;
            }
            set
            {
                this.subjectInfoField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionCarrierInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string aMBestField;

        private string carrierNameField;

        private string nAICCodeField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string AMBest
        {
            get
            {
                return this.aMBestField;
            }
            set
            {
                this.aMBestField = value;
            }
        }

        /// <remarks/>
        public string CarrierName
        {
            get
            {
                return this.carrierNameField;
            }
            set
            {
                this.carrierNameField = value;
            }
        }

        /// <remarks/>
        public string NAICCode
        {
            get
            {
                return this.nAICCodeField;
            }
            set
            {
                this.nAICCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string policyNumberField;

        private string riskClassCodeField;

        private string riskClassCodeDescField;

        private string policyTypeField;

        private string policyTypeDescField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfoPolicyHolder1 policyHolder1Field;

        private object policyHolder2Field;

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string PolicyNumber
        {
            get
            {
                return this.policyNumberField;
            }
            set
            {
                this.policyNumberField = value;
            }
        }

        /// <remarks/>
        public string RiskClassCode
        {
            get
            {
                return this.riskClassCodeField;
            }
            set
            {
                this.riskClassCodeField = value;
            }
        }

        /// <remarks/>
        public string RiskClassCodeDesc
        {
            get
            {
                return this.riskClassCodeDescField;
            }
            set
            {
                this.riskClassCodeDescField = value;
            }
        }

        /// <remarks/>
        public string PolicyType
        {
            get
            {
                return this.policyTypeField;
            }
            set
            {
                this.policyTypeField = value;
            }
        }

        /// <remarks/>
        public string PolicyTypeDesc
        {
            get
            {
                return this.policyTypeDescField;
            }
            set
            {
                this.policyTypeDescField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfoPolicyHolder1 PolicyHolder1
        {
            get
            {
                return this.policyHolder1Field;
            }
            set
            {
                this.policyHolder1Field = value;
            }
        }

        /// <remarks/>
        public object PolicyHolder2
        {
            get
            {
                return this.policyHolder2Field;
            }
            set
            {
                this.policyHolder2Field = value;
            }
        }

        /// <remarks/>
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionPolicyHistoryInfoPolicyHolder1
    {

        private string firstNameField;

        private string middleNameField;

        private string lastNameField;

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionTransactionInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string transactionTypeField;

        private string transactionDescField;

        private string effectiveDateField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string TransactionType
        {
            get
            {
                return this.transactionTypeField;
            }
            set
            {
                this.transactionTypeField = value;
            }
        }

        /// <remarks/>
        public string TransactionDesc
        {
            get
            {
                return this.transactionDescField;
            }
            set
            {
                this.transactionDescField = value;
            }
        }

        /// <remarks/>
        public string EffectiveDate
        {
            get
            {
                return this.effectiveDateField;
            }
            set
            {
                this.effectiveDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionMailingAddressInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionMailingAddressInfoMailingAddress mailingAddressField;

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionMailingAddressInfoMailingAddress MailingAddress
        {
            get
            {
                return this.mailingAddressField;
            }
            set
            {
                this.mailingAddressField = value;
            }
        }

        /// <remarks/>
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionMailingAddressInfoMailingAddress
    {

        private string addressLine1Field;

        private string addressLine2Field;

        private string cityField;

        private string stateField;

        private string zipField;

        /// <remarks/>
        public string AddressLine1
        {
            get
            {
                return this.addressLine1Field;
            }
            set
            {
                this.addressLine1Field = value;
            }
        }

        /// <remarks/>
        public string AddressLine2
        {
            get
            {
                return this.addressLine2Field;
            }
            set
            {
                this.addressLine2Field = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public string Zip
        {
            get
            {
                return this.zipField;
            }
            set
            {
                this.zipField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionCoverageInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private object propertyAddressField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionCoverageInfoAddressCoverageInfo[] addressCoverageInfoField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public object PropertyAddress
        {
            get
            {
                return this.propertyAddressField;
            }
            set
            {
                this.propertyAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AddressCoverageInfo")]
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionCoverageInfoAddressCoverageInfo[] AddressCoverageInfo
        {
            get
            {
                return this.addressCoverageInfoField;
            }
            set
            {
                this.addressCoverageInfoField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionCoverageInfoAddressCoverageInfo
    {

        private string coverageCodeField;

        private string coverageCodeDescField;

        private string individualLimitField;

        private string occuranceLimitField;

        private string cSILimitField;

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        public string CoverageCode
        {
            get
            {
                return this.coverageCodeField;
            }
            set
            {
                this.coverageCodeField = value;
            }
        }

        /// <remarks/>
        public string CoverageCodeDesc
        {
            get
            {
                return this.coverageCodeDescField;
            }
            set
            {
                this.coverageCodeDescField = value;
            }
        }

        /// <remarks/>
        public string IndividualLimit
        {
            get
            {
                return this.individualLimitField;
            }
            set
            {
                this.individualLimitField = value;
            }
        }

        /// <remarks/>
        public string OccuranceLimit
        {
            get
            {
                return this.occuranceLimitField;
            }
            set
            {
                this.occuranceLimitField = value;
            }
        }

        /// <remarks/>
        public string CSILimit
        {
            get
            {
                return this.cSILimitField;
            }
            set
            {
                this.cSILimitField = value;
            }
        }

        /// <remarks/>
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionVehicleInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string vehicleNoField;

        private string yearField;

        private string makeField;

        private string modelField;

        private string vINField;

        private string classCodeField;

        private string collDeductibleField;

        private bool collDeductibleFieldSpecified;

        private string compDeductibleField;

        private bool compDeductibleFieldSpecified;

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string VehicleNo
        {
            get
            {
                return this.vehicleNoField;
            }
            set
            {
                this.vehicleNoField = value;
            }
        }

        /// <remarks/>
        public string Year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
            }
        }

        /// <remarks/>
        public string Make
        {
            get
            {
                return this.makeField;
            }
            set
            {
                this.makeField = value;
            }
        }

        /// <remarks/>
        public string Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        public string VIN
        {
            get
            {
                return this.vINField;
            }
            set
            {
                this.vINField = value;
            }
        }

        /// <remarks/>
        public string ClassCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        public string CollDeductible
        {
            get
            {
                return this.collDeductibleField;
            }
            set
            {
                this.collDeductibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CollDeductibleSpecified
        {
            get
            {
                return this.collDeductibleFieldSpecified;
            }
            set
            {
                this.collDeductibleFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string CompDeductible
        {
            get
            {
                return this.compDeductibleField;
            }
            set
            {
                this.compDeductibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CompDeductibleSpecified
        {
            get
            {
                return this.compDeductibleFieldSpecified;
            }
            set
            {
                this.compDeductibleFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionGarageLocationInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string vehicleNoField;

        private string vINField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionGarageLocationInfoGarageAddress garageAddressField;

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string VehicleNo
        {
            get
            {
                return this.vehicleNoField;
            }
            set
            {
                this.vehicleNoField = value;
            }
        }

        /// <remarks/>
        public string VIN
        {
            get
            {
                return this.vINField;
            }
            set
            {
                this.vINField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionGarageLocationInfoGarageAddress GarageAddress
        {
            get
            {
                return this.garageAddressField;
            }
            set
            {
                this.garageAddressField = value;
            }
        }

        /// <remarks/>
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionGarageLocationInfoGarageAddress
    {

        private string addressLine1Field;

        private string addressLine2Field;

        private bool addressLine2FieldSpecified;

        private string cityField;

        private string stateField;

        private string zipField;

        /// <remarks/>
        public string AddressLine1
        {
            get
            {
                return this.addressLine1Field;
            }
            set
            {
                this.addressLine1Field = value;
            }
        }

        /// <remarks/>
        public string AddressLine2
        {
            get
            {
                return this.addressLine2Field;
            }
            set
            {
                this.addressLine2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AddressLine2Specified
        {
            get
            {
                return this.addressLine2FieldSpecified;
            }
            set
            {
                this.addressLine2FieldSpecified = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public string Zip
        {
            get
            {
                return this.zipField;
            }
            set
            {
                this.zipField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionFinanceInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string numField;

        private string vINField;

        private object propertyAddressField;

        private string lienHolderNameField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionFinanceInfoFinanceCompanyAddress financeCompanyAddressField;

        private string aBANumberField;

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string Num
        {
            get
            {
                return this.numField;
            }
            set
            {
                this.numField = value;
            }
        }

        /// <remarks/>
        public string VIN
        {
            get
            {
                return this.vINField;
            }
            set
            {
                this.vINField = value;
            }
        }

        /// <remarks/>
        public object PropertyAddress
        {
            get
            {
                return this.propertyAddressField;
            }
            set
            {
                this.propertyAddressField = value;
            }
        }

        /// <remarks/>
        public string LienHolderName
        {
            get
            {
                return this.lienHolderNameField;
            }
            set
            {
                this.lienHolderNameField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionFinanceInfoFinanceCompanyAddress FinanceCompanyAddress
        {
            get
            {
                return this.financeCompanyAddressField;
            }
            set
            {
                this.financeCompanyAddressField = value;
            }
        }

        /// <remarks/>
        public string ABANumber
        {
            get
            {
                return this.aBANumberField;
            }
            set
            {
                this.aBANumberField = value;
            }
        }

        /// <remarks/>
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionFinanceInfoFinanceCompanyAddress
    {

        private string addressLine1Field;

        private string cityField;

        private string stateField;

        private string zipField;

        /// <remarks/>
        public string AddressLine1
        {
            get
            {
                return this.addressLine1Field;
            }
            set
            {
                this.addressLine1Field = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public string Zip
        {
            get
            {
                return this.zipField;
            }
            set
            {
                this.zipField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private string reportSectionField;

        private string subjectNoField;

        private ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfoSubjectName subjectNameField;

        private string dateOfBirthField;

        private string sSNField;

        private string genderField;

        private string relationCodeField;

        private string relationCodeDescField;

        private string dLNumberField;

        private string dLStateField;

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public string ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }

        /// <remarks/>
        public string SubjectNo
        {
            get
            {
                return this.subjectNoField;
            }
            set
            {
                this.subjectNoField = value;
            }
        }

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfoSubjectName SubjectName
        {
            get
            {
                return this.subjectNameField;
            }
            set
            {
                this.subjectNameField = value;
            }
        }

        /// <remarks/>
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }

        /// <remarks/>
        public string SSN
        {
            get
            {
                return this.sSNField;
            }
            set
            {
                this.sSNField = value;
            }
        }

        /// <remarks/>
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }

        /// <remarks/>
        public string RelationCode
        {
            get
            {
                return this.relationCodeField;
            }
            set
            {
                this.relationCodeField = value;
            }
        }

        /// <remarks/>
        public string RelationCodeDesc
        {
            get
            {
                return this.relationCodeDescField;
            }
            set
            {
                this.relationCodeDescField = value;
            }
        }

        /// <remarks/>
        public string DLNumber
        {
            get
            {
                return this.dLNumberField;
            }
            set
            {
                this.dLNumberField = value;
            }
        }

        /// <remarks/>
        public string DLState
        {
            get
            {
                return this.dLStateField;
            }
            set
            {
                this.dLStateField = value;
            }
        }

        /// <remarks/>
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierPolicyHistorySectionSubjectInfoSubjectName
    {

        private string firstNameField;

        private string middleNameField;

        private string lastNameField;

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataCoverageVerifierTrailerInfo
    {

        private string sequenceNumberField;

        private string recordTypeField;

        private object recordsTransmittedField;

        /// <remarks/>
        public string SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string RecordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        public object RecordsTransmitted
        {
            get
            {
                return this.recordsTransmittedField;
            }
            set
            {
                this.recordsTransmittedField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataACXIOM
    {

        private ISOPassportSvcRsReportReportDataACXIOMREQGRPHEAD rEQGRPHEADField;

        private ISOPassportSvcRsReportReportDataACXIOMPRODRESHEAD pRODRESHEADField;

        private ISOPassportSvcRsReportReportDataACXIOMSUPADDRLAYOUT sUPADDRLAYOUTField;

        private ISOPassportSvcRsReportReportDataACXIOMSUPINDIVLAYOUT sUPINDIVLAYOUTField;

        private ISOPassportSvcRsReportReportDataACXIOMRETRESPONSE rETRESPONSEField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("REQ-GRP-HEAD")]
        public ISOPassportSvcRsReportReportDataACXIOMREQGRPHEAD REQGRPHEAD
        {
            get
            {
                return this.rEQGRPHEADField;
            }
            set
            {
                this.rEQGRPHEADField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PROD-RES-HEAD")]
        public ISOPassportSvcRsReportReportDataACXIOMPRODRESHEAD PRODRESHEAD
        {
            get
            {
                return this.pRODRESHEADField;
            }
            set
            {
                this.pRODRESHEADField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SUP-ADDR-LAYOUT")]
        public ISOPassportSvcRsReportReportDataACXIOMSUPADDRLAYOUT SUPADDRLAYOUT
        {
            get
            {
                return this.sUPADDRLAYOUTField;
            }
            set
            {
                this.sUPADDRLAYOUTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SUP-INDIV-LAYOUT")]
        public ISOPassportSvcRsReportReportDataACXIOMSUPINDIVLAYOUT SUPINDIVLAYOUT
        {
            get
            {
                return this.sUPINDIVLAYOUTField;
            }
            set
            {
                this.sUPINDIVLAYOUTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RET-RESPONSE")]
        public ISOPassportSvcRsReportReportDataACXIOMRETRESPONSE RETRESPONSE
        {
            get
            {
                return this.rETRESPONSEField;
            }
            set
            {
                this.rETRESPONSEField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataACXIOMREQGRPHEAD
    {

        private string gH_RECTYPEField;

        private string gH_RECVERSIONField;

        private string gH_AXMBATNUMField;

        private string gH_CLIENTNUMField;

        private string gH_CLIENTNUMSUFFField;

        private object gH_BRANCHNUMField;

        private object gH_USERIDField;

        private object gH_SEQNUMField;

        private string gH_REPORTNUMField;

        private object gH_INTWRKAREAField;

        private object gH_BATCHPROCINDField;

        private object gH_CREDITSUBCODEField;

        private object gH_FILLERField;

        private string gH_NUMPRODREQField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GH_REC-TYPE")]
        public string GH_RECTYPE
        {
            get
            {
                return this.gH_RECTYPEField;
            }
            set
            {
                this.gH_RECTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GH_REC-VERSION")]
        public string GH_RECVERSION
        {
            get
            {
                return this.gH_RECVERSIONField;
            }
            set
            {
                this.gH_RECVERSIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GH_AXM-BAT-NUM")]
        public string GH_AXMBATNUM
        {
            get
            {
                return this.gH_AXMBATNUMField;
            }
            set
            {
                this.gH_AXMBATNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GH_CLIENT-NUM")]
        public string GH_CLIENTNUM
        {
            get
            {
                return this.gH_CLIENTNUMField;
            }
            set
            {
                this.gH_CLIENTNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GH_CLIENT-NUM-SUFF")]
        public string GH_CLIENTNUMSUFF
        {
            get
            {
                return this.gH_CLIENTNUMSUFFField;
            }
            set
            {
                this.gH_CLIENTNUMSUFFField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GH_BRANCH-NUM")]
        public object GH_BRANCHNUM
        {
            get
            {
                return this.gH_BRANCHNUMField;
            }
            set
            {
                this.gH_BRANCHNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GH_USER-ID")]
        public object GH_USERID
        {
            get
            {
                return this.gH_USERIDField;
            }
            set
            {
                this.gH_USERIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GH_SEQ-NUM")]
        public object GH_SEQNUM
        {
            get
            {
                return this.gH_SEQNUMField;
            }
            set
            {
                this.gH_SEQNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GH_REPORT-NUM")]
        public string GH_REPORTNUM
        {
            get
            {
                return this.gH_REPORTNUMField;
            }
            set
            {
                this.gH_REPORTNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GH_INT-WRK-AREA")]
        public object GH_INTWRKAREA
        {
            get
            {
                return this.gH_INTWRKAREAField;
            }
            set
            {
                this.gH_INTWRKAREAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GH_BATCH-PROC-IND")]
        public object GH_BATCHPROCIND
        {
            get
            {
                return this.gH_BATCHPROCINDField;
            }
            set
            {
                this.gH_BATCHPROCINDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GH_CREDIT-SUB-CODE")]
        public object GH_CREDITSUBCODE
        {
            get
            {
                return this.gH_CREDITSUBCODEField;
            }
            set
            {
                this.gH_CREDITSUBCODEField = value;
            }
        }

        /// <remarks/>
        public object GH_FILLER
        {
            get
            {
                return this.gH_FILLERField;
            }
            set
            {
                this.gH_FILLERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GH_NUM-PROD-REQ")]
        public string GH_NUMPRODREQ
        {
            get
            {
                return this.gH_NUMPRODREQField;
            }
            set
            {
                this.gH_NUMPRODREQField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataACXIOMPRODRESHEAD
    {

        private string pH_RECTYPEField;

        private string pH_RECVERSIONField;

        private string pH_PRODREQField;

        private string pH_VENDORField;

        private string pH_SEQNUMField;

        private string pH_MATCHTYPEField;

        private string pH_LOWAGEFILTERField;

        private string pH_HIGH_AGEFILTERField;

        private string pH_RETRECLIMITField;

        private string pH_SCRUBFLAGField;

        private string pH_EDITRETCODEField;

        private string pH_PRIORITYField;

        private string pH_AXMHISTField;

        private string pH_VENDHISTField;

        private string pH_CUSTHISTVENDORField;

        private object pH_FILLERField;

        private string pH_BILLABLEFLAGField;

        private object pH_FILLER2Field;

        private string pH_RESULTFLAGField;

        private string pH_NUMINDIVRETField;

        private string pH_NUMUNDISCRETField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_REC-TYPE")]
        public string PH_RECTYPE
        {
            get
            {
                return this.pH_RECTYPEField;
            }
            set
            {
                this.pH_RECTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_REC-VERSION")]
        public string PH_RECVERSION
        {
            get
            {
                return this.pH_RECVERSIONField;
            }
            set
            {
                this.pH_RECVERSIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_PROD-REQ")]
        public string PH_PRODREQ
        {
            get
            {
                return this.pH_PRODREQField;
            }
            set
            {
                this.pH_PRODREQField = value;
            }
        }

        /// <remarks/>
        public string PH_VENDOR
        {
            get
            {
                return this.pH_VENDORField;
            }
            set
            {
                this.pH_VENDORField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_SEQ-NUM")]
        public string PH_SEQNUM
        {
            get
            {
                return this.pH_SEQNUMField;
            }
            set
            {
                this.pH_SEQNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_MATCH-TYPE")]
        public string PH_MATCHTYPE
        {
            get
            {
                return this.pH_MATCHTYPEField;
            }
            set
            {
                this.pH_MATCHTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_LOW-AGE-FILTER")]
        public string PH_LOWAGEFILTER
        {
            get
            {
                return this.pH_LOWAGEFILTERField;
            }
            set
            {
                this.pH_LOWAGEFILTERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_HIGH_AGE-FILTER")]
        public string PH_HIGH_AGEFILTER
        {
            get
            {
                return this.pH_HIGH_AGEFILTERField;
            }
            set
            {
                this.pH_HIGH_AGEFILTERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_RET-REC-LIMIT")]
        public string PH_RETRECLIMIT
        {
            get
            {
                return this.pH_RETRECLIMITField;
            }
            set
            {
                this.pH_RETRECLIMITField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_SCRUB-FLAG")]
        public string PH_SCRUBFLAG
        {
            get
            {
                return this.pH_SCRUBFLAGField;
            }
            set
            {
                this.pH_SCRUBFLAGField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_EDIT-RET-CODE")]
        public string PH_EDITRETCODE
        {
            get
            {
                return this.pH_EDITRETCODEField;
            }
            set
            {
                this.pH_EDITRETCODEField = value;
            }
        }

        /// <remarks/>
        public string PH_PRIORITY
        {
            get
            {
                return this.pH_PRIORITYField;
            }
            set
            {
                this.pH_PRIORITYField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_AXM-HIST")]
        public string PH_AXMHIST
        {
            get
            {
                return this.pH_AXMHISTField;
            }
            set
            {
                this.pH_AXMHISTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_VEND-HIST")]
        public string PH_VENDHIST
        {
            get
            {
                return this.pH_VENDHISTField;
            }
            set
            {
                this.pH_VENDHISTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_CUST-HIST-VENDOR")]
        public string PH_CUSTHISTVENDOR
        {
            get
            {
                return this.pH_CUSTHISTVENDORField;
            }
            set
            {
                this.pH_CUSTHISTVENDORField = value;
            }
        }

        /// <remarks/>
        public object PH_FILLER
        {
            get
            {
                return this.pH_FILLERField;
            }
            set
            {
                this.pH_FILLERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_BILLABLE-FLAG")]
        public string PH_BILLABLEFLAG
        {
            get
            {
                return this.pH_BILLABLEFLAGField;
            }
            set
            {
                this.pH_BILLABLEFLAGField = value;
            }
        }

        /// <remarks/>
        public object PH_FILLER2
        {
            get
            {
                return this.pH_FILLER2Field;
            }
            set
            {
                this.pH_FILLER2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_RESULT-FLAG")]
        public string PH_RESULTFLAG
        {
            get
            {
                return this.pH_RESULTFLAGField;
            }
            set
            {
                this.pH_RESULTFLAGField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_NUM-INDIV-RET")]
        public string PH_NUMINDIVRET
        {
            get
            {
                return this.pH_NUMINDIVRETField;
            }
            set
            {
                this.pH_NUMINDIVRETField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PH_NUM-UNDISC-RET")]
        public string PH_NUMUNDISCRET
        {
            get
            {
                return this.pH_NUMUNDISCRETField;
            }
            set
            {
                this.pH_NUMUNDISCRETField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataACXIOMSUPADDRLAYOUT
    {

        private string sA_RECTYPEField;

        private string sA_RECVERSIONField;

        private object sA_SASEQNUMField;

        private object sA_RASEQNUMField;

        private string sA_ADDRSTATUSField;

        private object sA_ADRESSField;

        private string sA_PRINUMField;

        private object sA_PRE_DIRECTField;

        private string sA_STREETNAMEField;

        private object sA_STREETSUFFIXField;

        private object sA_UNITDESIGField;

        private object sA_SECNUMField;

        private string sA_CITYField;

        private string sA_STATEField;

        private string sA_ZIPCODEField;

        private string sA_ZIPEXTField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SA_REC-TYPE")]
        public string SA_RECTYPE
        {
            get
            {
                return this.sA_RECTYPEField;
            }
            set
            {
                this.sA_RECTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SA_REC-VERSION")]
        public string SA_RECVERSION
        {
            get
            {
                return this.sA_RECVERSIONField;
            }
            set
            {
                this.sA_RECVERSIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SA_SA-SEQ-NUM")]
        public object SA_SASEQNUM
        {
            get
            {
                return this.sA_SASEQNUMField;
            }
            set
            {
                this.sA_SASEQNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SA_RA-SEQ-NUM")]
        public object SA_RASEQNUM
        {
            get
            {
                return this.sA_RASEQNUMField;
            }
            set
            {
                this.sA_RASEQNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SA_ADDR-STATUS")]
        public string SA_ADDRSTATUS
        {
            get
            {
                return this.sA_ADDRSTATUSField;
            }
            set
            {
                this.sA_ADDRSTATUSField = value;
            }
        }

        /// <remarks/>
        public object SA_ADRESS
        {
            get
            {
                return this.sA_ADRESSField;
            }
            set
            {
                this.sA_ADRESSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SA_PRI-NUM")]
        public string SA_PRINUM
        {
            get
            {
                return this.sA_PRINUMField;
            }
            set
            {
                this.sA_PRINUMField = value;
            }
        }

        /// <remarks/>
        public object SA_PRE_DIRECT
        {
            get
            {
                return this.sA_PRE_DIRECTField;
            }
            set
            {
                this.sA_PRE_DIRECTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SA_STREET-NAME")]
        public string SA_STREETNAME
        {
            get
            {
                return this.sA_STREETNAMEField;
            }
            set
            {
                this.sA_STREETNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SA_STREET-SUFFIX")]
        public object SA_STREETSUFFIX
        {
            get
            {
                return this.sA_STREETSUFFIXField;
            }
            set
            {
                this.sA_STREETSUFFIXField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SA_UNIT-DESIG")]
        public object SA_UNITDESIG
        {
            get
            {
                return this.sA_UNITDESIGField;
            }
            set
            {
                this.sA_UNITDESIGField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SA_SEC-NUM")]
        public object SA_SECNUM
        {
            get
            {
                return this.sA_SECNUMField;
            }
            set
            {
                this.sA_SECNUMField = value;
            }
        }

        /// <remarks/>
        public string SA_CITY
        {
            get
            {
                return this.sA_CITYField;
            }
            set
            {
                this.sA_CITYField = value;
            }
        }

        /// <remarks/>
        public string SA_STATE
        {
            get
            {
                return this.sA_STATEField;
            }
            set
            {
                this.sA_STATEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SA_ZIP-CODE")]
        public string SA_ZIPCODE
        {
            get
            {
                return this.sA_ZIPCODEField;
            }
            set
            {
                this.sA_ZIPCODEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SA_ZIP-EXT")]
        public string SA_ZIPEXT
        {
            get
            {
                return this.sA_ZIPEXTField;
            }
            set
            {
                this.sA_ZIPEXTField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataACXIOMSUPINDIVLAYOUT
    {

        private string sI_RECTYPEField;

        private string sI_RECVERSIONField;

        private object sI_SISEQNUMField;

        private object sI_RISEQNUMField;

        private object sI_INDIVTYPEField;

        private string sI_NAMESTATUSField;

        private object sI_INDIVNAMEField;

        private string sI_LASTNAMEField;

        private string sI_FIRSTNAMEField;

        private string sI_MIDDLEINITIALField;

        private object sI_SUFFIXField;

        private object sI_GENDERField;

        private object sI_FILLERField;

        private string sI_DOBField;

        private object sI_AGEField;

        private string sI_SSNField;

        private object sI_DLSTATECODEField;

        private string sI_DLNUMBERField;

        private object sI_DLISSUEDATEField;

        private string sI_PHONEFLAGField;

        private string sI_PHONENUMField;

        private object sI_SOURCEOFINDIVField;

        private object sI_GRADCENTField;

        private object sI_GRADYEARField;

        private object sI_FILLER2Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_REC-TYPE")]
        public string SI_RECTYPE
        {
            get
            {
                return this.sI_RECTYPEField;
            }
            set
            {
                this.sI_RECTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_REC-VERSION")]
        public string SI_RECVERSION
        {
            get
            {
                return this.sI_RECVERSIONField;
            }
            set
            {
                this.sI_RECVERSIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_SI-SEQ-NUM")]
        public object SI_SISEQNUM
        {
            get
            {
                return this.sI_SISEQNUMField;
            }
            set
            {
                this.sI_SISEQNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_RI-SEQ-NUM")]
        public object SI_RISEQNUM
        {
            get
            {
                return this.sI_RISEQNUMField;
            }
            set
            {
                this.sI_RISEQNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_INDIV-TYPE")]
        public object SI_INDIVTYPE
        {
            get
            {
                return this.sI_INDIVTYPEField;
            }
            set
            {
                this.sI_INDIVTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_NAME-STATUS")]
        public string SI_NAMESTATUS
        {
            get
            {
                return this.sI_NAMESTATUSField;
            }
            set
            {
                this.sI_NAMESTATUSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_INDIV-NAME")]
        public object SI_INDIVNAME
        {
            get
            {
                return this.sI_INDIVNAMEField;
            }
            set
            {
                this.sI_INDIVNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_LAST-NAME")]
        public string SI_LASTNAME
        {
            get
            {
                return this.sI_LASTNAMEField;
            }
            set
            {
                this.sI_LASTNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_FIRST-NAME")]
        public string SI_FIRSTNAME
        {
            get
            {
                return this.sI_FIRSTNAMEField;
            }
            set
            {
                this.sI_FIRSTNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_MIDDLE-INITIAL")]
        public string SI_MIDDLEINITIAL
        {
            get
            {
                return this.sI_MIDDLEINITIALField;
            }
            set
            {
                this.sI_MIDDLEINITIALField = value;
            }
        }

        /// <remarks/>
        public object SI_SUFFIX
        {
            get
            {
                return this.sI_SUFFIXField;
            }
            set
            {
                this.sI_SUFFIXField = value;
            }
        }

        /// <remarks/>
        public object SI_GENDER
        {
            get
            {
                return this.sI_GENDERField;
            }
            set
            {
                this.sI_GENDERField = value;
            }
        }

        /// <remarks/>
        public object SI_FILLER
        {
            get
            {
                return this.sI_FILLERField;
            }
            set
            {
                this.sI_FILLERField = value;
            }
        }

        /// <remarks/>
        public string SI_DOB
        {
            get
            {
                return this.sI_DOBField;
            }
            set
            {
                this.sI_DOBField = value;
            }
        }

        /// <remarks/>
        public object SI_AGE
        {
            get
            {
                return this.sI_AGEField;
            }
            set
            {
                this.sI_AGEField = value;
            }
        }

        /// <remarks/>
        public string SI_SSN
        {
            get
            {
                return this.sI_SSNField;
            }
            set
            {
                this.sI_SSNField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_DL-STATE-CODE")]
        public object SI_DLSTATECODE
        {
            get
            {
                return this.sI_DLSTATECODEField;
            }
            set
            {
                this.sI_DLSTATECODEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_DL-NUMBER")]
        public string SI_DLNUMBER
        {
            get
            {
                return this.sI_DLNUMBERField;
            }
            set
            {
                this.sI_DLNUMBERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_DL-ISSUE-DATE")]
        public object SI_DLISSUEDATE
        {
            get
            {
                return this.sI_DLISSUEDATEField;
            }
            set
            {
                this.sI_DLISSUEDATEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_PHONE-FLAG")]
        public string SI_PHONEFLAG
        {
            get
            {
                return this.sI_PHONEFLAGField;
            }
            set
            {
                this.sI_PHONEFLAGField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_PHONE-NUM")]
        public string SI_PHONENUM
        {
            get
            {
                return this.sI_PHONENUMField;
            }
            set
            {
                this.sI_PHONENUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_SOURCE-OF-INDIV")]
        public object SI_SOURCEOFINDIV
        {
            get
            {
                return this.sI_SOURCEOFINDIVField;
            }
            set
            {
                this.sI_SOURCEOFINDIVField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_GRAD-CENT")]
        public object SI_GRADCENT
        {
            get
            {
                return this.sI_GRADCENTField;
            }
            set
            {
                this.sI_GRADCENTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SI_GRAD-YEAR")]
        public object SI_GRADYEAR
        {
            get
            {
                return this.sI_GRADYEARField;
            }
            set
            {
                this.sI_GRADYEARField = value;
            }
        }

        /// <remarks/>
        public object SI_FILLER2
        {
            get
            {
                return this.sI_FILLER2Field;
            }
            set
            {
                this.sI_FILLER2Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataACXIOMRETRESPONSE
    {

        private ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUT rETADDRLAYOUTField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RET-ADDR-LAYOUT")]
        public ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUT RETADDRLAYOUT
        {
            get
            {
                return this.rETADDRLAYOUTField;
            }
            set
            {
                this.rETADDRLAYOUTField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUT
    {

        private string rA_RECTYPEField;

        private string rA_RECVERSIONField;

        private object rA_SASEQNUMField;

        private string rA_RASEQNUMField;

        private string rA_ADDRSTATUSField;

        private object rA_ADRESSField;

        private string rA_PRINUMField;

        private object rA_PRE_DIRECTField;

        private string rA_STREETNAMEField;

        private string rA_STREETSUFFIXField;

        private object rA_UNITDESIGField;

        private object rA_SECNUMField;

        private string rA_CITYField;

        private string rA_STATEField;

        private string rA_ZIPCODEField;

        private string rA_ZIPEXTField;

        private object rA_ADDTYPEField;

        private object rA_DATEREPORTEDField;

        private object rA_FILLERField;

        private object rA_GEOCODELEVELField;

        private object rA_LENOFRESField;

        private object rA_OWNField;

        private string rA_MATCHTYPEField;

        private string rA_UNITTYPEField;

        private ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT[] rETINDIVLAYOUTField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_REC-TYPE")]
        public string RA_RECTYPE
        {
            get
            {
                return this.rA_RECTYPEField;
            }
            set
            {
                this.rA_RECTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_REC-VERSION")]
        public string RA_RECVERSION
        {
            get
            {
                return this.rA_RECVERSIONField;
            }
            set
            {
                this.rA_RECVERSIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_SA-SEQ-NUM")]
        public object RA_SASEQNUM
        {
            get
            {
                return this.rA_SASEQNUMField;
            }
            set
            {
                this.rA_SASEQNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_RA-SEQ-NUM")]
        public string RA_RASEQNUM
        {
            get
            {
                return this.rA_RASEQNUMField;
            }
            set
            {
                this.rA_RASEQNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_ADDR-STATUS")]
        public string RA_ADDRSTATUS
        {
            get
            {
                return this.rA_ADDRSTATUSField;
            }
            set
            {
                this.rA_ADDRSTATUSField = value;
            }
        }

        /// <remarks/>
        public object RA_ADRESS
        {
            get
            {
                return this.rA_ADRESSField;
            }
            set
            {
                this.rA_ADRESSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_PRI-NUM")]
        public string RA_PRINUM
        {
            get
            {
                return this.rA_PRINUMField;
            }
            set
            {
                this.rA_PRINUMField = value;
            }
        }

        /// <remarks/>
        public object RA_PRE_DIRECT
        {
            get
            {
                return this.rA_PRE_DIRECTField;
            }
            set
            {
                this.rA_PRE_DIRECTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_STREET-NAME")]
        public string RA_STREETNAME
        {
            get
            {
                return this.rA_STREETNAMEField;
            }
            set
            {
                this.rA_STREETNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_STREET-SUFFIX")]
        public string RA_STREETSUFFIX
        {
            get
            {
                return this.rA_STREETSUFFIXField;
            }
            set
            {
                this.rA_STREETSUFFIXField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_UNIT-DESIG")]
        public object RA_UNITDESIG
        {
            get
            {
                return this.rA_UNITDESIGField;
            }
            set
            {
                this.rA_UNITDESIGField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_SEC-NUM")]
        public object RA_SECNUM
        {
            get
            {
                return this.rA_SECNUMField;
            }
            set
            {
                this.rA_SECNUMField = value;
            }
        }

        /// <remarks/>
        public string RA_CITY
        {
            get
            {
                return this.rA_CITYField;
            }
            set
            {
                this.rA_CITYField = value;
            }
        }

        /// <remarks/>
        public string RA_STATE
        {
            get
            {
                return this.rA_STATEField;
            }
            set
            {
                this.rA_STATEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_ZIP-CODE")]
        public string RA_ZIPCODE
        {
            get
            {
                return this.rA_ZIPCODEField;
            }
            set
            {
                this.rA_ZIPCODEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_ZIP-EXT")]
        public string RA_ZIPEXT
        {
            get
            {
                return this.rA_ZIPEXTField;
            }
            set
            {
                this.rA_ZIPEXTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_ADD-TYPE")]
        public object RA_ADDTYPE
        {
            get
            {
                return this.rA_ADDTYPEField;
            }
            set
            {
                this.rA_ADDTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_DATE-REPORTED")]
        public object RA_DATEREPORTED
        {
            get
            {
                return this.rA_DATEREPORTEDField;
            }
            set
            {
                this.rA_DATEREPORTEDField = value;
            }
        }

        /// <remarks/>
        public object RA_FILLER
        {
            get
            {
                return this.rA_FILLERField;
            }
            set
            {
                this.rA_FILLERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_GEO-CODE-LEVEL")]
        public object RA_GEOCODELEVEL
        {
            get
            {
                return this.rA_GEOCODELEVELField;
            }
            set
            {
                this.rA_GEOCODELEVELField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_LEN-OF-RES")]
        public object RA_LENOFRES
        {
            get
            {
                return this.rA_LENOFRESField;
            }
            set
            {
                this.rA_LENOFRESField = value;
            }
        }

        /// <remarks/>
        public object RA_OWN
        {
            get
            {
                return this.rA_OWNField;
            }
            set
            {
                this.rA_OWNField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_MATCH-TYPE")]
        public string RA_MATCHTYPE
        {
            get
            {
                return this.rA_MATCHTYPEField;
            }
            set
            {
                this.rA_MATCHTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RA_UNIT-TYPE")]
        public string RA_UNITTYPE
        {
            get
            {
                return this.rA_UNITTYPEField;
            }
            set
            {
                this.rA_UNITTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RET-INDIV-LAYOUT")]
        public ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT[] RETINDIVLAYOUT
        {
            get
            {
                return this.rETINDIVLAYOUTField;
            }
            set
            {
                this.rETINDIVLAYOUTField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataACXIOMRETRESPONSERETADDRLAYOUTRETINDIVLAYOUT
    {

        private string rI_RECTYPEField;

        private string rI_RECVERSIONField;

        private object rI_SISEQNUMField;

        private string rI_RISEQNUMField;

        private string rI_INDIVTYPEField;

        private string rI_NAMESTATUSField;

        private object rI_INDIVNAMEField;

        private string rI_LASTNAMEField;

        private string rI_FIRSTNAMEField;

        private string rI_MIDDLEINITIALField;

        private object rI_SUFFIXField;

        private string rI_GENDERField;

        private object rI_FILLERField;

        private string rI_DOBField;

        private string rI_AGEField;

        private string rI_SSNField;

        private object rI_DLSTATECODEField;

        private string rI_DLNUMBERField;

        private object rI_DLISSUEDATEField;

        private object rI_PHONEFLAGField;

        private string rI_PHONENUMField;

        private string rI_SOURCEOFINDIVField;

        private object rI_GRADCENTField;

        private object rI_GRADYEARField;

        private object rI_FILLER2Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_REC-TYPE")]
        public string RI_RECTYPE
        {
            get
            {
                return this.rI_RECTYPEField;
            }
            set
            {
                this.rI_RECTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_REC-VERSION")]
        public string RI_RECVERSION
        {
            get
            {
                return this.rI_RECVERSIONField;
            }
            set
            {
                this.rI_RECVERSIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_SI-SEQ-NUM")]
        public object RI_SISEQNUM
        {
            get
            {
                return this.rI_SISEQNUMField;
            }
            set
            {
                this.rI_SISEQNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_RI-SEQ-NUM")]
        public string RI_RISEQNUM
        {
            get
            {
                return this.rI_RISEQNUMField;
            }
            set
            {
                this.rI_RISEQNUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_INDIV-TYPE")]
        public string RI_INDIVTYPE
        {
            get
            {
                return this.rI_INDIVTYPEField;
            }
            set
            {
                this.rI_INDIVTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_NAME-STATUS")]
        public string RI_NAMESTATUS
        {
            get
            {
                return this.rI_NAMESTATUSField;
            }
            set
            {
                this.rI_NAMESTATUSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_INDIV-NAME")]
        public object RI_INDIVNAME
        {
            get
            {
                return this.rI_INDIVNAMEField;
            }
            set
            {
                this.rI_INDIVNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_LAST-NAME")]
        public string RI_LASTNAME
        {
            get
            {
                return this.rI_LASTNAMEField;
            }
            set
            {
                this.rI_LASTNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_FIRST-NAME")]
        public string RI_FIRSTNAME
        {
            get
            {
                return this.rI_FIRSTNAMEField;
            }
            set
            {
                this.rI_FIRSTNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_MIDDLE-INITIAL")]
        public string RI_MIDDLEINITIAL
        {
            get
            {
                return this.rI_MIDDLEINITIALField;
            }
            set
            {
                this.rI_MIDDLEINITIALField = value;
            }
        }

        /// <remarks/>
        public object RI_SUFFIX
        {
            get
            {
                return this.rI_SUFFIXField;
            }
            set
            {
                this.rI_SUFFIXField = value;
            }
        }

        /// <remarks/>
        public string RI_GENDER
        {
            get
            {
                return this.rI_GENDERField;
            }
            set
            {
                this.rI_GENDERField = value;
            }
        }

        /// <remarks/>
        public object RI_FILLER
        {
            get
            {
                return this.rI_FILLERField;
            }
            set
            {
                this.rI_FILLERField = value;
            }
        }

        /// <remarks/>
        public string RI_DOB
        {
            get
            {
                return this.rI_DOBField;
            }
            set
            {
                this.rI_DOBField = value;
            }
        }

        /// <remarks/>
        public string RI_AGE
        {
            get
            {
                return this.rI_AGEField;
            }
            set
            {
                this.rI_AGEField = value;
            }
        }

        /// <remarks/>
        public string RI_SSN
        {
            get
            {
                return this.rI_SSNField;
            }
            set
            {
                this.rI_SSNField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_DL-STATE-CODE")]
        public object RI_DLSTATECODE
        {
            get
            {
                return this.rI_DLSTATECODEField;
            }
            set
            {
                this.rI_DLSTATECODEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_DL-NUMBER")]
        public string RI_DLNUMBER
        {
            get
            {
                return this.rI_DLNUMBERField;
            }
            set
            {
                this.rI_DLNUMBERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_DL-ISSUE-DATE")]
        public object RI_DLISSUEDATE
        {
            get
            {
                return this.rI_DLISSUEDATEField;
            }
            set
            {
                this.rI_DLISSUEDATEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_PHONE-FLAG")]
        public object RI_PHONEFLAG
        {
            get
            {
                return this.rI_PHONEFLAGField;
            }
            set
            {
                this.rI_PHONEFLAGField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_PHONE-NUM")]
        public string RI_PHONENUM
        {
            get
            {
                return this.rI_PHONENUMField;
            }
            set
            {
                this.rI_PHONENUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_SOURCE-OF-INDIV")]
        public string RI_SOURCEOFINDIV
        {
            get
            {
                return this.rI_SOURCEOFINDIVField;
            }
            set
            {
                this.rI_SOURCEOFINDIVField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_GRAD-CENT")]
        public object RI_GRADCENT
        {
            get
            {
                return this.rI_GRADCENTField;
            }
            set
            {
                this.rI_GRADCENTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RI_GRAD-YEAR")]
        public object RI_GRADYEAR
        {
            get
            {
                return this.rI_GRADYEARField;
            }
            set
            {
                this.rI_GRADYEARField = value;
            }
        }

        /// <remarks/>
        public object RI_FILLER2
        {
            get
            {
                return this.rI_FILLER2Field;
            }
            set
            {
                this.rI_FILLER2Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataConditionalOrdering
    {

        private ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummary responseSummaryField;

        private ISOPassportSvcRsReportReportDataConditionalOrderingProductOrder[] transactionHistoryField;

        /// <remarks/>
        public ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummary ResponseSummary
        {
            get
            {
                return this.responseSummaryField;
            }
            set
            {
                this.responseSummaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ProductOrder", IsNullable = false)]
        public ISOPassportSvcRsReportReportDataConditionalOrderingProductOrder[] TransactionHistory
        {
            get
            {
                return this.transactionHistoryField;
            }
            set
            {
                this.transactionHistoryField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummary
    {

        private ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS[] vINSField;

        private ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER[] dRIVERField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("VINS")]
        public ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS[] VINS
        {
            get
            {
                return this.vINSField;
            }
            set
            {
                this.vINSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DRIVER")]
        public ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER[] DRIVER
        {
            get
            {
                return this.dRIVERField;
            }
            set
            {
                this.dRIVERField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryVINS
    {

        private string vINField;

        private string yearField;

        private string makeField;

        private string modelField;

        private string sourceField;

        /// <remarks/>
        public string VIN
        {
            get
            {
                return this.vINField;
            }
            set
            {
                this.vINField = value;
            }
        }

        /// <remarks/>
        public string Year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
            }
        }

        /// <remarks/>
        public string Make
        {
            get
            {
                return this.makeField;
            }
            set
            {
                this.makeField = value;
            }
        }

        /// <remarks/>
        public string Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        public string Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataConditionalOrderingResponseSummaryDRIVER
    {

        private string firstNameField;

        private string middleNameField;

        private string lastNameField;

        private string dLNumberField;

        private string dLStateField;

        private string dateOfBirthField;

        private string sSNField;

        private string sourceField;

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }

        /// <remarks/>
        public string DLNumber
        {
            get
            {
                return this.dLNumberField;
            }
            set
            {
                this.dLNumberField = value;
            }
        }

        /// <remarks/>
        public string DLState
        {
            get
            {
                return this.dLStateField;
            }
            set
            {
                this.dLStateField = value;
            }
        }

        /// <remarks/>
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }

        /// <remarks/>
        public string SSN
        {
            get
            {
                return this.sSNField;
            }
            set
            {
                this.sSNField = value;
            }
        }

        /// <remarks/>
        public string Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ISOPassportSvcRsReportReportDataConditionalOrderingProductOrder
    {

        private string productCdField;

        private string statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string productCd
        {
            get
            {
                return this.productCdField;
            }
            set
            {
                this.productCdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }
}
