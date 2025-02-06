using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon.Dto
{
    public class VINMasterWithModelDto
    {
        public int VinID { get; set; }
        public string VIN { get; set; }
        public int ModelYear { get; set; }
        public DateTime? EffDate { get; set; }
        public string MakeCode { get; set; }
        public int? MakeID { get; set; }
        public string Make { get; set; }
        public string MakeAbbr { get; set; }
        public string ShortModel { get; set; }
        public string FullModel { get; set; }
        public string ISOSymbol { get; set; }
        public string CompSymbol { get; set; }
        public string CollSymbol { get; set; }
        public string ModelInfo { get; set; }
        public string ModelInfoDescription { get; set; }
        public string BodyStyle { get; set; }
        public string BodyStyleDesc { get; set; }
        public string Horsepower { get; set; }
        public string DistinguishingInfo { get; set; }
        public string BiSymbol { get; set; }
        public string PDSymbol { get; set; }
        public string MedPaySymbol { get; set; }
        public string PIPSymbol { get; set; }
        public string RestraintInd { get; set; }
        public string AntiTheftInd { get; set; }
        public string FourWheelDriveInd { get; set; }
        public string ISONumber { get; set; }
        public string Cylinders { get; set; }
        public string EngineType { get; set; }
        public string EngineSize { get; set; }
        public string EngineInfo { get; set; }
        public string ClassCode { get; set; }
        public string DaytimeRunningLightsInd { get; set; }
        public string AntiLockInd { get; set; }
        public string WheelbaseInfo { get; set; }
        public string TransmissionInfo { get; set; }
        public string StateException { get; set; }
        public string NCIC_Manufacturer { get; set; }
        public string SpecialInfoSelector { get; set; }
        public string LiabilitySymbol { get; set; }

        public short? PayloadCapacity { get; set; }
        public string TonnageInd { get; set; }

        public bool Supplemental { get; set; }

    }
}
