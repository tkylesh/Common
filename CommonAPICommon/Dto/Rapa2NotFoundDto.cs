using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon.Dto
{
    public class Rapa2NotFoundDto
    {
        public NFHeader Header { get; set; }
        public NFBody Body { get; set; }
    }

    public class NFHeader
    {
        public string Quoteback { get; set; }
        public string TransactionId { get; set; }
    }

    public class NFBody
    {
        public string message { get; set; }
        public string details { get; set; }
    }

}
