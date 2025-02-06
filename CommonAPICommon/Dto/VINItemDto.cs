using System;

namespace CommonAPICommon.Dto
{
    public class VINItemDto
    {
        public string PassedVIN { get; set; }
        public string MatchedVIN { get; set; }
        public int ModelYear { get; set; }
        public DateTime EffDate { get; set; }
        public string MakeCode { get; set; }
        public int MakeId { get; set; }
        public string Make { get; set; }
        public string ShortModel { get; set; }
        public string FullModel { get; set; }
        public string ISOSymbol { get; set; }
        public string CompSymbol { get; set; }
        public string CollSymbol { get; set; }
        public int VinId { get; set; }
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
        public string BodyStyle { get; set; }
        public string BodyStyleDesc { get; set; }
        public string TransmissionInfo { get; set; }
        public string StateException { get; set; }
        public string NCIC_Manufacturer { get; set; }
        public string SpecialInfoSelector { get; set; }
        public string LiabilitySymbol { get; set; }
        public string BaseMSRP { get; set; }
        public string GrossVehicleWeight { get; set; }
        public string UnacceptableVehicleReason { get; set; }
        public int seq { get; set; }
    }
}
