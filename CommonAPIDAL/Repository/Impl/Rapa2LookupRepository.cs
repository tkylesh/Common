using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPIDAL.DataAccess;
using CommonAPIDAL.Repository.Interface;
using CommonAPICommon.Dto;

namespace CommonAPIDAL.Repository.Impl
{
    public class Rapa2LookupRepository : IRapa2LookupRepository
    {
        Rapa2DataAccess R2 = new Rapa2DataAccess();
        public string GetBodyStyleDesc(string bodyStyleCode)
        {
            return R2.GetBodyStyleDesc(bodyStyleCode);
        }

        public string GetMakeDesc(string makeIsoCode)
        {
            return R2.GetMakeDesc(makeIsoCode);
        }

        public string GetOldMakeDesc(string makeIsoCode)
        {
            return R2.GetMakeDesc(makeIsoCode);
        }

        public int GetOldMakeID(string makeIsoCode)
        {
            return R2.GetOldMakeID(makeIsoCode);
        }


        public int Rapa2WriteAuditHdr(string vsr, Rapa2VinResponseDto response, int quoteId, string policyNbr, string vin, string rapaparm)
        {
            return R2.Rapa2WriteAuditHdr(vsr, response, quoteId, policyNbr, vin, rapaparm);
        }
        public void Rapa2WriteAuditDetail(Rapa2VinResponseDto response, int hdrId, string vin)
        {
            R2.Rapa2WriteAuditDetail(response, hdrId, vin);
        }

        public void Rapa2UpdateSelectedRecord(int hdrId, int Seq)
        {
            R2.Rapa2UpdateSelectedRecord(hdrId, Seq);
        }
        public Rapa2LimitsDto GetRapa2Limits(string state)
        {
            return R2.GetRapa2Limits(state);
        }
        public void SetSelectedRapa2Vin(int quoteid, string vin, int seq)
        {
            R2.SetSelectedRapa2Vin(quoteid, vin, seq);
        }
    }
}
