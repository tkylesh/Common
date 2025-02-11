﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonAPIDAL.VisionAppModels;

[Table("VinLiabilitySymbol", Schema = "dbo")]
[Index("VIN", Name = "IX_VIN")]
public partial class VinLiabilitySymbol
{
    [Key]
    public int vinBIID { get; set; }

    public short ModelYear { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string VinChangeInd { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string FieldChangeInd { get; set; }

    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string MakeCode { get; set; }

    [StringLength(8)]
    [Unicode(false)]
    public string BodyStyle { get; set; }

    [StringLength(39)]
    [Unicode(false)]
    public string FullModelName { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string SpecialInfoSelector { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string WheelBaseInfo { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string ClassCode { get; set; }

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

    [Column(TypeName = "smalldatetime")]
    public DateTime? EffDate { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string LiabilitySymbol { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string PIPSymbol { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string FilingDate { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string VIN { get; set; }
}