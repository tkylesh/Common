using System.Collections.Generic;
using CommonAPICommon.Dto;
using CommonAPIDAL.DataAccess;
using CommonAPIDAL.Repository.Interface;

namespace CommonAPIDAL.Repository.Impl
{
    public class VehicleLookupRAPARepository : IVehicleLookupRepository
    {
        VehicleLookupDataAccess DA;
        public VehicleLookupRAPARepository()
        {
            DA = new VehicleLookupDataAccess();
        }
        public IEnumerable<VehicleMakeDto> GetVINMakes(int modelYear)
        {
            return VehicleLookupDataAccess.GetVINMakesRAPA(modelYear);
        }

        public IEnumerable<VINMasterWithModelDto> GetModels(int modelYear, ref int makeId, string state = "", string make = "")
        {
            return VehicleLookupDataAccess.GetModelsRAPA(modelYear, ref makeId, state, make);
        }

        public VehicleMakeDto GetMakeById(int makeId)
        {
            return DA.GetMakeByIdRAPA(makeId);
        }

        public VINMasterWithModelDto GetModelById(int vinId, string state = "")
        {
            return VehicleLookupDataAccess.GetModelByVINIdRAPA(vinId, state);
        }

        public IEnumerable<VINMasterWithMakeDto> GetVINSubset(string makeCode, string state = "", char supp = 'P')
        {
            return VehicleLookupDataAccess.GetVINSubsetRAPA(makeCode, state, supp.ToString());
        }

        public bool ValidateVehicle(int modelyear, string model, string make)
        {
            return DA.ValidateVehicleRAPA(modelyear, model, make);
        }

        public VINMasterWithMakeDto GetDefaultValues()
        {
            return VehicleLookupDataAccess.GetDefaultSymbolsRAPA();
        }

        public IEnumerable<VINMasterWithModelDto> GetMatchingMakeModels(int modelyear, string make, string model, string state = "")
        {
            return VehicleLookupDataAccess.GetMatchingMakeModelsRAPA(modelyear, make, model, state);
        }
    }
}
