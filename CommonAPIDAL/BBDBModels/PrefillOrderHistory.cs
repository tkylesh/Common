﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonAPIDAL.BBDBModels;

[Keyless]
[Table("PrefillOrderHistory", Schema = "dbo")]
public partial class PrefillOrderHistory
{
    public int ISOMasterID { get; set; }

    public int QuoteID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CallDate { get; set; }
}