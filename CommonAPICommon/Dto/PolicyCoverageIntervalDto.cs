using System;

namespace CommonAPICommon.Dto
{
    public class PolicyCoverageIntervalDto
    {
        public int Id { get; set; }
        public int ISOMasterId { get; set; }
        public string Company { get; set; }
        public string AMBest { get; set; }
        public DateTime InceptionDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int CoverageDays { get; set; }
        public string BreakFromPrior { get; set; }
        public int LapsedDays { get; set; }
        public string LapseReason { get; set; }
        public string PolicyHolderName { get; set; }
        public string PolicyHolderRelationship { get; set; }
        public string SubjectUnitNumber { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyStatus { get; set; }
    }
}
