using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon.Dto
{
    public class VehicleRiskDto
    {
        public string Results { get; set; }
        public string EffectiveEndDate { get; set; }
        public string VIN { get; set; }
        public string ErrorMessage { get; set; }
    }

}
