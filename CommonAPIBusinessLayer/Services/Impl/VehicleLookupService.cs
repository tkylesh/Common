using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon.Dto;
using CommonAPICommon.ExtensionMethods;
using CommonAPIDAL.Repository.Impl;
using CommonAPIDAL.Repository.Interface;

namespace CommonAPIBusinessLayer.Services.Impl
{
    public class VehicleLookupService : IVehicleLookupService
    {
        private IVehicleLookupRepository repository = null;

        public VehicleLookupService(IVehicleLookupRepository lookupRepository)
        {
            repository = lookupRepository;
        }

        public VehicleLookupService()
        {
            repository = new VehicleLookupRepository();
        }

        public VINResultDto GetVINResults(string vin)
        {
            var vinResult = new VINResultDto();

            vin = vin.Replace("&", "_");

            var fullMatches = GetFullMatches(vin);
            if (fullMatches.Count() == 1)  //exactly one match.  This makes us smile.
            {
                vinResult.ISODirectMatch = true;
                vinResult.VinItems = fullMatches.ToVinItemList(vin);
            }
            else if (fullMatches.Select(vm => vm.VIN).Distinct().Count() == 1)  // matched multiples but they all have the same VIN.  Need to get most recent update. 
                                                                                //                                                              Update qdl 1/24:  When I changed entity to point at view vs table this
                                                                                //                                                              became less important, but no harm in keeping block.
            {
                vinResult.ISODirectMatch = true;
                vinResult.VinItems = fullMatches.OrderByDescending(vm => vm.EffDate).First().ToVinItemList(vin); //sorts desc by EffDate and get first one.
            }
            else if (fullMatches.Select(vm => vm.VIN).Distinct().Count() > 1)   //this means we somehow we have a full match on muliple records due to inconsistent 
                                                                                //wildcards.  I don't think this should ever happen and considered 
                                                                                //throwing an error, but decidedto return them.
            {
                var multList = new List<VINMasterWithMakeDto>();
                foreach (var _vin in fullMatches.Select(vm => vm.VIN).Distinct())  // distinct VIN values in Vinmaster 
                {
                    multList.Add(fullMatches.Where(vm => vm.VIN == _vin).OrderByDescending(vm => vm.EffDate).First());  //filter by vin, sort, and get first.
                }
                //vinResult.VinItems = multList;
            }
            else  //try 3rd party
            {
                // Call 3rd Party
                //vinResult.ThirdPartyCalled = true;
                VINMasterWithMakeDto vinmaster = repository.GetDefaultValues();
                vinResult.VinItems = new List<VINItemDto>();
                VINItemDto item = new VINItemDto
                {
                    PIPSymbol = vinmaster.PIPSymbol,
                    LiabilitySymbol = vinmaster.LiabilitySymbol
                };
                vinResult.VinItems.Add(item);
            }
            return vinResult;
        }
        public async Task<VINResultDto> GetVINResults(string vin, Dictionary<string, string> RapaParmDict, string Rapaparm)
        {
            return GetVINResults(vin);
        }
        public VehicleMakeResultDto GetMakes(int modelYear)
        {
            var makeResult = new VehicleMakeResultDto();

            makeResult.PassedModelYear = modelYear;

            var makes = new List<VehicleMakeDto>();

            var dbMakes = repository.GetVINMakes(modelYear);

            foreach (var dbMake in dbMakes.OrderBy(m => m.MakeName))
            {
                VehicleMakeDto dto = new VehicleMakeDto
                {
                    MakeId = dbMake.MakeId == null ? 0 : dbMake.MakeId,
                    MakeAbbr = dbMake.MakeAbbr == null ? "" : dbMake.MakeAbbr,
                    MakeName = dbMake.MakeName == null ? "" : dbMake.MakeName
                };
                makes.Add(dto);
            }

            makeResult.Makes = makes;
            return makeResult;

        }

        public VehicleModelResultDto GetModels(int modelYear, int makeId, string state = "", string make = "")
        {
            var modelResult = new VehicleModelResultDto();
            modelResult.PassedModelYear = modelYear;
            var models = new List<VehicleModelDto>();

            var dbModels = repository.GetModels(modelYear, ref makeId, "", make);

            modelResult.PassedMakeId = makeId;

            foreach (var dbModel in dbModels.OrderBy(m => m.ShortModel).Select(m => new { m.ShortModel, m.FullModel }).Distinct())
            {
                var model = new VehicleModelDto();
                model.ShortModelName = dbModel.ShortModel;
                model.FullModelName = dbModel.FullModel;
                var submodels = new List<VehicleSubModelDto>();
                var vinMasterLastestsByModel = dbModels
                                    .Where(dm => dm.FullModel == model.FullModelName && dm.ShortModel == model.ShortModelName)
                                    .Select(dm => new
                                    {
                                        dm.MakeID,
                                        dm.VinID,
                                        dm.VIN,
                                        dm.BodyStyle,
                                        dm.BodyStyleDesc,
                                        dm.DistinguishingInfo,
                                        dm.ISOSymbol,
                                        dm.CompSymbol,
                                        dm.CollSymbol,
                                        dm.ModelInfo,
                                        dm.ModelInfoDescription,
                                        dm.RestraintInd,
                                        dm.AntiTheftInd,
                                        dm.FourWheelDriveInd,
                                        dm.Horsepower,
                                        dm.EngineInfo,
                                        dm.EngineSize,
                                        dm.EngineType,
                                        dm.Cylinders,
                                        dm.ClassCode,
                                        dm.DaytimeRunningLightsInd,
                                        dm.AntiLockInd,
                                        dm.WheelbaseInfo,
                                        dm.TransmissionInfo,
                                        dm.StateException,
                                        dm.NCIC_Manufacturer,
                                        dm.SpecialInfoSelector,
                                        dm.PIPSymbol,
                                        dm.LiabilitySymbol
                                    });
                foreach (var lbm in vinMasterLastestsByModel.OrderBy(v => v.BodyStyle))
                {
                    model.MakeId = (int)lbm.MakeID;
                    submodels.Add(new VehicleSubModelDto()
                    {
                        BodyStyle = lbm.BodyStyle,
                        BodyStyleDesc = lbm.BodyStyleDesc,
                        CollSymbol = lbm.CollSymbol,
                        CompSymbol = lbm.CompSymbol,
                        DistinguishingInfo = lbm.DistinguishingInfo,
                        Horsepower = lbm.Horsepower,
                        ISOSymbol = lbm.ISOSymbol,
                        VIN = lbm.VIN,
                        VinId = lbm.VinID,
                        ModelInfo = lbm.ModelInfo,
                        ModelInfoDescription = lbm.ModelInfoDescription,
                        RestraintInd = lbm.RestraintInd,
                        AntiTheftInd = lbm.AntiTheftInd,
                        FourWheelDriveInd = lbm.FourWheelDriveInd,
                        Cylinders = lbm.Cylinders,
                        EngineType = lbm.EngineType,
                        EngineSize = lbm.EngineSize,
                        EngineInfo = lbm.EngineInfo,
                        ClassCode = lbm.ClassCode,
                        DaytimeRunningLightsInd = lbm.DaytimeRunningLightsInd,
                        AntiLockInd = lbm.AntiTheftInd,
                        WheelbaseInfo = lbm.WheelbaseInfo,
                        TransmissionInfo = lbm.TransmissionInfo,
                        StateException = lbm.StateException,
                        NCIC_Manufacturer = lbm.NCIC_Manufacturer,
                        SpecialInfoSelector = lbm.SpecialInfoSelector,
                        PIPSymbol = lbm.PIPSymbol,
                        LiabilitySymbol = lbm.LiabilitySymbol
                    });
                }
                model.SubModels = submodels.OrderBy(sm => sm.BodyStyle).ToList();
                models.Add(model);
            }

            modelResult.Models = models.OrderBy(m => m.FullModelName).ToList();
            return modelResult;
        }

        public VehicleMakeDto GetMakeById(int makeId)
        {
            var make = new VehicleMakeDto();
            var vinMake = repository.GetMakeById(makeId);
            if (vinMake != null)
            {
                make.MakeAbbr = vinMake.MakeAbbr;
                make.MakeId = vinMake.MakeId;
                make.MakeName = vinMake.MakeName;
            }
            return make;
        }

        public VehicleModelDto GetModelByVINId(int vinId, string state = "")
        {
            var model = new VehicleModelDto();

            var vinmasterWithModelInfo = repository.GetModelById(vinId);
            if (vinmasterWithModelInfo != null)
            {
                model.FullModelName = vinmasterWithModelInfo.FullModel;
                model.ShortModelName = vinmasterWithModelInfo.ShortModel;
                model.SubModels = new List<VehicleSubModelDto>() {
                    new VehicleSubModelDto() {
                        BodyStyle = vinmasterWithModelInfo.BodyStyle,
                        BodyStyleDesc = vinmasterWithModelInfo.BodyStyleDesc,
                        CollSymbol = vinmasterWithModelInfo.CollSymbol,
                        CompSymbol = vinmasterWithModelInfo.CompSymbol,
                        DistinguishingInfo = vinmasterWithModelInfo.DistinguishingInfo,
                        Horsepower = vinmasterWithModelInfo.Horsepower,
                        ISOSymbol = vinmasterWithModelInfo.ISOSymbol,
                        ModelInfo = vinmasterWithModelInfo.ModelInfo,
                        ModelInfoDescription = vinmasterWithModelInfo.ModelInfoDescription,
                        VIN = vinmasterWithModelInfo.VIN,
                        VinId = vinmasterWithModelInfo.VinID,
                        RestraintInd = vinmasterWithModelInfo.RestraintInd,
                        AntiTheftInd = vinmasterWithModelInfo.AntiTheftInd,
                        FourWheelDriveInd = vinmasterWithModelInfo.FourWheelDriveInd,
                        Cylinders = vinmasterWithModelInfo.Cylinders,
                        EngineType = vinmasterWithModelInfo.EngineType,
                        EngineSize = vinmasterWithModelInfo.EngineSize,
                        EngineInfo = vinmasterWithModelInfo.EngineInfo,
                        ClassCode = vinmasterWithModelInfo.ClassCode,
                        DaytimeRunningLightsInd = vinmasterWithModelInfo.DaytimeRunningLightsInd,
                        AntiLockInd = vinmasterWithModelInfo.AntiLockInd,
                        WheelbaseInfo = vinmasterWithModelInfo.WheelbaseInfo,
                        TransmissionInfo = vinmasterWithModelInfo.TransmissionInfo,
                        StateException = vinmasterWithModelInfo.StateException,
                        NCIC_Manufacturer = vinmasterWithModelInfo.NCIC_Manufacturer,
                        SpecialInfoSelector = vinmasterWithModelInfo.SpecialInfoSelector,
                        PIPSymbol = vinmasterWithModelInfo.PIPSymbol,
                        LiabilitySymbol = vinmasterWithModelInfo.LiabilitySymbol
                    }
                };
            }
            return model;
        }

        private IEnumerable<VINMasterWithMakeDto> GetFullMatches(string vin)
        {
            var list = new List<VINMasterWithMakeDto>();
            if (vin.Length >= 10)  // if less than 10, best to let third party handle it.
            {
                var vinmastersubset = repository.GetVINSubset(vin.Substring(0, 3));
                int i = 0;
                foreach (var vinMaster in vinmastersubset)
                {
                    i++;
                    string v = vinMaster.VIN.ToString();
                    int matchedDigs = v.MatchedDigits(vin, 3, 10);
                    int vinMasterVINAmpCount = v.AmpCount();
                    if (matchedDigs == 7 ||                                  //Matched all 7 digits (already know first 3 match)
                        (matchedDigs == 6 && vinMasterVINAmpCount == 1) ||   //Matched 6 of 7 but vimmaster record has 1 wildcard
                        (matchedDigs == 5 && vinMasterVINAmpCount == 2))     //Matched 5 of 7 but vinmaster record has 2 wildcards  
                    {

                        var dto = new VINMasterWithMakeDto
                        {
                            VinID = vinMaster.VinID == null ? 0 : vinMaster.VinID,
                            VIN = v,
                            ModelYear = vinMaster.ModelYear,
                            EffDate = vinMaster.EffDate,
                            MakeCode = vinMaster.MakeCode == null ? "" : vinMaster.MakeCode,
                            MakeID = vinMaster.MakeID == null ? 0 : vinMaster.MakeID,
                            Make = vinMaster.Make == null ? "" : vinMaster.Make,
                            MakeAbbr = vinMaster.MakeAbbr == null ? "" : vinMaster.MakeAbbr,
                            ShortModel = vinMaster.ShortModel == null ? "" : vinMaster.ShortModel,
                            FullModel = vinMaster.FullModel == null ? "" : vinMaster.FullModel,
                            ISOSymbol = vinMaster.ISOSymbol == null ? "" : vinMaster.ISOSymbol,
                            CollSymbol = vinMaster.CollSymbol == null ? "" : vinMaster.CollSymbol,
                            CompSymbol = vinMaster.CompSymbol == null ? "" : vinMaster.CompSymbol,
                            RestraintInd = vinMaster.RestraintInd == null ? "" : vinMaster.RestraintInd,
                            AntiTheftInd = vinMaster.AntiTheftInd == null ? "" : vinMaster.AntiTheftInd,
                            FourWheelDriveInd = vinMaster.FourWheelDriveInd == null ? "" : vinMaster.FourWheelDriveInd,
                            Cylinders = string.IsNullOrEmpty(vinMaster.Cylinders) ? "" : vinMaster.Cylinders,
                            EngineType = string.IsNullOrEmpty(vinMaster.EngineType) ? "" : vinMaster.EngineType,
                            EngineSize = string.IsNullOrEmpty(vinMaster.EngineSize) ? "" : vinMaster.EngineSize,
                            EngineInfo = string.IsNullOrEmpty(vinMaster.EngineInfo) ? "" : vinMaster.EngineInfo,
                            ClassCode = string.IsNullOrEmpty(vinMaster.ClassCode) ? "" : vinMaster.ClassCode,
                            DaytimeRunningLightsInd = string.IsNullOrEmpty(vinMaster.DaytimeRunningLightsInd) ? "" : vinMaster.DaytimeRunningLightsInd,
                            AntiLockInd = string.IsNullOrEmpty(vinMaster.AntiLockInd) ? "" : vinMaster.AntiLockInd,
                            WheelbaseInfo = string.IsNullOrEmpty(vinMaster.WheelbaseInfo) ? "" : vinMaster.WheelbaseInfo,
                            BodyStyle = string.IsNullOrEmpty(vinMaster.BodyStyle) ? "" : vinMaster.BodyStyle,
                            BodyStyleDesc = vinMaster.BodyStyleDesc,
                            TransmissionInfo = string.IsNullOrEmpty(vinMaster.TransmissionInfo) ? "" : vinMaster.TransmissionInfo,
                            StateException = string.IsNullOrEmpty(vinMaster.StateException) ? "" : vinMaster.StateException,
                            NCIC_Manufacturer = string.IsNullOrEmpty(vinMaster.NCIC_Manufacturer) ? "" : vinMaster.NCIC_Manufacturer,
                            SpecialInfoSelector = string.IsNullOrEmpty(vinMaster.SpecialInfoSelector) ? "" : vinMaster.SpecialInfoSelector,
                            PIPSymbol = string.IsNullOrEmpty(vinMaster.PIPSymbol) ? "" : vinMaster.PIPSymbol,
                            LiabilitySymbol = string.IsNullOrEmpty(vinMaster.LiabilitySymbol) ? "" : vinMaster.LiabilitySymbol
                        };
                        list.Add(dto);
                    }
                }
            }
            return list;
        }

        public bool ValidateVehicle(int modelyear, string model, string make)
        {
            return repository.ValidateVehicle(modelyear, model, make);
        }

        public List<VINMasterWithModelDto> GetMatchingMakeModels(int modelyear, string make, string model, string state = "")
        {
            var matches = repository.GetMatchingMakeModels(modelyear, make, model, state).OrderBy(m => m.ModelYear);
            int tries = 1;

            while (matches.Count() == 0 && tries != 10)
            {
                matches = repository.GetMatchingMakeModels(modelyear++, make, model, state).OrderBy(m => m.ModelYear);
                tries++;
            }

            return matches.ToList();
        }

        public void SetSelectedRapa2Vin(int quoteid, string vin, int seq)
        {
            throw new NotImplementedException();
        }

        
    }
}
