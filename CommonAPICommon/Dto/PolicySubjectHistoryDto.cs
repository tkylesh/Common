using System;

namespace CommonAPICommon.Dto
{
    public class PolicySubjectHistoryDto
    {
        public int Id { get; set; }
        public int PolicySubjectId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
