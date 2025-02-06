using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIDAL.Repository.Interface
{
    public interface IISORepository
    {
        int GetExistingISOMasterId(dynamic applicant);
        int StoreRawAndMaster(int quoteId, string xmlFromISO, double responseTime, string status, string xmlToISO, string requestId, string product, dynamic applicant, string lexId);
        void StoreRawOnly(int quoteId, double responseTime, string status, string xmlToISO, string requestId, string product);
        void SaveCV(int masterId, int? supplierId, IList<dynamic> intervals, IList<dynamic> polDatas);
        void SavePrefill(int masterId, int quoteId, IList<dynamic> drivers, IList<dynamic> vehicles, int? supplierID);
        IList<dynamic> GetPolicyDatas(int isoMasterId);
        IList<dynamic> GetCoverageIntervals(int isoMasterId);
        IList<dynamic> GetPrefillDrivers(int isoMasterId);
        IList<dynamic> GetPrefillVehicles(int isoMasterId);
        int GetQuoteId(int isoMasterId);
        bool GetISOproduct(int quoteid);
    }
}
