namespace CommonAPICommon.Dto
{
    public class PrefillVehicleDto
    {
        public int rowID { get; set; }
        public int ISOMasterID { get; set; }
        public int quoteID { get; set; }
        public string VIN { get; set; }
        public string ModelYear { get; set; }
        public string Make { get; set; }
        public string ShortModel { get; set; }
        public string LienHolderSequenceNumber { get; set; }
        public string LienHolderName { get; set; }
        public string LienHolderStreetAddress { get; set; }
        public string LienHolderCity { get; set; }
        public string LienHolderState { get; set; }
        public string LienHolderZipCode { get; set; }
    }
}
