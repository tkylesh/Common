﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonAPIDAL.VisionAppModels;

[Table("VIN_Make", Schema = "dbo")]
public partial class VIN_Make
{
    [Key]
    public int MakeID { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string MakeAbbr { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Make { get; set; }
}