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
    public class VehicleLookupRAPAService : IVehicleLookupService
    {
        private IVehicleLookupRepository repository = null;

        public string State { get; set; }

        public VehicleLookupRAPAService(string state)
        {
            repository = new VehicleLookupRAPARepository();
            State = state;
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
                vinResult.Supplemental = fullMatches.First().Supplemental;
            }
            else if (fullMatches.Select(vm => vm.VIN).Distinct().Count() == 1)  // matched multiples but they all have the same VIN.  Need to get most recent update. 
                                                                                //                                                              Update qdl 1/24:  When I changed entity to point at view vs table this
                                                                                //                                                              became less important, but no harm in keeping block.
            {
                vinResult.ISODirectMatch = true;
                vinResult.VinItems = fullMatches.OrderByDescending(vm => vm.EffDate).First().ToVinItemList(vin); //sorts desc by EffDate and get first one.
                vinResult.Supplemental = fullMatches.OrderByDescending(vm => vm.EffDate).First().Supplemental;
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
            }
            else  //try 3rd party
            {
                // Call 3rd Party
                //vinResult.ThirdPartyCalled = true;
                VINMasterWithMakeDto vinmaster = repository.GetDefaultValues();
                vinResult.VinItems = new List<VINItemDto>();

                VINItemDto item = new VINItemDto
                {
                    BiSymbol = vinmaster.BiSymbol,
                    PIPSymbol = vinmaster.PIPSymbol,
                    PDSymbol = vinmaster.PDSymbol,
                    MedPaySymbol = vinmaster.MedPaySymbol
                };

                vinResult.VinItems.Add(item);
            }
            return vinResult;
        }

        public async Task<VINResultDto> GetVINResults(string vin, Dictionary<string, string> RapaParmDict, string Rapaparm)
        {
            return GetVINResults(vin);
        }

        public VehicleMakeResultDto GetVINMakes(int modelYear)
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

            var dbModels = repository.GetModels(modelYear, ref makeId, state, make);

            modelResult.PassedMakeId = makeId;

            foreach (var dbModel in dbModels.OrderBy(m => m.ShortModel).Select(m => new { m.ShortModel, m.FullModel }).Distinct())
            {
                var model = new VehicleModelDto();
                model.ShortModelName = dbModel.ShortModel;
                model.FullModelName = dbModel.FullModel;
                model.SubModels = new List<VehicleSubModelDto>();
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
                                        dm.BiSymbol,
                                        dm.PDSymbol,
                                        dm.MedPaySymbol,
                                        dm.ModelInfo,
                                        dm.ModelInfoDescription,
                                        dm.RestraintInd,
                                        dm.AntiTheftInd,
                                        dm.FourWheelDriveInd,
                                        dm.ISONumber,
                                        dm.EngineInfo,
                                        dm.EngineSize,
                                        dm.Horsepower,
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
                                        dm.LiabilitySymbol,
                                        dm.TonnageInd,
                                        dm.PayloadCapacity,
                                        dm.Supplemental
                                    });
                foreach (var lbm in vinMasterLastestsByModel.OrderBy(v => v.BodyStyle))
                {
                    model.MakeId = (int)lbm.MakeID;
                    model.SubModels.Add(new VehicleSubModelDto()
                    {
                        BodyStyleDesc = string.IsNullOrWhiteSpace(lbm.BodyStyleDesc) ? lbm.BodyStyle : lbm.BodyStyleDesc.Replace('–', '-'),
                        BodyStyle = lbm.BodyStyle,
                        CollSymbol = string.IsNullOrEmpty(lbm.CollSymbol) ? "" : lbm.CollSymbol,
                        CompSymbol = string.IsNullOrEmpty(lbm.CompSymbol) ? "" : lbm.CompSymbol,
                        BiSymbol = string.IsNullOrEmpty(lbm.BiSymbol) ? "" : lbm.BiSymbol,
                        PDSymbol = string.IsNullOrEmpty(lbm.PDSymbol) ? "" : lbm.PDSymbol,
                        MedPaySymbol = string.IsNullOrEmpty(lbm.MedPaySymbol) ? "" : lbm.MedPaySymbol,
                        DistinguishingInfo = string.IsNullOrEmpty(lbm.DistinguishingInfo) ? "" : lbm.DistinguishingInfo,
                        ISOSymbol = string.IsNullOrEmpty(lbm.ISOSymbol) ? "" : lbm.ISOSymbol,
                        VIN = string.IsNullOrEmpty(lbm.VIN) ? "" : lbm.VIN,
                        VinId = lbm.VinID,
                        ModelInfo = string.IsNullOrEmpty(lbm.ModelInfo) ? "" : lbm.ModelInfo,
                        ModelInfoDescription = string.IsNullOrEmpty(lbm.ModelInfoDescription) ? "" : lbm.ModelInfoDescription,
                        RestraintInd = string.IsNullOrEmpty(lbm.RestraintInd) ? "" : lbm.RestraintInd,
                        AntiTheftInd = string.IsNullOrEmpty(lbm.AntiTheftInd) ? "" : lbm.AntiTheftInd,
                        FourWheelDriveInd = string.IsNullOrEmpty(lbm.FourWheelDriveInd) ? "" : lbm.FourWheelDriveInd,
                        ISONumber = string.IsNullOrEmpty(lbm.ISONumber) ? "" : lbm.ISONumber,
                        Cylinders = string.IsNullOrEmpty(lbm.Cylinders) ? "" : lbm.Cylinders,
                        EngineType = string.IsNullOrEmpty(lbm.EngineType) ? "" : lbm.EngineType,
                        EngineSize = string.IsNullOrEmpty(lbm.EngineSize) ? "" : lbm.EngineSize,
                        Horsepower = string.IsNullOrEmpty(lbm.Horsepower) ? "" : lbm.Horsepower,
                        EngineInfo = string.IsNullOrEmpty(lbm.EngineInfo) ? "" : lbm.EngineInfo,
                        ClassCode = string.IsNullOrEmpty(lbm.ClassCode) ? "" : lbm.ClassCode,
                        DaytimeRunningLightsInd = string.IsNullOrEmpty(lbm.DaytimeRunningLightsInd) ? "" : lbm.DaytimeRunningLightsInd,
                        AntiLockInd = string.IsNullOrEmpty(lbm.AntiLockInd) ? "" : lbm.AntiLockInd,
                        WheelbaseInfo = string.IsNullOrEmpty(lbm.WheelbaseInfo) ? "" : lbm.WheelbaseInfo,
                        TransmissionInfo = string.IsNullOrEmpty(lbm.TransmissionInfo) ? "" : lbm.TransmissionInfo,
                        StateException = string.IsNullOrEmpty(lbm.StateException) ? "" : lbm.StateException,
                        NCIC_Manufacturer = string.IsNullOrEmpty(lbm.NCIC_Manufacturer) ? "" : lbm.NCIC_Manufacturer,
                        SpecialInfoSelector = string.IsNullOrEmpty(lbm.SpecialInfoSelector) ? "" : lbm.SpecialInfoSelector,
                        PIPSymbol = string.IsNullOrEmpty(lbm.PIPSymbol) ? "" : lbm.PIPSymbol,
                        LiabilitySymbol = string.IsNullOrEmpty(lbm.LiabilitySymbol) ? "" : lbm.LiabilitySymbol,
                        TonnageInd = lbm.TonnageInd,
                        PayloadCapacity = lbm.PayloadCapacity,
                        Supplemental = lbm.Supplemental
                    });
                }

                models.Add(model);
            }

            modelResult.Models = models;

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
                make.Supplemental = false;
            }
            return make;
        }

        public VehicleModelDto GetModelByVINId(int vinId, string state)
        {
            var model = new VehicleModelDto();

            var vinmasterWithModelInfo = repository.GetModelById(vinId, state);
            if (vinmasterWithModelInfo != null)
            {
                model.FullModelName = vinmasterWithModelInfo.FullModel;
                model.ShortModelName = vinmasterWithModelInfo.ShortModel;
                model.SubModels = new List<VehicleSubModelDto>() {
                    new VehicleSubModelDto() {
                        BodyStyle = string.IsNullOrEmpty(vinmasterWithModelInfo.BodyStyle) ? "" : vinmasterWithModelInfo.BodyStyle,
                        CollSymbol = string.IsNullOrEmpty(vinmasterWithModelInfo.CollSymbol) ? "" : vinmasterWithModelInfo.CollSymbol,
                        CompSymbol = string.IsNullOrEmpty(vinmasterWithModelInfo.CompSymbol) ? "" : vinmasterWithModelInfo.CompSymbol,
                        BiSymbol = string.IsNullOrEmpty(vinmasterWithModelInfo.BiSymbol) ? "" : vinmasterWithModelInfo.BiSymbol,
                        PDSymbol = string.IsNullOrEmpty(vinmasterWithModelInfo.PDSymbol) ? "" : vinmasterWithModelInfo.PDSymbol,
                        MedPaySymbol = string.IsNullOrEmpty(vinmasterWithModelInfo.MedPaySymbol) ? "" : vinmasterWithModelInfo.MedPaySymbol,
                        DistinguishingInfo = string.IsNullOrEmpty(vinmasterWithModelInfo.DistinguishingInfo) ? "" : vinmasterWithModelInfo.DistinguishingInfo,
                        ISOSymbol = string.IsNullOrEmpty(vinmasterWithModelInfo.ISOSymbol) ? "" : vinmasterWithModelInfo.ISOSymbol,
                        ModelInfo = string.IsNullOrEmpty(vinmasterWithModelInfo.ModelInfo) ? "" : vinmasterWithModelInfo.ModelInfo,
                        ModelInfoDescription = string.IsNullOrEmpty(vinmasterWithModelInfo.ModelInfoDescription) ? "" : vinmasterWithModelInfo.ModelInfoDescription,
                        VIN = string.IsNullOrEmpty(vinmasterWithModelInfo.VIN) ? "" : vinmasterWithModelInfo.VIN,
                        VinId = vinmasterWithModelInfo.VinID,
                        RestraintInd = string.IsNullOrEmpty(vinmasterWithModelInfo.RestraintInd) ? "" : vinmasterWithModelInfo.RestraintInd,
                        AntiTheftInd = string.IsNullOrEmpty(vinmasterWithModelInfo.AntiTheftInd) ? "" : vinmasterWithModelInfo.AntiTheftInd,
                        FourWheelDriveInd = string.IsNullOrEmpty(vinmasterWithModelInfo.FourWheelDriveInd) ? "" : vinmasterWithModelInfo.FourWheelDriveInd,
                        ISONumber = string.IsNullOrEmpty(vinmasterWithModelInfo.ISONumber) ? "" : vinmasterWithModelInfo.ISONumber,
                        Cylinders = string.IsNullOrEmpty(vinmasterWithModelInfo.Cylinders) ? "" : vinmasterWithModelInfo.Cylinders,
                        EngineType = string.IsNullOrEmpty(vinmasterWithModelInfo.EngineType) ? "" : vinmasterWithModelInfo.EngineType,
                        EngineSize = string.IsNullOrEmpty(vinmasterWithModelInfo.EngineSize) ? "" : vinmasterWithModelInfo.EngineSize,
                        Horsepower = string.IsNullOrEmpty(vinmasterWithModelInfo.Horsepower) ? "" : vinmasterWithModelInfo.Horsepower,
                        EngineInfo = string.IsNullOrEmpty(vinmasterWithModelInfo.EngineInfo) ? "" : vinmasterWithModelInfo.EngineInfo,
                        ClassCode = string.IsNullOrEmpty(vinmasterWithModelInfo.ClassCode) ? "" : vinmasterWithModelInfo.ClassCode,
                        DaytimeRunningLightsInd = string.IsNullOrEmpty(vinmasterWithModelInfo.DaytimeRunningLightsInd) ? "" : vinmasterWithModelInfo.DaytimeRunningLightsInd,
                        AntiLockInd = string.IsNullOrEmpty(vinmasterWithModelInfo.AntiLockInd) ? "" : vinmasterWithModelInfo.AntiLockInd,
                        WheelbaseInfo = string.IsNullOrEmpty(vinmasterWithModelInfo.WheelbaseInfo) ? "" : vinmasterWithModelInfo.WheelbaseInfo,
                        TransmissionInfo = string.IsNullOrEmpty(vinmasterWithModelInfo.TransmissionInfo) ? "" : vinmasterWithModelInfo.TransmissionInfo,
                        StateException = string.IsNullOrEmpty(vinmasterWithModelInfo.StateException) ? "" : vinmasterWithModelInfo.StateException,
                        NCIC_Manufacturer = string.IsNullOrEmpty(vinmasterWithModelInfo.NCIC_Manufacturer) ? "" : vinmasterWithModelInfo.NCIC_Manufacturer,
                        SpecialInfoSelector = string.IsNullOrEmpty(vinmasterWithModelInfo.SpecialInfoSelector) ? "" : vinmasterWithModelInfo.SpecialInfoSelector,
                        PIPSymbol = string.IsNullOrEmpty(vinmasterWithModelInfo.PIPSymbol) ? "" : vinmasterWithModelInfo.PIPSymbol,
                        LiabilitySymbol = string.IsNullOrEmpty(vinmasterWithModelInfo.LiabilitySymbol) ? "" : vinmasterWithModelInfo.LiabilitySymbol,
                        BodyStyleDesc = vinmasterWithModelInfo.BodyStyleDesc,
                        TonnageInd = vinmasterWithModelInfo.TonnageInd,
                        PayloadCapacity = vinmasterWithModelInfo.PayloadCapacity,
                        Supplemental = vinmasterWithModelInfo.Supplemental
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
                //Access the primary tables
                var vinmastersubset = repository.GetVINSubset(vin.Substring(0, 3), State, 'P');

                AddVinToListIfMatch(vin, list, vinmastersubset);

                if (!list.Any())
                {
                    //Access the suppelmental tables
                    vinmastersubset = repository.GetVINSubset(vin.Substring(0, 3), State, 'S');

                    AddVinToListIfMatch(vin, list, vinmastersubset);

                }
            }
            return list;
        }

        private static void AddVinToListIfMatch(string vin, List<VINMasterWithMakeDto> list, IEnumerable<VINMasterWithMakeDto> vinmastersubset)
        {
            foreach (var vinMaster in vinmastersubset)
            {
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
                        ModelYear = vinMaster.ModelYear == null ? 0 : vinMaster.ModelYear,
                        EffDate = vinMaster.EffDate == null ? DateTime.Now : vinMaster.EffDate,
                        MakeCode = vinMaster.MakeCode == null ? "" : vinMaster.MakeCode,
                        MakeID = vinMaster.MakeID == null ? 0 : vinMaster.MakeID,
                        Make = vinMaster.Make == null ? "" : vinMaster.Make,
                        MakeAbbr = vinMaster.MakeAbbr == null ? "" : vinMaster.MakeAbbr,
                        ShortModel = vinMaster.ShortModel == null ? "" : vinMaster.ShortModel,
                        FullModel = vinMaster.FullModel == null ? "" : vinMaster.FullModel,
                        ISOSymbol = vinMaster.ISOSymbol == null ? "" : vinMaster.ISOSymbol,
                        CollSymbol = vinMaster.CollSymbol == null ? "" : vinMaster.CollSymbol,
                        CompSymbol = vinMaster.CompSymbol == null ? "" : vinMaster.CompSymbol,
                        BiSymbol = string.IsNullOrEmpty(vinMaster.BiSymbol) ? "" : vinMaster.BiSymbol,
                        PDSymbol = string.IsNullOrEmpty(vinMaster.PDSymbol) ? "" : vinMaster.PDSymbol,
                        MedPaySymbol = string.IsNullOrEmpty(vinMaster.MedPaySymbol) ? "" : vinMaster.MedPaySymbol,
                        RestraintInd = string.IsNullOrEmpty(vinMaster.RestraintInd) ? "" : vinMaster.RestraintInd,
                        AntiTheftInd = string.IsNullOrEmpty(vinMaster.AntiTheftInd) ? "" : vinMaster.AntiTheftInd,
                        FourWheelDriveInd = string.IsNullOrEmpty(vinMaster.FourWheelDriveInd) ? "" : vinMaster.FourWheelDriveInd,
                        ISONumber = string.IsNullOrEmpty(vinMaster.ISONumber) ? "" : vinMaster.ISONumber,
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
                        LiabilitySymbol = string.IsNullOrEmpty(vinMaster.LiabilitySymbol) ? "" : vinMaster.LiabilitySymbol,
                        TonnageInd = vinMaster.TonnageInd,
                        PayloadCapacity = vinMaster.PayloadCapacity,
                        Supplemental = vinMaster.Supplemental
                    };
                    list.Add(dto);
                }
            }
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
                    MakeName = dbMake.MakeName == null ? "" : dbMake.MakeName,
                    Supplemental = dbMake.Supplemental
                };
                makes.Add(dto);
            }

            makeResult.Makes = makes;
            return makeResult;
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
