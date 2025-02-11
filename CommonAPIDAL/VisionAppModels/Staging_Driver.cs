﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonAPIDAL.VisionAppModels;

[Table("Staging_Driver", Schema = "dbo")]
[Index("DrvNum", "DrvFirstName", "DrvLastName", "LicenseState", Name = "IXNC_SD_DrvLicDob")]
[Index("CustomaryOpSvId", Name = "IX_Staging_Driver_CustomaryOpSvId")]
[Index("QuoteID", "sdID", "MaritalStatusID", "DrvTypeID", "LicenseStatusID", Name = "IX_Staging_Driver_QuoteID")]
[Index("QuoteID", "DrvNum", "AS400DriverNumber", Name = "IX_Staging_Driver_QuoteID_AS400DrNum_DrvNum")]
[Index("LicenseStatusID", Name = "IxFK_Staging_Driver_LicenseStatus")]
public partial class Staging_Driver
{
    [Key]
    public int sdID { get; set; }

    public int QuoteID { get; set; }

    public byte DrvNum { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string DrvFirstName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string DrvMidName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string DrvLastName { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string DrvSuffixName { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; }

    public int? MaritalStatusID { get; set; }

    public int? RelationID { get; set; }

    /// <summary>
    /// Principal, Excluded, Occasional, etc
    /// </summary>
    public int? DrvTypeID { get; set; }

    public bool? SR22 { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string SR22State { get; set; }

    public int? SR22TypeID { get; set; }

    public int? LicenseStatusID { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string LicenseState { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DateLicensed { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string MVROrdered { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string DrvPoints { get; set; }

    public byte? RatedAge { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Occupation { get; set; }

    public bool RateReady { get; set; }

    public bool MedicalCert { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string EndorsementAction { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string FilingCaseNum { get; set; }

    public bool IsDirty { get; set; }

    public bool? SR50 { get; set; }

    public byte[] SSNEncrypted { get; set; }

    public byte[] LicenseNumEncrypted { get; set; }

    public byte[] DOBEncrypted { get; set; }

    public int? ListOnlyTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ListOnlyOtherInsurance { get; set; }

    public int? AS400DriverNumber { get; set; }

    public int? CustomaryOpSvId { get; set; }

    public int? ForeignLicenseTypeID { get; set; }

    [ForeignKey("QuoteID")]
    [InverseProperty("Staging_Driver")]
    public virtual Staging_Policy Quote { get; set; }
}