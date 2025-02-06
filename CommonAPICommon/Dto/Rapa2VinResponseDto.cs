using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon.Dto
{
    public class Rapa2VinResponseDto
    {
        public Header Header { get; set; }
        public Body[] Body { get; set; }
    }

    public class Header
    {
        public string Quoteback { get; set; }
        public string TransactionId { get; set; }
    }

    public class Body
    {
        public string message { get; set; }
        public string details { get; set; }
        public Vehicle Vehicle { get; set; }
        public Physicaldamage PhysicalDamage { get; set; }
        public Liability Liability { get; set; }
    }

    public class Vehicle
    {
        public string VIN { get; set; }
        public string ModelYear { get; set; }
        public string BaseMSRP { get; set; }
        public string SpecialHandlingIndicator { get; set; }
        public string InterimIndicator { get; set; }
        public string SpecialInfoSelector { get; set; }
        public string ModelSeriesInfo { get; set; }
        public string BodyInfo { get; set; }
        public string EngineInfo { get; set; }
        public string RestraintInfo { get; set; }
        public string TransmissionInfo { get; set; }
        public string OtherInfo { get; set; }
        public string ReleaseDate { get; set; }
        public string VINChangeIndicator { get; set; }
        public string DistributionDate { get; set; }
        public string Restraint { get; set; }
        public string AntiLockBrakes { get; set; }
        public string EngineCylinders { get; set; }
        public string EngineType { get; set; }
        public string FieldChangeIndicator { get; set; }
        public string Make { get; set; }
        public string BasicModelName { get; set; }
        public string BodyStyle { get; set; }
        public string EngineSize { get; set; }
        public string FourWheelDriveIndicator { get; set; }
        public string ElectronicStabilityControl { get; set; }
        public string TonnageIndicator { get; set; }
        public string PayloadCapacity { get; set; }
        public string FullModelName { get; set; }
        public string DaytimeRunningLightIndicator { get; set; }
        public string Wheelbase { get; set; }
        public string ClassCode { get; set; }
        public string AntiTheftIndicator { get; set; }
        public string CurbWeight { get; set; }
        public string GrossVehicleWeight { get; set; }
        public string Height { get; set; }
        public string Horsepower { get; set; }
        public string StateException { get; set; }
        public string VMPerformanceIndicator { get; set; }
        public string NCICCode { get; set; }
        public string Chassis { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
    }

    public class Physicaldamage
    {
        public string StateCode { get; set; }
        public string RiskAnalyzerCollisionIndicatedSymbolRelativityChar1 { get; set; }
        public string RiskAnalyzerComprehensiveIndicatedSymbolRelativityChar1 { get; set; }
        public string RiskAnalyzerCollisionIndicatedSymbolRelativityChar2 { get; set; }
        public string RiskAnalyzerComprehensiveIndicatedSymbolRelativityChar2 { get; set; }
        public string RiskAnalyzerCollisionRatingSymbolRelativity { get; set; }
        public string RiskAnalyzerCollisionRatingSymbol { get; set; }
        public string RiskAnalyzerComprehensiveRatingSymbolRelativity { get; set; }
        public string RiskAnalyzerComprehensiveRatingSymbol { get; set; }
        public string RiskAnalyzerComprehensiveNonGlassRatingSymbolRelativity { get; set; }
        public string RiskAnalyzerComprehensiveNonGlassRatingSymbol { get; set; }
        public string RiskAnalyzerCollisionCappingIndicator { get; set; }
        public string RiskAnalyzerComprehensiveCappingIndicator { get; set; }
        public string RiskAnalyzerComprehensiveNonGlassCappingIndicator { get; set; }
        public string RiskAnalyzerComprehensiveNonGlassIndicatedSymbolRelativityChar2 { get; set; }
        public string RiskAnalyzerCollisionIndicatedSymbol { get; set; }
        public string RiskAnalyzerComprehensiveIndicatedSymbol { get; set; }
        public string RiskAnalyzerCollisionIndicatedSymbolRelativity { get; set; }
        public string RiskAnalyzerComprehensiveIndicatedSymbolRelativity { get; set; }
        public string RiskAnalyzerComprehensiveNonGlassIndicatedSymbolRelativityChar1 { get; set; }
        public string RiskAnalyzerComprehensiveNonGlassIndicatedSymbol { get; set; }
        public string RiskAnalyzerComprehensiveNonGlassIndicatedSymbolRelativity { get; set; }
    }

    public class Liability
    {
        public string StateCode { get; set; }
        public string RiskAnalyzerBodilyInjuryIndicatedSymbolRelativityChar1 { get; set; }
        public string RiskAnalyzerPropertyDamageIndicatedSymbolRelativityChar1 { get; set; }
        public string RiskAnalyzerMedicalPaymentsIndicatedSymbolRelativityChar1 { get; set; }
        public string RiskAnalyzerPersonalInjuryProtectionIndicatedSymbolRelativityChar1 { get; set; }
        public string RiskAnalyzerSingleLimitIndicatedSymbolRelativityChar1 { get; set; }
        public string RiskAnalyzerBodilyInjuryIndicatedSymbolRelativityChar2 { get; set; }
        public string RiskAnalyzerPropertyDamageIndicatedSymbolRelativityChar2 { get; set; }
        public string RiskAnalyzerMedicalPaymentsIndicatedSymbolRelativityChar2 { get; set; }
        public string RiskAnalyzerPersonalInjuryProtectionIndicatedSymbolRelativityChar2 { get; set; }
        public string RiskAnalyzerSingleLimitIndicatedSymbolRelativityChar2 { get; set; }
        public string RiskAnalyzerBodilyInjuryIndicatedSymbol { get; set; }
        public string RiskAnalyzerPropertyDamageIndicatedSymbol { get; set; }
        public string RiskAnalyzerMedicalPaymentsIndicatedSymbol { get; set; }
        public string RiskAnalyzerBodilyInjuryRatingSymbolRelativity { get; set; }
        public string RiskAnalyzerBodilyInjuryRatingSymbol { get; set; }
        public string RiskAnalyzerPropertyDamageRatingSymbolRelativity { get; set; }
        public string RiskAnalyzerPropertyDamageRatingSymbol { get; set; }
        public string RiskAnalyzerMedicalPaymentsRatingSymbolRelativity { get; set; }
        public string RiskAnalyzerMedicalPaymentsRatingSymbol { get; set; }
        public string RiskAnalyzerPersonalInjuryProtectionRatingSymbolRelativity { get; set; }
        public string RiskAnalyzerPersonalInjuryProtectionRatingSymbol { get; set; }
        public string RiskAnalyzerSingleLimitRatingSymbolRelativity { get; set; }
        public string RiskAnalyzerSingleLimitRatingSymbol { get; set; }
        public string RiskAnalyzerBodilyInjuryCappingIndicator { get; set; }
        public string RiskAnalyzerPropertyDamageCappingIndicator { get; set; }
        public string RiskAnalyzerMedicalPaymentsCappingIndicator { get; set; }
        public string RiskAnalyzerPersonalInjuryProtectionCappingIndicator { get; set; }
        public string RiskAnalyzerSingleLimitCappingIndicator { get; set; }
        public string RiskAnalyzerPersonalInjuryProtectionIndicatedSymbol { get; set; }
        public string RiskAnalyzerSingleLimitIndicatedSymbol { get; set; }
        public string RiskAnalyzerBodilyInjuryIndicatedSymbolRelativity { get; set; }
        public string RiskAnalyzerPropertyDamageIndicatedSymbolRelativity { get; set; }
        public string RiskAnalyzerMedicalPaymentsIndicatedSymbolRelativity { get; set; }
        public string RiskAnalyzerPersonalInjuryProtectionIndicatedSymbolRelativity { get; set; }
        public string RiskAnalyzerSingleLimitIndicatedSymbolRelativity { get; set; }
    }
}
