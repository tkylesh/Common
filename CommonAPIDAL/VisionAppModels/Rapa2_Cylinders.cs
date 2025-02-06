﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonAPIDAL.VisionAppModels;

[Table("Rapa2_Cylinders", Schema = "dbo")]
public partial class Rapa2_Cylinders
{
    [Key]
    public int cyId { get; set; }

    [Required]
    [StringLength(8)]
    public string CylinderCode { get; set; }

    [Required]
    [StringLength(200)]
    public string CylinderDesc { get; set; }
}