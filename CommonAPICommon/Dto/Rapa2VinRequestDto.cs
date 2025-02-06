using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon.Dto
{
    public class Rapa2VinRequestDto
    {
        public RequestHeader Header { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestHeader
    {
        public Authorization Authorization { get; set; }
        public string QuoteBack { get; set; }
    }

    public class Authorization
    {
        public string OrgID { get; set; }
        public string ShipId { get; set; }
    }

    public class RequestBody
    {
        public string StateCode { get; set; }
        public string VIN { get; set; }
        public string ModelYear { get; set; }
        public string Make { get; set; }
        public string FullModelName { get; set; }
        public string BasicModelName { get; set; }
        public string EngineCylinders { get; set; }
        public string FourWheelDriveIndicator { get; set; }
        public string BodyStyle { get; set; }
        public string Exclusion { get; set; }
        public string DistributionDate { get; set; }
        public string EffectiveDate { get; set; }
    }
}
