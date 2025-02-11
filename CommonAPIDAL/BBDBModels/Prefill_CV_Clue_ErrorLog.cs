﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonAPIDAL.BBDBModels;

[Keyless]
[Table("Prefill_CV_Clue_ErrorLog", Schema = "dbo")]
public partial class Prefill_CV_Clue_ErrorLog
{
    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string TransactionID { get; set; }

    public int? QuoteId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ErrorDate { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Process { get; set; }

    [Required]
    [StringLength(250)]
    [Unicode(false)]
    public string ErrorMessage { get; set; }

    [Unicode(false)]
    public string StackTrace { get; set; }
}