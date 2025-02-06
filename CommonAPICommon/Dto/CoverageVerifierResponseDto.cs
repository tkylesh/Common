using System;

namespace CommonAPICommon.Dto
{
    public class CoverageVerifierResponseDto
    {
        public int LapseDays { get; set; }
        public int LapseDaysLimit { get; set; }
        public DateTime LastTermExpirationDate { get; set; }
        public bool PolicyIsStandard { get; set; }
        public int IndividualLimit { get; set; }
        public int OccurranceLimit { get; set; }
        public string PolicyState { get; set; }
        public bool PriorWasAvic { get; set; }
        public bool PriorWasAsic { get; set; }
        public bool PriorWasHS { get; set; }
    }
}
