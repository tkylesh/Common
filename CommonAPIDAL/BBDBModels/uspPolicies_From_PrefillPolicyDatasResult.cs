﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonAPIDAL.BBDBModels
{
    public partial class uspPolicies_From_PrefillPolicyDatasResult
    {
        public int QuoteID { get; set; }
        public int? IsoMasterId { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyType { get; set; }
        public string PolicyStatus { get; set; }
        public string CarrierName { get; set; }
        public string AmbestNumber { get; set; }
        public DateTime InceptionDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastCancelDate { get; set; }
        public string Occurences { get; set; }
        public string SubjectUnitNumber { get; set; }
        public string PolicyHolderName { get; set; }
        public string PolicyHolderRelationship { get; set; }
        public DateTime PolicyFromDate { get; set; }
        public DateTime PolicyToDate { get; set; }
    }
}
