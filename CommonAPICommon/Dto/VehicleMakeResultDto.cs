using System.Collections.Generic;

namespace CommonAPICommon.Dto
{
    public class VehicleMakeResultDto
    {
        public int PassedModelYear { get; set; }
        public List<VehicleMakeDto> Makes { get; set; }
        public string ErrorMessage { get; set; } 
        public bool Supplemental { get; set; }
    }
}
