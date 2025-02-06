using System.Collections.Generic;

namespace CommonAPICommon.Dto
{
    public class PrefillResponseDto
    {
        public IList<PrefillDriverDto> PrefillDrivers { get; set; }
        public IList<PrefillVehicleDto> PrefillVehicles { get; set; }
    }
}
