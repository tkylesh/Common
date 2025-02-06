using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon.Dto
{
    public class VehicleRiskResultsDto
    {
        public string Type { get; set; }
        public List<VehicleRiskDto> VehicleRiskResults { get; set; }
        public string ErrorMessage { get; set; }
    }

}
