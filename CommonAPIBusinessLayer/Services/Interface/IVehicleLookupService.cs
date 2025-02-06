using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;

namespace CommonAPIBusinessLayer.Services.Interface
{
    public interface IVehicleLookupService
    {
        VINResultDto GetVINResults(string vin);
        Task<VINResultDto> GetVINResults(string vin, Dictionary<string, string> RapaParmDict, string Rapaparm);

        VehicleMakeResultDto GetMakes(int modelYear);
        VehicleModelResultDto GetModels(int modelYear, int makeID, string state = "", string make = "");
        VehicleMakeDto GetMakeById(int makeId);
        VehicleModelDto GetModelByVINId(int vinId, string state = "");
        bool ValidateVehicle(int modelyear, string model, string make);
        List<VINMasterWithModelDto> GetMatchingMakeModels(int modelyear, string make, string model, string state = "");
        void SetSelectedRapa2Vin(int quoteid, string vin, int seq);
    }
}
