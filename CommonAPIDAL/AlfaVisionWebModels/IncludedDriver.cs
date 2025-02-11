﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonAPIDAL.AlfaVisionWebModels;

[Table("IncludedDriver", Schema = "dbo")]
public partial class IncludedDriver
{
    [Key]
    public int incDrvID { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string PolicyNo { get; set; }

    [Column(TypeName = "numeric(5, 0)")]
    public decimal SeqNo { get; set; }

    [Column(TypeName = "numeric(5, 0)")]
    public decimal EndNo { get; set; }

    [Column(TypeName = "numeric(5, 0)")]
    public decimal CarNo { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string SubNo { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string DriverName { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string DriverAge { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string DriverSex { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string DriverMaritalStatus { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string DriverLicenseState { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string DriverLiabilityPoints { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string DriverPhysicalDamagePoints { get; set; }

    [Column(TypeName = "numeric(5, 0)")]
    public decimal? DriverNoOfChargeableAccidents { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string DriverNoOfMajors { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string DriverNoOfMinors { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string DriverYrsDrivingExpTotal { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string DriverSRFilingType { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string DriverOccupation { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string SafetyDiscount { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string TrainingDiscount { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string GoodDriverDiscount { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string SeniorMatureDriverDiscount { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string DriverDiscountAlpha { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string GoodStudent { get; set; }

    public byte[] DriverLicenseNoEncrypted { get; set; }

    public byte[] DriverDateOfBirthEncrypted { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string DriverType { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DriverEffectiveDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DriverDeletedDate { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string DriverStatus { get; set; }

    public byte? VehicleCustomarilyOperated { get; set; }
}