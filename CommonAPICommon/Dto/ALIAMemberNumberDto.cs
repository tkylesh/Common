using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon.Dto
{
    public class ALIAMemberNumberDto
    {
         public string success { get; set; }
         public MemberData memberData { get; set; }
         public string found { get; set; }
         public List<Errors> errors { get; set; }
    }
    public class MemberData
    {
        public DateTime? voidAfterdate { get; set; }
        public string primaryContactName { get; set; }
        public string membershipNumber { get; set; }
        public string currentStatus { get; set; }
        public string countyPresident { get; set; }
        public string countyFederation { get; set; }

    }
    public class Errors
    {
        public string errorMessage { get; set; }
        public string errorCode { get; set; }
        public string column { get; set; }
    }

    public class NewMemberData
    {
        public string memberNumber { get; set; }
        public string message { get; set; }
        public List<string> errors { get; set; }

    }

    public class QuoteDataForNewMember
    {
        public string serviceCenterNumber { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string addressCity { get; set; }
        public string email { get; set; }
        public string primaryFirstName { get; set; }
        public string primaryLastName { get; set; }
        public string addressState { get; set; }
        public string addressZipCode { get; set; }
        public string agentCsrEmail { get; set; }
        public string agentNumber { get; set; }
        public string comments { get; set; }
        public string dateOfBirth { get; set; }
        public string phoneNumber { get; set; }
        public string status { get; set; }
        public string typeIndicator { get; set; }
        public string secondaryFirstName { get; set; }
        public string secondaryLastName { get; set; }
    }

    public class NewMemberFlags
    {
        public bool MembershipFlag { get; set; }
        public bool PacFlag { get; set; }
    }

    public class MemberSearchResponse
    {
        public string message { get; set; }
        public List<string> errors { get; set; }
        public List<MemberSearchData> members { get; set; }
    }

    public class MemberSearchData
    {
        public string memberNumber { get; set; }
        public string fontevaStatus { get; set; }
        public string paidToDate { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string addressCity { get; set; }
        public string addressState { get; set; }
        public string addressZipCode { get; set; }
        public string primaryFirstName { get; set; }
        public string primaryLastName { get; set; }
        public string secondaryFirstName { get; set; }
        public string secondaryLastName { get; set; }
    }
    public class QuoteDataForMemberSearch
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string addressCity { get; set; }
        public string addressState { get; set; }
        public string addressZipCode { get; set; }
    }
}
