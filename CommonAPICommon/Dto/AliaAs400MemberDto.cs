using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon.Dto
{
    public class AliaAs400MemberDto
    {
        public string MembershipNumber { get; set; }
        public string PolicyNumber { get; set; }
    }

    public class ALDto
    {
        public string Status { get; set; }
        public string ExpirationDate { get; set; }
    }

    public class ALResults
    {
        public ALDto ALDto { get; set; }
        public string ErrorMessage { get; set; }
    }
}
