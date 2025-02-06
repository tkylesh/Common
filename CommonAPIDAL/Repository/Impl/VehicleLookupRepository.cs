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
    public class VehicleLookupRepository : IVehicleLookupRepository
    {
        public IEnumerable<VehicleMakeDto> GetVINMakes(int modelYear)
        {
            return VehicleLookupDataAccess.GetVINMakes(modelYear);
        }

        public IEnumerable<VINMasterWithModelDto> GetModels(int modelYear, ref int makeId, string state = "", string make = "")
        {
            return VehicleLookupDataAccess.GetModels(modelYear, ref makeId, make);
        }

        public VehicleMakeDto GetMakeById(int makeId)
        {
            return VehicleLookupDataAccess.GetMakeById(makeId);
        }

        public VINMasterWithModelDto GetModelById(int vinId, string state = "")
        {
            return VehicleLookupDataAccess.GetModelByVINId(vinId);
        }

        public IEnumerable<VINMasterWithMakeDto> GetVINSubset(string makeCode, string state = "", char supp = 'P')
        {
            return VehicleLookupDataAccess.GetVINSubset(makeCode);
        }

        public bool ValidateVehicle(int modelyear, string model, string make)
        {
            return VehicleLookupDataAccess.ValidateVehicle(modelyear, model, make);
        }

        public VINMasterWithMakeDto GetDefaultValues()
        {
            return VehicleLookupDataAccess.GetDefaultSymbols();
        }


        public IEnumerable<VINMasterWithModelDto> GetMatchingMakeModels(int modelyear, string make, string model, string state = "")
        {
            return VehicleLookupDataAccess.GetMatchingMakeModels(modelyear, make, model, state);
        }

    }
}
