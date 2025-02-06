using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon.Dto
{
    public class VINRequestDto
    {
        public string RequestState { get; set; }
        public List<VINItem> vins { get; set; }
    }
    public class VINItem
    {
        public string vin { get; set; }
    }
}
