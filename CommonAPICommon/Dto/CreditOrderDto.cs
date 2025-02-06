using System;

namespace CommonAPICommon.Dto
{
    public class CreditOrderDto
    {
        public int RMID { get; set; }
        public int QuoteId { get; set; }
        public int ClientId { get; set; }
        public DateTime OrderDate { get; set; }
        public string NamePrefix { get; set; }
        public string NameFirst { get; set; }
        public string NameMiddle { get; set; }
        public string NameLast { get; set; }
        public string NameSuffix { get; set; }
        public DateTime DOB { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string SSN { get; set; }
        public string HouseNumber { get; set; }
        public string StreetName { get; set; }
        public string ApartmentNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ZipPlus4 { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseState { get; set; }
        public string ProductArray { get { return "1"; } }// do not want this to be anything but a 1.
        public string CPSuffix { get; set; }
        public string AccountNumber { get; set; }
        public int DevEnv { get; set; }

    }
}
