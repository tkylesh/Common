﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonAPIDAL.VisionAppModels;

[Table("VinrapaLiabilitySupplemental", Schema = "dbo")]
[Index("ModelYear", Name = "IXNC_VinrapaLiabilitySupplemental_ModelYear")]
[Index("ModelYear", "MakeID", Name = "IX_ModelYear_MakeID")]
[Index("VIN", Name = "IX_VIN")]
public partial class VinrapaLiabilitySupplemental
{
    [Key]
    public int vinraLiabID { get; set; }

    public int? vinraID { get; set; }

    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string VIN { get; set; }

    [Required]
    [StringLength(6)]
    [Unicode(false)]
    public string ISONum { get; set; }

    public short ModelYear { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string VINChangeInd { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? EffDate { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string RestraintInd { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string AntiLockInd { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Cylinders { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string EngineType { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string FieldChangeInd { get; set; }

    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string MakeCode { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string ShortModelName { get; set; }

    [StringLength(8)]
    [Unicode(false)]
    public string BodyStyle { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string EngineSize { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string FourWheelDriveInd { get; set; }

    [StringLength(39)]
    [Unicode(false)]
    public string FullModelName { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string SpecialInfoSelector { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string ModelInfo { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string BodyInfo { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string EngineInfo { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string RestraintInfo { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string TransmissionInfo { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string NotOtherwiseClassifiedInfo { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string DayTimeRunningLightInd { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string WheelbaseInfo { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string ClassCode { get; set; }

    public int? MakeID { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string NewPriceSymbol { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string BiSymbol { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string PDSymbol { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string MedPaySymbol { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string PIPSymbol { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string NewPriceSymbolGA { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string BiSymbolGA { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string PDSymbolGA { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string MedPaySymbolGA { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string PIPSymbolGA { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string CurbWeight { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string GrossVehicleWeight { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string Height { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string HorsePower { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string RapaSupplemental { get; set; }
}