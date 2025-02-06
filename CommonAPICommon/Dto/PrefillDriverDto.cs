using System;

namespace CommonAPICommon.Dto
{
    public class PrefillDriverDto
    {
        public int rowID { get; set; }
        public int ISOMasterID { get; set; }
        public int quoteID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DLNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string SSN { get; set; }
        public string Gender { get; set; }
        public string MaskedDOB { get; set; }
        public string MaskedDLNumber { get; set; }
        public string MaskedSSN { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
