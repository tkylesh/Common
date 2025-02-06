using System.Collections.Generic;
using CommonAPIDAL.Repository.Interface;
using CommonAPIDAL.DataAccess;

namespace CommonAPIDAL.Repository.Impl
{
    public class ISORepository : IISORepository
    {
        public int GetExistingISOMasterId(dynamic applicant)
        {
            return ISODataAccess.GetExistingISOMasterId(applicant);
        }
        public int StoreRawAndMaster(int quoteId, string xmlFromISO, double responseTime, string status, string xmlToISO, string requestId, string product, dynamic applicant, string lexId)
        {
            ISODataAccess.StoreRawISOXML(quoteId, xmlFromISO, responseTime, status, xmlToISO, requestId, product, lexId);
            return ISODataAccess.StoreISOMaster(applicant);
        }
        public void StoreRawOnly(int quoteId, double responseTime, string status, string xmlToISO, string requestId, string product)
        {
            ISODataAccess.StoreRawISOXML(quoteId, "", responseTime, status, xmlToISO, requestId, product, "");
        }
        public void SaveCV(int masterId, int? supplierId, IList<dynamic> intervals, IList<dynamic> polDatas)
        {
            ISODataAccess.SaveCV(masterId, supplierId, intervals, polDatas);
        }
        public void SavePrefill(int masterId, int quoteId, IList<dynamic> drivers, IList<dynamic> vehicles, int? supplierID)
        {
            ISODataAccess.SavePrefill(masterId, quoteId, drivers, vehicles, supplierID);
        }
        public IList<dynamic> GetPolicyDatas(int isoMasterId)
        {
            return ISODataAccess.GetPolicyDatas(isoMasterId);
        }
        public IList<dynamic> GetCoverageIntervals(int isoMasterId)
        {
            return ISODataAccess.GetCoverageIntervals(isoMasterId);
        }
        public IList<dynamic> GetPrefillDrivers(int isoMasterId)
        {
            return ISODataAccess.GetPrefillDrivers(isoMasterId);
        }
        public IList<dynamic> GetPrefillVehicles(int isoMasterId)
        {
            return ISODataAccess.GetPrefillVehicles(isoMasterId);
        }

        public int GetQuoteId(int isoMasterId)
        {
            return ISODataAccess.GetQuoteID(isoMasterId);
        }
        public bool GetISOproduct(int quoteID)
        {
            return Common.getISOproduct(quoteID);
        }
    }
}
