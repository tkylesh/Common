using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;
using CommonAPIDAL.Mapping;
using CommonAPIDAL.VisionAppModels;

namespace CommonAPIDAL.DataAccess
{
    internal partial class VehicleLookupDataAccess : DataAccessBase
    {

        public bool ValidateVehicleRAPA(int modelyear, string model, string make)
        {
            bool ret = false;

            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                int makeid = context.VIN_Make.Single(vm => vm.MakeAbbr == make).MakeID;

                if (makeid > 0)
                {
                    var result = context.VinrapaPD.Where(v => v.MakeID == makeid && v.ModelYear == modelyear && v.ShortModelName == model).ToList();
                    if (result.Count > 0)
                    {
                        ret = true;
                    }
                    else // MM New for Supp
                    {
                        var resultSupp = context.VinrapaSupplementalPD.Where(v => v.MakeID == makeid && v.ModelYear == modelyear && v.ShortModelName == model).ToList();
                        if (resultSupp.Count > 0)
                        {
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }



        public static IEnumerable<VINMasterWithModelDto> GetModelsRAPA(int modelYear, ref int makeId, string state, string make = "")
        {
            List<VINMasterWithModelDto> list = new List<VINMasterWithModelDto>();

            try
            {
                using (var context = new VisionAppEntities(ConnectionString))
                {
                    int tmpmakeid = makeId;

                    if (make.Length > 0 && makeId == 0)
                        tmpmakeid = context.VIN_Make.Single(vm => vm.MakeAbbr == make).MakeID;

                    list = GetModelList(modelYear, state, context, tmpmakeid);

                    var tempList = GetModelListSupp(modelYear, state, context, tmpmakeid);
                    if (tempList != null)
                    {
                        foreach (var item in tempList)
                        {
                            if (!CompareVinRecords(item, list))
                            {
                                list.Add(item);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return list.AsEnumerable();
        }

        private static bool CompareVinRecords(VINMasterWithModelDto item, List<VINMasterWithModelDto> list)
        {
            bool ret = false;
            foreach (var oldItem in list)
            {
                if (oldItem.VIN == item.VIN &&
                oldItem.ModelYear == item.ModelYear &&
                oldItem.MakeCode == item.MakeCode &&
                oldItem.MakeID == item.MakeID &&
                oldItem.Make == item.Make &&
                oldItem.MakeAbbr == item.MakeAbbr &&
                oldItem.ShortModel == item.ShortModel &&
                oldItem.FullModel == item.FullModel &&
                oldItem.ISOSymbol == item.ISOSymbol &&
                oldItem.CompSymbol == item.CompSymbol &&
                oldItem.CollSymbol == item.CollSymbol &&
                oldItem.ModelInfo == item.ModelInfo &&
                oldItem.ModelInfoDescription == item.ModelInfoDescription &&
                oldItem.BodyStyle == item.BodyStyle &&
                oldItem.BodyStyleDesc == item.BodyStyleDesc &&
                oldItem.Horsepower == item.Horsepower &&
                oldItem.DistinguishingInfo == item.DistinguishingInfo &&
                oldItem.BiSymbol == item.BiSymbol &&
                oldItem.PDSymbol == item.PDSymbol &&
                oldItem.MedPaySymbol == item.MedPaySymbol &&
                oldItem.PIPSymbol == item.PIPSymbol &&
                oldItem.RestraintInd == item.RestraintInd &&
                oldItem.AntiTheftInd == item.AntiTheftInd &&
                oldItem.FourWheelDriveInd == item.FourWheelDriveInd &&
                oldItem.ISONumber == item.ISONumber &&
                oldItem.Cylinders == item.Cylinders &&
                oldItem.EngineType == item.EngineType &&
                oldItem.EngineSize == item.EngineSize &&
                oldItem.EngineInfo == item.EngineInfo &&
                oldItem.ClassCode == item.ClassCode &&
                oldItem.DaytimeRunningLightsInd == item.DaytimeRunningLightsInd &&
                oldItem.AntiLockInd == item.AntiLockInd &&
                oldItem.WheelbaseInfo == item.WheelbaseInfo &&
                oldItem.TransmissionInfo == item.TransmissionInfo &&
                oldItem.StateException == item.StateException &&
                oldItem.NCIC_Manufacturer == item.NCIC_Manufacturer &&
                oldItem.SpecialInfoSelector == item.SpecialInfoSelector &&
                oldItem.LiabilitySymbol == item.LiabilitySymbol &&
                oldItem.PayloadCapacity == item.PayloadCapacity &&
                oldItem.TonnageInd == item.TonnageInd)
                {
                    ret = true;
                    break;
                }
            }

            return ret;
        }

        private static List<VINMasterWithModelDto> GetModelList(int modelYear, string state, VisionAppEntities context, int tmpmakeid)
        {
            List<VINMasterWithModelDto> list = (from vm in context.VinrapaPD
                                                join vinmake in context.VIN_Make on vm.MakeID equals vinmake.MakeID
                                                join mi in context.VIN_ModelInfo on vm.ModelInfo equals mi.ModelInfo into ps
                                                from p in ps.DefaultIfEmpty()
                                                join bs in context.VIN_BodyStyle on vm.BodyStyle equals bs.BodyStyle into vbs
                                                from b in vbs.DefaultIfEmpty()
                                                join l in context.VinrapaLiability on vm.VIN equals l.VIN into u_pl
                                                from pl in u_pl.DefaultIfEmpty()
                                                join ls in context.VinLiabilitySymbol on vm.VIN equals ls.VIN into vls
                                                from l in vls.DefaultIfEmpty()
                                                where vm.ModelYear == modelYear && vm.MakeID == tmpmakeid
                                                select new VINMasterWithModelDto
                                                {
                                                    VinID = vm.vinraID,
                                                    VIN = vm.VIN,
                                                    ModelYear = vm.ModelYear,
                                                    EffDate = vm.EffDate,
                                                    MakeCode = vm.MakeCode,
                                                    MakeID = vm.MakeID,
                                                    Make = vinmake.Make,
                                                    MakeAbbr = vinmake.MakeAbbr,
                                                    ShortModel = vm.ShortModelName,
                                                    FullModel = vm.FullModelName,
                                                    CompSymbol = state == "ga" ? vm.CompSymbolGA : vm.CompSymbol,
                                                    CollSymbol = state == "ga" ? vm.CollSymbolGA : vm.CollSymbol,
                                                    BiSymbol = state == "ga" ? pl.BiSymbolGA : pl.BiSymbol,
                                                    PDSymbol = state == "ga" ? pl.PDSymbolGA : pl.PDSymbol,
                                                    MedPaySymbol = state == "ga" ? pl.MedPaySymbolGA : pl.MedPaySymbol,
                                                    PIPSymbol = state == "ga" ? pl.PIPSymbolGA : pl.PIPSymbol,
                                                    ModelInfo = vm.ModelInfo,
                                                    ModelInfoDescription = p.ModelInfoDescription,
                                                    BodyStyle = vm.BodyStyle.Length > 0 ? vm.BodyStyle : l.BodyStyle.Length > 0 ? l.BodyStyle : "",
                                                    BodyStyleDesc = b.BodyStyleDesc,
                                                    Horsepower = vm.HorsePower,
                                                    RestraintInd = vm.RestraintInd,
                                                    AntiTheftInd = vm.AntiTheftInd,
                                                    FourWheelDriveInd = vm.FourWheelDriveInd,
                                                    ISONumber = vm.ISONum,
                                                    Cylinders = vm.Cylinders,
                                                    EngineType = vm.EngineType,
                                                    EngineSize = vm.EngineSize,
                                                    EngineInfo = vm.EngineInfo,
                                                    ClassCode = vm.ClassCode,
                                                    DaytimeRunningLightsInd = vm.DayTimeRunningLightInd,
                                                    AntiLockInd = vm.AntiLockInd,
                                                    TransmissionInfo = vm.TransmissionInfo,
                                                    StateException = vm.StateException,
                                                    WheelbaseInfo = l.WheelBaseInfo,
                                                    SpecialInfoSelector = vm.SpecialInfoSelector,
                                                    TonnageInd = vm.TonnageInd,
                                                    PayloadCapacity = vm.PayloadCapacity,
                                                    Supplemental = false
                                                }).Distinct().ToList();

            list.Select(l => { l.BodyStyleDesc = l.BodyStyleDesc.Replace('–', '-'); return l; });

            return list;
        }

        private static List<VINMasterWithModelDto> GetModelListSupp(int modelYear, string state, VisionAppEntities context, int tmpmakeid)
        {
            List<VINMasterWithModelDto> list = (from vm in context.VinrapaSupplementalPD
                                                join vinmake in context.VIN_Make on vm.MakeID equals vinmake.MakeID
                                                join mi in context.VIN_ModelInfo on vm.ModelInfo equals mi.ModelInfo into ps
                                                from p in ps.DefaultIfEmpty()
                                                join bs in context.VIN_BodyStyle on vm.BodyStyle equals bs.BodyStyle into vbs
                                                from b in vbs.DefaultIfEmpty()
                                                join l in context.VinrapaLiabilitySupplemental on vm.VIN equals l.VIN into u_pl
                                                from pl in u_pl.DefaultIfEmpty()
                                                join ls in context.VinLiabilitySymbol on vm.VIN equals ls.VIN into vls
                                                from l in vls.DefaultIfEmpty()
                                                where vm.ModelYear == modelYear && vm.MakeID == tmpmakeid
                                                select new VINMasterWithModelDto
                                                {
                                                    VinID = vm.vinraID,
                                                    VIN = vm.VIN,
                                                    ModelYear = vm.ModelYear,
                                                    EffDate = vm.EffDate,
                                                    MakeCode = vm.MakeCode,
                                                    MakeID = vm.MakeID,
                                                    Make = vinmake.Make,
                                                    MakeAbbr = vinmake.MakeAbbr,
                                                    ShortModel = vm.ShortModelName,
                                                    FullModel = vm.FullModelName,
                                                    CompSymbol = state == "ga" ? vm.CompSymbolGA : vm.CompSymbol,
                                                    CollSymbol = state == "ga" ? vm.CollSymbolGA : vm.CollSymbol,
                                                    BiSymbol = state == "ga" ? pl.BiSymbolGA : pl.BiSymbol,
                                                    PDSymbol = state == "ga" ? pl.PDSymbolGA : pl.PDSymbol,
                                                    MedPaySymbol = state == "ga" ? pl.MedPaySymbolGA : pl.MedPaySymbol,
                                                    PIPSymbol = state == "ga" ? pl.PIPSymbolGA : pl.PIPSymbol,
                                                    ModelInfo = vm.ModelInfo,
                                                    ModelInfoDescription = p.ModelInfoDescription,
                                                    BodyStyle = vm.BodyStyle.Length > 0 ? vm.BodyStyle : l.BodyStyle.Length > 0 ? l.BodyStyle : "",
                                                    BodyStyleDesc = b.BodyStyleDesc,
                                                    Horsepower = vm.HorsePower,
                                                    RestraintInd = vm.RestraintInd,
                                                    AntiTheftInd = vm.AntiTheftInd,
                                                    FourWheelDriveInd = vm.FourWheelDriveInd,
                                                    ISONumber = vm.ISONum,
                                                    Cylinders = vm.Cylinders,
                                                    EngineType = vm.EngineType,
                                                    EngineSize = vm.EngineSize,
                                                    EngineInfo = vm.EngineInfo,
                                                    ClassCode = vm.ClassCode,
                                                    DaytimeRunningLightsInd = vm.DayTimeRunningLightInd,
                                                    AntiLockInd = vm.AntiLockInd,
                                                    TransmissionInfo = vm.TransmissionInfo,
                                                    StateException = vm.StateException,
                                                    WheelbaseInfo = l.WheelBaseInfo,
                                                    SpecialInfoSelector = vm.SpecialInfoSelector,
                                                    TonnageInd = vm.TonnageInd,
                                                    PayloadCapacity = vm.PayloadCapacity,
                                                    Supplemental = true
                                                }).Distinct().ToList();

            list.Select(l => { l.BodyStyleDesc = l.BodyStyleDesc.Replace('–', '-'); return l; });

            return list;
        }

        public static VINMasterWithModelDto GetModelByVINIdRAPA(int vinId, string state)
        {
            VINMasterWithModelDto vinmasterWithModelInfo = null;

            using (var context = new VisionAppEntities(ConnectionString))
            {
                vinmasterWithModelInfo = (from vm in context.VinrapaPD
                                          join make in context.VIN_Make
                                          on vm.MakeID equals make.MakeID
                                          join mi in context.VIN_ModelInfo on vm.ModelInfo equals mi.ModelInfo into ps
                                          from p in ps.DefaultIfEmpty()
                                          join l in context.VinrapaLiability on vm.VIN equals l.VIN into u_pl
                                          from pl in u_pl.DefaultIfEmpty()
                                          join ls in context.VinLiabilitySymbol on vm.VIN equals ls.VIN into vls
                                          from l in vls.DefaultIfEmpty()
                                          join bs in context.VIN_BodyStyle on vm.BodyStyle equals bs.BodyStyle into vbs
                                          from b in vbs.DefaultIfEmpty()
                                          where vm.vinraID == vinId
                                          select new VINMasterWithModelDto
                                          {
                                              VinID = vm.vinraID,
                                              VIN = vm.VIN,
                                              ModelYear = vm.ModelYear,
                                              EffDate = vm.EffDate,
                                              MakeCode = vm.MakeCode,
                                              MakeID = vm.MakeID,
                                              Make = make.Make,
                                              MakeAbbr = make.MakeAbbr,
                                              ShortModel = vm.ShortModelName,
                                              FullModel = vm.FullModelName,
                                              ModelInfo = vm.ModelInfo,
                                              ModelInfoDescription = p.ModelInfoDescription,
                                              BodyStyle = vm.BodyStyle.Length > 0 ? vm.BodyStyle : l.BodyStyle.Length > 0 ? l.BodyStyle : "",
                                              BodyStyleDesc = b.BodyStyleDesc,
                                              Horsepower = vm.HorsePower,
                                              CompSymbol = state == "ga" ? vm.CompSymbolGA : vm.CompSymbol,
                                              CollSymbol = state == "ga" ? vm.CollSymbolGA : vm.CollSymbol,
                                              BiSymbol = state == "ga" ? pl.BiSymbolGA : pl.BiSymbol,
                                              PDSymbol = state == "ga" ? pl.PDSymbolGA : pl.PDSymbol,
                                              MedPaySymbol = state == "ga" ? pl.MedPaySymbolGA : pl.MedPaySymbol,
                                              PIPSymbol = state == "ga" ? pl.PIPSymbolGA : pl.PIPSymbol,
                                              RestraintInd = vm.RestraintInd,
                                              AntiTheftInd = vm.AntiTheftInd,
                                              FourWheelDriveInd = vm.FourWheelDriveInd,
                                              ISONumber = vm.ISONum,
                                              Cylinders = vm.Cylinders,
                                              EngineType = vm.EngineType,
                                              EngineSize = vm.EngineSize,
                                              EngineInfo = vm.EngineInfo,
                                              ClassCode = vm.ClassCode,
                                              DaytimeRunningLightsInd = vm.DayTimeRunningLightInd,
                                              AntiLockInd = vm.AntiLockInd,
                                              TransmissionInfo = vm.TransmissionInfo,
                                              StateException = vm.StateException,
                                              WheelbaseInfo = l.WheelBaseInfo,
                                              SpecialInfoSelector = vm.SpecialInfoSelector,
                                              TonnageInd = vm.TonnageInd,
                                              PayloadCapacity = vm.PayloadCapacity,
                                              Supplemental = false
                                          }).SingleOrDefault<VINMasterWithModelDto>();

                // MM new for Supp
                if (vinmasterWithModelInfo == null)
                {
                    vinmasterWithModelInfo = (from vm in context.VinrapaSupplementalPD
                                              join make in context.VIN_Make
                                              on vm.MakeID equals make.MakeID
                                              join mi in context.VIN_ModelInfo on vm.ModelInfo equals mi.ModelInfo into ps
                                              from p in ps.DefaultIfEmpty()
                                              join l in context.VinrapaLiabilitySupplemental on vm.VIN equals l.VIN into u_pl
                                              from pl in u_pl.DefaultIfEmpty()
                                              join ls in context.VinLiabilitySymbol on vm.VIN equals ls.VIN into vls
                                              from l in vls.DefaultIfEmpty()
                                              join bs in context.VIN_BodyStyle on vm.BodyStyle equals bs.BodyStyle into vbs
                                              from b in vbs.DefaultIfEmpty()
                                              where vm.vinraID == vinId
                                              select new VINMasterWithModelDto
                                              {
                                                  VinID = vm.vinraID,
                                                  VIN = vm.VIN,
                                                  ModelYear = vm.ModelYear,
                                                  EffDate = vm.EffDate,
                                                  MakeCode = vm.MakeCode,
                                                  MakeID = vm.MakeID,
                                                  Make = make.Make,
                                                  MakeAbbr = make.MakeAbbr,
                                                  ShortModel = vm.ShortModelName,
                                                  FullModel = vm.FullModelName,
                                                  ModelInfo = vm.ModelInfo,
                                                  ModelInfoDescription = p.ModelInfoDescription,
                                                  BodyStyle = vm.BodyStyle.Length > 0 ? vm.BodyStyle : l.BodyStyle.Length > 0 ? l.BodyStyle : "",
                                                  BodyStyleDesc = b.BodyStyleDesc,
                                                  Horsepower = vm.HorsePower,
                                                  CompSymbol = state == "ga" ? vm.CompSymbolGA : vm.CompSymbol,
                                                  CollSymbol = state == "ga" ? vm.CollSymbolGA : vm.CollSymbol,
                                                  BiSymbol = state == "ga" ? pl.BiSymbolGA : pl.BiSymbol,
                                                  PDSymbol = state == "ga" ? pl.PDSymbolGA : pl.PDSymbol,
                                                  MedPaySymbol = state == "ga" ? pl.MedPaySymbolGA : pl.MedPaySymbol,
                                                  PIPSymbol = state == "ga" ? pl.PIPSymbolGA : pl.PIPSymbol,
                                                  RestraintInd = vm.RestraintInd,
                                                  AntiTheftInd = vm.AntiTheftInd,
                                                  FourWheelDriveInd = vm.FourWheelDriveInd,
                                                  ISONumber = vm.ISONum,
                                                  Cylinders = vm.Cylinders,
                                                  EngineType = vm.EngineType,
                                                  EngineSize = vm.EngineSize,
                                                  EngineInfo = vm.EngineInfo,
                                                  ClassCode = vm.ClassCode,
                                                  DaytimeRunningLightsInd = vm.DayTimeRunningLightInd,
                                                  AntiLockInd = vm.AntiLockInd,
                                                  TransmissionInfo = vm.TransmissionInfo,
                                                  StateException = vm.StateException,
                                                  WheelbaseInfo = l.WheelBaseInfo,
                                                  SpecialInfoSelector = vm.SpecialInfoSelector,
                                                  TonnageInd = vm.TonnageInd,
                                                  PayloadCapacity = vm.PayloadCapacity,
                                                  Supplemental = true
                                              }).SingleOrDefault<VINMasterWithModelDto>();
                }

                if (vinmasterWithModelInfo != null)
                {
                    vinmasterWithModelInfo.BodyStyleDesc.Replace('–', '-');
                }

                return vinmasterWithModelInfo;

            }
        }

        public static IEnumerable<VINMasterWithMakeDto> GetVINSubsetRAPA(string makeCode, string state, string supp)
        {
            IEnumerable<VINMasterWithMakeDto> results = null;

            using (var context = new VisionAppEntities(ConnectionString))
            {
                // MM changed for New Process
                var q = context.Procedures.uspVinLookup_MannufacturerID_TypeAsync(makeCode, state, supp).Result;

                results = q.Select(t =>  Mapper.Map(t, supp)).AsQueryable();
            }

            return results;
        }

        public static IEnumerable<VehicleMakeDto> GetVINMakesRAPA(int modelYear)
        {
            IEnumerable<VehicleMakeDto> makes = null;

            using (var context = new VisionAppEntities(ConnectionString))
            {
                var v = (from pd in context.VinrapaPD
                         join make in context.VIN_Make on pd.MakeID equals make.MakeID
                         where pd.ModelYear == modelYear
                         select new VehicleMakeDto
                         {
                             MakeAbbr = make.MakeAbbr,
                             MakeName = make.Make,
                             MakeId = make.MakeID,
                             Supplemental = false
                         }).Distinct().ToList();

                // MM Added for new supplemtal
                if (v == null || v.Count == 0)
                {
                    v = (from pd in context.VinrapaSupplementalPD
                         join make in context.VIN_Make on pd.MakeID equals make.MakeID
                         where pd.ModelYear == modelYear
                         select new VehicleMakeDto
                         {
                             MakeAbbr = make.MakeAbbr,
                             MakeName = make.Make,
                             MakeId = make.MakeID,
                             Supplemental = true
                         }).Distinct().ToList();
                }
                else
                {
                    var vsup = (from mml in context.VinrapaSupplementalPD
                                join make in context.VIN_Make on mml.MakeCode equals make.MakeAbbr
                                where mml.ModelYear == modelYear
                                select new VehicleMakeDto
                                {
                                    MakeAbbr = make.MakeAbbr,
                                    MakeName = make.Make,
                                    MakeId = make.MakeID,
                                    Supplemental = true
                                }).Distinct().ToList();


                    foreach (var make in vsup)
                    {
                        if (v.Where(w => w.MakeId == make.MakeId).Count() == 0)
                        {
                            v.Add(make);
                        }
                    }
                }

                if (v == null || v.Count == 0)
                {
                    v = (from mml in context.MakeModelLookup
                         join make in context.VIN_Make on mml.MakeCode equals make.MakeAbbr
                         where mml.ModelYear == modelYear
                         select new VehicleMakeDto
                         {
                             MakeAbbr = make.MakeAbbr,
                             MakeName = make.Make,
                             MakeId = make.MakeID,
                             Supplemental = false
                         }).Distinct().ToList();

                }


                //if prior to 1975, use the makes from the vin_make table.
                if (v == null || v.Count == 0)
                {
                    v = Common.getMakesPrior1975(context);

                    foreach (var res in v)
                    {
                        res.Supplemental = false;
                    }
                }

                makes = v.AsEnumerable();
            }

            return makes;
        }

        public  VehicleMakeDto GetMakeByIdRAPA(int makeId)
        {
            VehicleMakeDto make = new VehicleMakeDto();

            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                var v = context.VIN_Make.FirstOrDefault(m => m.MakeID == makeId);
                make.MakeId = v.MakeID;
                make.MakeAbbr = v.MakeAbbr;
                make.MakeName = v.Make;
            }
            return make;
        }

        public static VINMasterWithMakeDto GetDefaultSymbolsRAPA()
        {
            return new VINMasterWithMakeDto
            {
                PIPSymbol = "99",
                PDSymbol = "99",
                BiSymbol = "99",
                MedPaySymbol = "99"
            };
        }

        public static IEnumerable<VINMasterWithModelDto> GetMatchingMakeModelsRAPA(int modelyear, string make, string model, string state = "")
        {
            List<VINMasterWithModelDto> list = new List<VINMasterWithModelDto>();

            try
            {
                using (var context = new VisionAppEntities(ConnectionString))
                {
                    var makeid = context.VIN_Make.Single(vm => vm.MakeAbbr == make).MakeID;

                    list = GetMatchingModelList(modelyear, model, state, context, makeid);

                    if (list == null || list.Count == 0)
                    {
                        list = GetMatchingModelListSupp(modelyear, model, state, context, makeid);
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return list.AsEnumerable();
        }

        private static List<VINMasterWithModelDto> GetMatchingModelList(int modelyear, string model, string state, VisionAppEntities context, int makeid)
        {
            List<VINMasterWithModelDto> list = (from vm in context.VinrapaPD
                                                join vinmake in context.VIN_Make on vm.MakeID equals vinmake.MakeID
                                                join mi in context.VIN_ModelInfo on vm.ModelInfo equals mi.ModelInfo into ps
                                                from p in ps.DefaultIfEmpty()
                                                join bs in context.VIN_BodyStyle on vm.BodyStyle equals bs.BodyStyle into vbs
                                                from b in vbs.DefaultIfEmpty()
                                                join l in context.VinrapaLiability on vm.VIN equals l.VIN into u_pl
                                                from pl in u_pl.DefaultIfEmpty()
                                                join ls in context.VinLiabilitySymbol on vm.VIN equals ls.VIN into vls
                                                from l in vls.DefaultIfEmpty()
                                                where vm.ShortModelName == model && vm.MakeID == makeid && vm.ModelYear >= modelyear && vm.ModelYear <= modelyear + 2
                                                select new VINMasterWithModelDto
                                                {
                                                    VinID = vm.vinraID,
                                                    VIN = vm.VIN,
                                                    ModelYear = vm.ModelYear,
                                                    EffDate = vm.EffDate,
                                                    MakeCode = vm.MakeCode,
                                                    MakeID = vm.MakeID,
                                                    Make = vinmake.Make,
                                                    MakeAbbr = vinmake.MakeAbbr,
                                                    ShortModel = vm.ShortModelName,
                                                    FullModel = vm.FullModelName,
                                                    CompSymbol = state == "ga" ? vm.CompSymbolGA : vm.CompSymbol,
                                                    CollSymbol = state == "ga" ? vm.CollSymbolGA : vm.CollSymbol,
                                                    BiSymbol = state == "ga" ? pl.BiSymbolGA : pl.BiSymbol,
                                                    PDSymbol = state == "ga" ? pl.PDSymbolGA : pl.PDSymbol,
                                                    MedPaySymbol = state == "ga" ? pl.MedPaySymbolGA : pl.MedPaySymbol,
                                                    PIPSymbol = state == "ga" ? pl.PIPSymbolGA : pl.PIPSymbol,
                                                    ModelInfo = vm.ModelInfo,
                                                    ModelInfoDescription = p.ModelInfoDescription,
                                                    BodyStyle = vm.BodyStyle.Length > 0 ? vm.BodyStyle : l.BodyStyle.Length > 0 ? l.BodyStyle : "",
                                                    BodyStyleDesc = b.BodyStyleDesc,
                                                    Horsepower = vm.HorsePower,
                                                    RestraintInd = vm.RestraintInd,
                                                    AntiTheftInd = vm.AntiTheftInd,
                                                    FourWheelDriveInd = vm.FourWheelDriveInd,
                                                    ISONumber = vm.ISONum,
                                                    Cylinders = vm.Cylinders,
                                                    EngineType = vm.EngineType,
                                                    EngineSize = vm.EngineSize,
                                                    EngineInfo = vm.EngineInfo,
                                                    ClassCode = vm.ClassCode,
                                                    DaytimeRunningLightsInd = vm.DayTimeRunningLightInd,
                                                    AntiLockInd = vm.AntiLockInd,
                                                    TransmissionInfo = vm.TransmissionInfo,
                                                    StateException = vm.StateException,
                                                    WheelbaseInfo = l.WheelBaseInfo,
                                                    SpecialInfoSelector = vm.SpecialInfoSelector,
                                                    TonnageInd = vm.TonnageInd,
                                                    PayloadCapacity = vm.PayloadCapacity,
                                                    Supplemental = false
                                                }).Distinct().ToList();
            list.Select(l => { l.BodyStyleDesc = l.BodyStyleDesc.Replace('–', '-'); return l; });
            return list;
        }

        private static List<VINMasterWithModelDto> GetMatchingModelListSupp(int modelyear, string model, string state, VisionAppEntities context, int makeid)
        {
            List<VINMasterWithModelDto> list = (from vm in context.VinrapaSupplementalPD
                                                join vinmake in context.VIN_Make on vm.MakeID equals vinmake.MakeID
                                                join mi in context.VIN_ModelInfo on vm.ModelInfo equals mi.ModelInfo into ps
                                                from p in ps.DefaultIfEmpty()
                                                join bs in context.VIN_BodyStyle on vm.BodyStyle equals bs.BodyStyle into vbs
                                                from b in vbs.DefaultIfEmpty()
                                                join l in context.VinrapaLiabilitySupplemental on vm.VIN equals l.VIN into u_pl
                                                from pl in u_pl.DefaultIfEmpty()
                                                join ls in context.VinLiabilitySymbol on vm.VIN equals ls.VIN into vls
                                                from l in vls.DefaultIfEmpty()
                                                where vm.ShortModelName == model && vm.MakeID == makeid && vm.ModelYear >= modelyear && vm.ModelYear <= modelyear + 2
                                                select new VINMasterWithModelDto
                                                {
                                                    VinID = vm.vinraID,
                                                    VIN = vm.VIN,
                                                    ModelYear = vm.ModelYear,
                                                    EffDate = vm.EffDate,
                                                    MakeCode = vm.MakeCode,
                                                    MakeID = vm.MakeID,
                                                    Make = vinmake.Make,
                                                    MakeAbbr = vinmake.MakeAbbr,
                                                    ShortModel = vm.ShortModelName,
                                                    FullModel = vm.FullModelName,
                                                    CompSymbol = state == "ga" ? vm.CompSymbolGA : vm.CompSymbol,
                                                    CollSymbol = state == "ga" ? vm.CollSymbolGA : vm.CollSymbol,
                                                    BiSymbol = state == "ga" ? pl.BiSymbolGA : pl.BiSymbol,
                                                    PDSymbol = state == "ga" ? pl.PDSymbolGA : pl.PDSymbol,
                                                    MedPaySymbol = state == "ga" ? pl.MedPaySymbolGA : pl.MedPaySymbol,
                                                    PIPSymbol = state == "ga" ? pl.PIPSymbolGA : pl.PIPSymbol,
                                                    ModelInfo = vm.ModelInfo,
                                                    ModelInfoDescription = p.ModelInfoDescription,
                                                    BodyStyle = vm.BodyStyle.Length > 0 ? vm.BodyStyle : l.BodyStyle.Length > 0 ? l.BodyStyle : "",
                                                    BodyStyleDesc = b.BodyStyleDesc,
                                                    Horsepower = vm.HorsePower,
                                                    RestraintInd = vm.RestraintInd,
                                                    AntiTheftInd = vm.AntiTheftInd,
                                                    FourWheelDriveInd = vm.FourWheelDriveInd,
                                                    ISONumber = vm.ISONum,
                                                    Cylinders = vm.Cylinders,
                                                    EngineType = vm.EngineType,
                                                    EngineSize = vm.EngineSize,
                                                    EngineInfo = vm.EngineInfo,
                                                    ClassCode = vm.ClassCode,
                                                    DaytimeRunningLightsInd = vm.DayTimeRunningLightInd,
                                                    AntiLockInd = vm.AntiLockInd,
                                                    TransmissionInfo = vm.TransmissionInfo,
                                                    StateException = vm.StateException,
                                                    WheelbaseInfo = l.WheelBaseInfo,
                                                    SpecialInfoSelector = vm.SpecialInfoSelector,
                                                    TonnageInd = vm.TonnageInd,
                                                    PayloadCapacity = vm.PayloadCapacity,
                                                    Supplemental = true
                                                }).Distinct().ToList();
            list.Select(l => { l.BodyStyleDesc = l.BodyStyleDesc.Replace('–', '-'); return l; });
            return list;
        }
    }
}
