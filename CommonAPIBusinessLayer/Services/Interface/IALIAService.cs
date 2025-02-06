using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;

namespace CommonAPIBusinessLayer.Services.Interface
{
    public interface IALIAService
    {
        string CallAlfaWebService(string memberNumber, string zipCode, string quoteId);
        ALResults CallAlfaWebServiceAs400(string memberNumber, string policyNumber);
        string AddNewMembershipNumber(QuoteDataForNewMember user, int quoteId);
        QuoteDataForNewMember GetAFFNewMemberData(int quoteId);
        //string GetALIAToken(string memberNumber);
        //bool AliaNameMatch(ALIAMemberNumberDto alfaCallResults, string quoteId, List<string> applicants);
        //string AliaWebReturn(ALIAMemberNumberDto alfaCallResults, bool applicantMatch);
        //ALDto AliaAs400Return(ALIAMemberNumberDto alfaCallResults);
        string MemberSearch(QuoteDataForMemberSearch user, int quoteId);
        QuoteDataForMemberSearch GetMemberSearchData(int quoteId);
    }
}
