using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;

namespace CommonAPIBusinessLayer.Services.Interface
{
    public interface IVehicleScoreService
    {
        VehicleScoreResultsDto GetVehicleScores(int quoteid, List<VINItem> VINS, string reqstate, string rescoreFlag = "N");
    }
}
