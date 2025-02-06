using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon.Dto
{
    public class CoverageVerifierDto
    {
        public ApplicantDto Applicant { get; set; }
        public DateTime EffDate { get; set; }
        public int CarrierId { get; set; }
    }
}
