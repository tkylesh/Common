using System.Collections.Generic;

namespace CommonAPICommon.Dto
{
    public class VINResultDto
    {
        public bool ISODirectMatch { get; set; }
        public bool ThirdPartyCalled { get; set; }
        public List<VINItemDto> VinItems { get; set; }
        public string ErrorMessage { get; set; }
        public bool Supplemental { get; set; }
    }
}
