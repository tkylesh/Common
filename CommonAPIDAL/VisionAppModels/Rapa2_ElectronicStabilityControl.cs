﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonAPIDAL.VisionAppModels;

[Table("Rapa2_ElectronicStabilityControl", Schema = "dbo")]
public partial class Rapa2_ElectronicStabilityControl
{
    [Key]
    public int escId { get; set; }

    [Required]
    [StringLength(8)]
    public string ElectronicStabilityControlCode { get; set; }

    [Required]
    [StringLength(100)]
    public string ElectronicStabilityControlDesc { get; set; }
}