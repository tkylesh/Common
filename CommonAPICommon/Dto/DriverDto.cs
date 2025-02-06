using System;

namespace CommonAPICommon.Dto
{
    public class DriverDto
  {
    public int sdID { get; set; }
    public int tempId { get; set; }
    public int QuoteId { get; set; }
    public string DrvFirstName { get; set; }
    public string DrvLastName { get; set; }
    public string SSN { get; set; }
    public DateTime DOB { get; set; }
    public string LicenseState { get; set; }
    public string LicenseNum { get; set; }
  }
}
