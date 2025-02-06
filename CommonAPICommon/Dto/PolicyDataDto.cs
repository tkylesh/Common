using System;
using System.Collections.Generic;

namespace CommonAPICommon.Dto
{
    public class PolicyDataDto
    {
        public int Id { get; set; }
        public int ISOMasterId { get; set; }
        public string CarrierName { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyStatus { get; set; }
        public DateTime InceptionDate { get; set; }
        public DateTime LastReportedTermEffectiveDate { get; set; }
        public DateTime LastReportedTermExpirationDate { get; set; }
        public int NumberOfCancellations { get; set; }
        public int NumberOfRenewals { get; set; }
        public int BIIndividualLimit { get; set; }
        public int BIOccuranceLimit { get; set; }
        public int CSILimit { get; set; }
        public DateTime CoverageFromDate { get; set; }
        public DateTime CoverageToDate { get; set; }
        public DateTime ReportAsOfDate { get; set; }
        public bool IsStandardPolicy { get; set; }
        public string PolicyType { get; set; }
        public string PolicyState { get; set; }
        public DateTime? LastCancelDate { get; set; }
        public string CancelReason { get; set; }
        public string NAIC { get; set; }
        public string PolicyHolderName { get; set; }
        public IList<PolicySubjectDto> PolicySubjects { get; set; }
    }
}
