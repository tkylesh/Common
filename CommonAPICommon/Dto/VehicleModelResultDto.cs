using System.Collections.Generic;

namespace CommonAPICommon.Dto
{
    public class VehicleModelResultDto
    {
        public int PassedModelYear { get; set; }
        public int PassedMakeId { get; set; }
        public List<VehicleModelDto> Models { get; set; }
        public string ErrorMessage { get; set; }

        public bool Supplemental { get; set; }
    }
}
