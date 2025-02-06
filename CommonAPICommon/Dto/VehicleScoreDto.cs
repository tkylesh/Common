using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon.Dto
{
    public class VehicleScoreDto
    {
        public string VehicleId { get; set; }
        public string Score { get; set; }
        public int hdrId { get; set; }
        public string ErrorMessage { get; set; }
        public string ScoreDate { get; set; }
    }
}
