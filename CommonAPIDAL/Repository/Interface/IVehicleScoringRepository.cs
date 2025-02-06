using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;

namespace CommonAPIDAL.Repository.Interface
{
    public interface IVehicleScoringRepository
    {
        int StoreRawAndMaster(int quoteId, VehicleRiskResultsDto vsr, string xmlFromRM, double responseTime, string errorMessage, string xmlToRM, string jsTo, string jsFrom, string requestState);
        VehicleScoreDto CheckExistingVehicleScore(string VIN);
        string GetRequestState(int quoteID);
    }
}
