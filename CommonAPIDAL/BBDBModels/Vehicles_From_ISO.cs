﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonAPIDAL.BBDBModels;

[Table("Vehicles_From_ISO", Schema = "dbo")]
[Index("quoteID", Name = "IX_Vehicles_From_ISO_quoteID")]
public partial class Vehicles_From_ISO
{
    [Key]
    public int rowID { get; set; }

    public int ISOMasterID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime insertDate { get; set; }

    public int quoteID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string VIN { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string ModelYear { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Make { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ShortModel { get; set; }

    public int? VehicleCount { get; set; }

    public int? LienHolderSequenceNumber { get; set; }

    [StringLength(50)]
    public string LienHolderName { get; set; }

    [StringLength(50)]
    public string LienHolderStreetAddress { get; set; }

    [StringLength(50)]
    public string LienHolderCity { get; set; }

    [StringLength(50)]
    public string LienHolderState { get; set; }

    [StringLength(50)]
    public string LienHolderZipCode { get; set; }
}