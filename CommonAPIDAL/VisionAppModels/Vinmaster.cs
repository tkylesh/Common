﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonAPIDAL.VisionAppModels;

[Table("Vinmaster", Schema = "dbo")]
[Index("ManufacturerID", Name = "IX_Vinmaster_ManufacturerID")]
public partial class Vinmaster
{
    [Key]
    public int vinID { get; set; }

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

    [StringLength(2)]
    [Unicode(false)]
    public string StateException { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string SymbolChangeInd { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string VSRSymbol { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string NonVSRSymbol { get; set; }

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

    [StringLength(1)]
    [Unicode(false)]
    public string CountrywidePerformance { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string NonVSRPerformance { get; set; }

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

    [StringLength(1)]
    [Unicode(false)]
    public string NCIC_Manufacturer { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string CircularNumber { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string ISOSymbol { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string NonVSRSymbol_2Digit { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string WheelbaseInfo { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string ClassCode { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string AntiTheftInd { get; set; }

    public int? MakeID { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string DistinguishingInfo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string EngineSummary { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string CompSymbol { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string CollSymbol { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string NewPriceSymbol { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string ManufacturerID { get; set; }
}