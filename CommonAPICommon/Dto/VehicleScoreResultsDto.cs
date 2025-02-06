using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon.Dto
{
    public class VehicleScoreResultsDto
    {
        public List<VehicleScoreDto> VehicleScoreResults { get; set; }
        public string ErrorMessage { get; set; }
    }

}
