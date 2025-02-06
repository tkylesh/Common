using CommonAPICommon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIDAL.Repository.Interface
{
    public interface IRapa2LookupRepository
    {
        string GetBodyStyleDesc(string bodyStyleCode);
        string GetMakeDesc(string makeIsoCode);
        string GetOldMakeDesc(string makeIsoCode);
        int GetOldMakeID(string makeIsoCode);
        int Rapa2WriteAuditHdr(string vsr, Rapa2VinResponseDto response, int quoteId, string policyNbr, string vin, string rapaparm);
        void Rapa2WriteAuditDetail(Rapa2VinResponseDto response, int hdrId, string vin);
        void Rapa2UpdateSelectedRecord(int hdrId, int Seq);
        Rapa2LimitsDto GetRapa2Limits(string state);
        void SetSelectedRapa2Vin(int quoteid, string vin, int seq);
    }
}
