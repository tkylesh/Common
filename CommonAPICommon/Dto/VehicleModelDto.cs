using System.Collections.Generic;

namespace CommonAPICommon.Dto
{
    public class VehicleModelDto
    {
        public int MakeId { get; set; }
        public string ShortModelName { get; set; }
        public string FullModelName { get; set; }
        public List<VehicleSubModelDto> SubModels { get; set; }
    }
}
