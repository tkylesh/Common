using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;

namespace CommonAPIDAL.Repository.Interface
{
    public interface IVehicleLookupRepository
    {
        IEnumerable<VehicleMakeDto> GetVINMakes(int modelYear);
        IEnumerable<VINMasterWithModelDto> GetModels(int modelYear, ref int makeId, string state = "", string make = "");
        VehicleMakeDto GetMakeById(int makeId);
        VINMasterWithModelDto GetModelById(int vinId, string state = "");
        IEnumerable<VINMasterWithMakeDto> GetVINSubset(string makeCode, string state = "", char supp = 'P');
        bool ValidateVehicle(int modelyear, string model, string make);
        VINMasterWithMakeDto GetDefaultValues();
        IEnumerable<VINMasterWithModelDto> GetMatchingMakeModels(int modelyear, string make, string model, string state = "");
    }
}
