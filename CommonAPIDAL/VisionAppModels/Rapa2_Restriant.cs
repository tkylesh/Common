﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonAPIDAL.VisionAppModels;

[Table("Rapa2_Restriant", Schema = "dbo")]
public partial class Rapa2_Restriant
{
    [Key]
    public int RestriantId { get; set; }

    [Required]
    [StringLength(4)]
    public string RestriantCode { get; set; }

    [Required]
    [StringLength(100)]
    public string RestriantDesc { get; set; }
}