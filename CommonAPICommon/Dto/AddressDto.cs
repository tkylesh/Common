namespace CommonAPICommon.Dto
{
    public class AddressDto
  {
    public bool AddressValid { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string QASVerifyLevel { get; set; }
    public PicklistDto PickList { get; set; }
    //public DateTime EffectiveDate { get; set; }
  }
}
