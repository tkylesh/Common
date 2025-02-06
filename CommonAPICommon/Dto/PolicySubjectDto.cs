using System;
using System.Collections.Generic;

namespace CommonAPICommon.Dto
{
    public class PolicySubjectDto
    {
        public int Id { get; set; }
        public int PolicyId { get; set; }
        public string Name { get; set; }
        public string RelationCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string RelationCodeDesc { get; set; }
        public string SubjectId { get; set; }
        public IList<PolicySubjectHistoryDto> PolicySubjectHistories { get; set; }
    }
}
