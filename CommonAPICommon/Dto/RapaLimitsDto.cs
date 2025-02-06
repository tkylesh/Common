using System.Collections.Generic;

namespace CommonAPICommon.Dto
{
    public class Rapa2LimitsDto
    {
        public int Id { get; set; }
        public string State { get; set; }
        public int MSRPLimit { get; set; }
        public int WeightLimit { get; set; }
        public bool UseCappedSymbols { get; set; }

    }
}
