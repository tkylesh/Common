﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonAPIDAL.VisionAppModels;

[Table("ProgramMaster", Schema = "dbo")]
public partial class ProgramMaster
{
    [Key]
    public int ProgramID { get; set; }

    [Required]
    [StringLength(2)]
    [Unicode(false)]
    public string State { get; set; }

    public short? CarrierID { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string PolType { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string ProgramDesc { get; set; }

    public bool UsesCreditReport { get; set; }

    [Required]
    [StringLength(50)]
    public string CurrentCarrierStorage { get; set; }

    public int? PaymentProcessor { get; set; }

    [Column(TypeName = "money")]
    public decimal? FeeExternal { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string OneIncAuthKey { get; set; }

    public int? PrefillSupplierID { get; set; }

    public int? CVLapsedDaysLimit { get; set; }

    public int? CVDayCountLimit { get; set; }

    public int? ClueSupplierID { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string LexisNexisPrefillStateAcctNbr { get; set; }

    public int? ClueClaimDaysLimit { get; set; }

    [InverseProperty("Program")]
    public virtual ICollection<ProgramRevision> ProgramRevision { get; set; } = new List<ProgramRevision>();
}