using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;
using CommonAPIDAL.DataAccess;
using CommonAPIDAL.Repository.Interface;

namespace CommonAPIDAL.Repository.Impl
{
    public class VehicleScoringRepository : IVehicleScoringRepository
    {
        public int StoreRawAndMaster(int quoteId, VehicleRiskResultsDto vsr, string xmlFromRM, double responseTime, string errorMessage, string xmlToRM, string jsTo, string jsFrom, string requestState)
        {
            int hdrid = VehicleScoringDataAccess.StoreRawRMXML(quoteId, vsr, xmlFromRM, responseTime, errorMessage, xmlToRM, jsTo, jsFrom);
            VehicleScoringDataAccess.StoreRMMaster(hdrid, vsr, quoteId, requestState);
            return hdrid;
        }
        public VehicleScoreDto CheckExistingVehicleScore(string VIN)
        {
            return VehicleScoringDataAccess.CheckExistingVehicleScore(VIN);
        }

        public string GetRequestState(int quoteID)
        {
            return Common.GetRequestState(quoteID);
        }
    }
}
