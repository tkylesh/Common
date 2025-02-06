using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon.Dto
{
    public class VINAS400RequestDto
    {
        public string PolicyNumber { get; set; }
        public List<VINItem> vins { get; set; }
    }
}
