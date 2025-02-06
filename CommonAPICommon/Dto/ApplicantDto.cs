using System;

namespace CommonAPICommon.Dto
{
    public class ApplicantDto
  {
    public int apID { get; set; }
    public int QuoteID { get; set; }
    public short ApplicantNum { get; set; }
    public string InsFirstName { get; set; }
    public string InsMidName { get; set; }
    public string InsLastName { get; set; }
    public string InsSuffixName { get; set; }
    public DateTime DOB { get; set; }
    public string InsAddress1 { get; set; }
    public string InsAddress2 { get; set; }
    public string InsCity { get; set; }
    public string InsState { get; set; }
    public string InsZip { get; set; }
    public string InsCounty { get; set; }
    public string InsPhone1 { get; set; }
    public string InsPhone2 { get; set; }
    public string InsEmail { get; set; }
    public string TaxJurisdiction { get; set; }
    public string GarAddress { get; set; }
    public string GarAddress2 { get; set; }
    public string GarCity { get; set; }
    public string GarState { get; set; }
    public string GarZip { get; set; }
    public string GarCounty { get; set; }
    public string GarTerritory { get; set; }
  }
}
