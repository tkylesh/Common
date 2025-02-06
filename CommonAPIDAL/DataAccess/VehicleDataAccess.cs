using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;
using CommonAPIDAL.Mapping;
using CommonAPIDAL.VisionAppModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CommonAPIDAL.DataAccess
{

    internal partial class VehicleLookupDataAccess
    { 
        private static DbContextOptions<VisionAppEntities> ConnectionString
        {
                get
            {
                    DbContextOptions<VisionAppEntities> optionsBuilder = new DbContextOptions<VisionAppEntities>();
                    return optionsBuilder;
                }

        }

        //private static string SymbolLookupConnectionString
        //{
        //    get
        //    {
        //        return ConfigurationManager.ConnectionStrings["SymbolLookupConnectionString"].ConnectionString;
        //    }
        //}

        public static bool ValidateVehicle(int modelyear, string model, string make)
        {
            bool ret = false;

            using (var context = new VisionAppEntities(ConnectionString))
            {
                try
                {
                    int makeid = context.VIN_Make.Single(vm => vm.MakeAbbr == make).MakeID;
                    if (makeid > 0)
                    {
                        var result = context.Vinmaster.Where(v => v.MakeID == makeid && v.ModelYear == modelyear && v.ShortModelName == model).ToList();
                        if (result.Count > 0)
                            ret = true;
                    }
                }
                catch { }

            }
            return ret;
        }

        public static IEnumerable<VINMasterWithModelDto> GetModels(int modelYear, ref int makeId, string make = "")
        {
            List<VINMasterWithModelDto> list = new List<VINMasterWithModelDto>();

            using (var context = new VisionAppEntities(ConnectionString))
            {

                int tmpmakeid = makeId;

                if (make.Length > 0 && makeId == 0)
                    tmpmakeid = context.VIN_Make.Single(vm => vm.MakeAbbr == make).MakeID;

                list = (from vm in context.Vinmaster
                        join ltst in context.Vinmaster.GroupBy(v => v.VIN).Select(v => new { vin = v.Key, maxEffDate = v.Max(x => x.EffDate) })
                        on new { v = vm.VIN, d = vm.EffDate } equals new { v = ltst.vin, d = ltst.maxEffDate }
                        join vinmake in context.VIN_Make
                        on vm.MakeID equals vinmake.MakeID
                        join mi in context.VIN_ModelInfo on vm.ModelInfo equals mi.ModelInfo into ps
                        from p in ps.DefaultIfEmpty()
                        join bs in context.VIN_BodyStyle on vm.BodyStyle equals bs.BodyStyle into vbs
                        from b in vbs.DefaultIfEmpty()
                        join ls in context.VinLiabilitySymbol on vm.VIN equals ls.VIN into vls
                        from l in vls.DefaultIfEmpty()
                        where vm.ModelYear == modelYear && vm.MakeID == tmpmakeid
                        select new VINMasterWithModelDto
                        {
                            VinID = vm.vinID,
                            VIN = vm.VIN,
                            ModelYear = vm.ModelYear,
                            EffDate = vm.EffDate,
                            MakeCode = vm.MakeCode,
                            MakeID = vm.MakeID,
                            Make = vinmake.Make,
                            MakeAbbr = vinmake.MakeAbbr,
                            ShortModel = vm.ShortModelName,
                            FullModel = vm.FullModelName,
                            ISOSymbol = vm.ModelYear < 2011 ? vm.ISOSymbol : "",
                            CompSymbol = vm.ModelYear >= 2011 ? vm.CompSymbol : "",
                            CollSymbol = vm.ModelYear >= 2011 ? vm.CollSymbol : "",
                            ModelInfo = vm.ModelInfo,
                            ModelInfoDescription = p.ModelInfoDescription,
                            BodyStyle = vm.BodyStyle.Length > 0 ? vm.BodyStyle : l.BodyStyle.Length > 0 ? l.BodyStyle : "",
                            BodyStyleDesc = b.BodyStyleDesc,
                            Horsepower = vm.EngineSummary,
                            RestraintInd = vm.RestraintInd,
                            AntiTheftInd = vm.AntiTheftInd,
                            FourWheelDriveInd = vm.FourWheelDriveInd,
                            Cylinders = vm.Cylinders,
                            EngineType = vm.EngineType,
                            EngineSize = vm.EngineSize,
                            EngineInfo = vm.EngineInfo,
                            ClassCode = vm.ClassCode,
                            DaytimeRunningLightsInd = vm.DayTimeRunningLightInd,
                            AntiLockInd = vm.AntiLockInd,
                            TransmissionInfo = vm.TransmissionInfo,
                            StateException = vm.StateException,
                            NCIC_Manufacturer = vm.MakeCode,
                            WheelbaseInfo = l.WheelBaseInfo,
                            SpecialInfoSelector = vm.SpecialInfoSelector,
                            PIPSymbol = vm.ModelYear <= 1997 ? "970" : l.PIPSymbol,
                            LiabilitySymbol = vm.ModelYear <= 1997 ? "970" : l.LiabilitySymbol
                        }).Distinct().ToList();

                list.Select(l => { l.BodyStyleDesc = l.BodyStyleDesc.Replace('–', '-'); return l; });

                return list.AsEnumerable();
            }
        }

        public static VehicleMakeDto GetMakeById(int makeId)
        {
            VehicleMakeDto make = new VehicleMakeDto();

            using (var context = new VisionAppEntities(ConnectionString))
            {
                var v = context.VIN_Make.FirstOrDefault(m => m.MakeID == makeId);
                make.MakeId = v.MakeID;
                make.MakeAbbr = v.MakeAbbr;
                make.MakeName = v.Make;
            }

            return make;
        }

        public static VINMasterWithModelDto GetModelByVINId(int vinId)
        {
            VINMasterWithModelDto vinmasterWithModelInfo = new VINMasterWithModelDto();

            using (var context = new VisionAppEntities(ConnectionString))
            {
                vinmasterWithModelInfo = (from vm in context.Vinmaster
                                          join ltst in context.Vinmaster.GroupBy(v => v.VIN).Select(v => new { vin = v.Key, maxEffDate = v.Max(x => x.EffDate) })
                                          on new { v = vm.VIN, d = vm.EffDate } equals new { v = ltst.vin, d = ltst.maxEffDate }
                                          join make in context.VIN_Make
                                          on vm.MakeID equals make.MakeID
                                          join mi in context.VIN_ModelInfo on vm.ModelInfo equals mi.ModelInfo into ps
                                          from p in ps.DefaultIfEmpty()
                                          join ls in context.VinLiabilitySymbol on vm.VIN equals ls.VIN into vls
                                          from l in vls.DefaultIfEmpty()
                                          join bs in context.VIN_BodyStyle on vm.BodyStyle equals bs.BodyStyle into vbs
                                          from b in vbs.DefaultIfEmpty()
                                          where vm.vinID == vinId
                                          select new VINMasterWithModelDto
                                          {
                                              VinID = vm.vinID,
                                              VIN = vm.VIN,
                                              ModelYear = vm.ModelYear,
                                              EffDate = vm.EffDate,
                                              MakeCode = vm.MakeCode,
                                              MakeID = vm.MakeID,
                                              Make = make.Make,
                                              MakeAbbr = make.MakeAbbr,
                                              ShortModel = vm.ShortModelName,
                                              FullModel = vm.FullModelName,
                                              ISOSymbol = vm.ModelYear < 2011 ? vm.ISOSymbol : "",
                                              CompSymbol = vm.ModelYear >= 2011 ? vm.CompSymbol : "",
                                              CollSymbol = vm.ModelYear >= 2011 ? vm.CollSymbol : "",
                                              ModelInfoDescription = p.ModelInfoDescription,
                                              BodyStyle = vm.BodyStyle.Length > 0 ? vm.BodyStyle : l.BodyStyle.Length > 0 ? l.BodyStyle : "",
                                              BodyStyleDesc = b.BodyStyleDesc,
                                              Horsepower = vm.EngineSummary,
                                              RestraintInd = vm.RestraintInd,
                                              AntiTheftInd = vm.AntiTheftInd,
                                              FourWheelDriveInd = vm.FourWheelDriveInd,
                                              Cylinders = vm.Cylinders,
                                              EngineType = vm.EngineType,
                                              EngineSize = vm.EngineSize,
                                              EngineInfo = vm.EngineInfo,
                                              ClassCode = vm.ClassCode,
                                              DaytimeRunningLightsInd = vm.DayTimeRunningLightInd,
                                              AntiLockInd = vm.AntiLockInd,
                                              TransmissionInfo = vm.TransmissionInfo,
                                              StateException = vm.StateException,
                                              NCIC_Manufacturer = vm.MakeCode,
                                              WheelbaseInfo = l.WheelBaseInfo,
                                              SpecialInfoSelector = vm.SpecialInfoSelector,
                                              PIPSymbol = vm.ModelYear <= 1997 ? "970" : l.PIPSymbol,
                                              LiabilitySymbol = vm.ModelYear <= 1997 ? "970" : l.LiabilitySymbol
                                          }).SingleOrDefault<VINMasterWithModelDto>();

                vinmasterWithModelInfo.BodyStyleDesc.Replace('–', '-');

                return vinmasterWithModelInfo;
            }
        }

        public static IEnumerable<VINMasterWithMakeDto> GetVINSubset(string makeCode)
        {
            IEnumerable<VINMasterWithMakeDto> results = null;

            using (var context = new VisionAppEntities(ConnectionString))
            {
                //uspGetVINDataByManufacturerIDLegacyAsync
                var query = context.Procedures.uspGetVINDataByManufacturerIDLegacyAsync(makeCode).Result;
                //results = Mapper.Map<List<VINMasterWithMakeDto>>(query).AsQueryable();
                results = query.Select(t => Mapper.Map(t)).AsQueryable();

                //results.Select(l => { l.BodyStyleDesc = l.BodyStyleDesc.Replace('–', '-'); return l; });
            }

            return results;
        }

        public static VINMasterWithMakeDto GetDefaultSymbols()
        {

            return new VINMasterWithMakeDto
            {
                PIPSymbol = "990",
                LiabilitySymbol = "990"
            };
        }

        public static IEnumerable<VehicleMakeDto> GetVINMakes(int modelYear)
        {
            IEnumerable<VehicleMakeDto> makes = null;

            using (var context = new VisionAppEntities(ConnectionString))
            {

                var v = (from pd in context.Vinmaster
                         join make in context.VIN_Make on pd.MakeID equals make.MakeID
                         where pd.ModelYear == modelYear
                         select new VehicleMakeDto
                         {
                             MakeAbbr = make.MakeAbbr,
                             MakeName = make.Make,
                             MakeId = make.MakeID
                         }).Distinct().ToList();

                if (v == null || v.Count == 0)
                {
                    v = (from mml in context.MakeModelLookup
                         join make in context.VIN_Make on mml.MakeCode equals make.MakeAbbr
                         where mml.ModelYear == modelYear
                         select new VehicleMakeDto
                         {
                             MakeAbbr = make.MakeAbbr,
                             MakeName = make.Make,
                             MakeId = make.MakeID
                         }).Distinct().ToList();

                }

                //if prior to 1975, use the makes from the vin_make table.
                if (v == null || v.Count == 0)
                {
                    v = Common.getMakesPrior1975(context);
                }

                makes = v.AsEnumerable();

            }

            return makes;
        }

        public static IEnumerable<VINMasterWithModelDto> GetMatchingMakeModels(int modelyear, string make, string model, string state = "")
        {
            List<VINMasterWithModelDto> list = new List<VINMasterWithModelDto>();

            using (var context = new VisionAppEntities(ConnectionString))
            {

                var makeid = context.VIN_Make.Single(vm => vm.MakeAbbr == make).MakeID;

                list = (from vm in context.Vinmaster
                        join ltst in context.Vinmaster.GroupBy(v => v.VIN).Select(v => new { vin = v.Key, maxEffDate = v.Max(x => x.EffDate) })
                        on new { v = vm.VIN, d = vm.EffDate } equals new { v = ltst.vin, d = ltst.maxEffDate }
                        join vinmake in context.VIN_Make
                        on vm.MakeID equals vinmake.MakeID
                        join mi in context.VIN_ModelInfo on vm.ModelInfo equals mi.ModelInfo into ps
                        from p in ps.DefaultIfEmpty()
                        join bs in context.VIN_BodyStyle on vm.BodyStyle equals bs.BodyStyle into vbs
                        from b in vbs.DefaultIfEmpty()
                        join ls in context.VinLiabilitySymbol on vm.VIN equals ls.VIN into vls
                        from l in vls.DefaultIfEmpty()
                        where vm.ShortModelName == model && vm.MakeID == makeid && vm.ModelYear >= modelyear && vm.ModelYear <= modelyear + 2
                        select new VINMasterWithModelDto
                        {
                            VinID = vm.vinID,
                            VIN = vm.VIN,
                            ModelYear = vm.ModelYear,
                            EffDate = vm.EffDate,
                            MakeCode = vm.MakeCode,
                            MakeID = vm.MakeID,
                            Make = vinmake.Make,
                            MakeAbbr = vinmake.MakeAbbr,
                            ShortModel = vm.ShortModelName,
                            FullModel = vm.FullModelName,
                            ISOSymbol = vm.ModelYear < 2011 ? vm.ISOSymbol : "",
                            CompSymbol = vm.ModelYear >= 2011 ? vm.CompSymbol : "",
                            CollSymbol = vm.ModelYear >= 2011 ? vm.CollSymbol : "",
                            ModelInfo = vm.ModelInfo,
                            ModelInfoDescription = p.ModelInfoDescription,
                            BodyStyle = vm.BodyStyle.Length > 0 ? vm.BodyStyle : l.BodyStyle.Length > 0 ? l.BodyStyle : "",
                            BodyStyleDesc = b.BodyStyleDesc,
                            Horsepower = vm.EngineSummary,
                            RestraintInd = vm.RestraintInd,
                            AntiTheftInd = vm.AntiTheftInd,
                            FourWheelDriveInd = vm.FourWheelDriveInd,
                            Cylinders = vm.Cylinders,
                            EngineType = vm.EngineType,
                            EngineSize = vm.EngineSize,
                            EngineInfo = vm.EngineInfo,
                            ClassCode = vm.ClassCode,
                            DaytimeRunningLightsInd = vm.DayTimeRunningLightInd,
                            AntiLockInd = vm.AntiLockInd,
                            TransmissionInfo = vm.TransmissionInfo,
                            StateException = vm.StateException,
                            NCIC_Manufacturer = vm.MakeCode,
                            WheelbaseInfo = l.WheelBaseInfo,
                            SpecialInfoSelector = vm.SpecialInfoSelector,
                            PIPSymbol = vm.ModelYear <= 1997 ? "970" : l.PIPSymbol,
                            LiabilitySymbol = vm.ModelYear <= 1997 ? "970" : l.LiabilitySymbol
                        }).Distinct().ToList();

                list.Select(l => { l.BodyStyleDesc = l.BodyStyleDesc.Replace('–', '-'); return l; });

                return list.AsEnumerable();
            }
        }
    }
    //public class LegacyProfile : Profile
    //{
    //    public LegacyProfile()
    //    {
    //        CreateMap<VINMasterWithMakeDto, VINMasterWithMakeDto>();
    //        CreateMap<uspGetVINDataByManufacturerIDLegacy, VINMasterWithMakeDto>()
    //          .ForMember(dest => dest.ISOSymbol, opts => opts.MapFrom(src => src.ModelYear < 2011 ? src.ISOSymbol : ""))
    //          .ForMember(dest => dest.CompSymbol, opts => opts.MapFrom(src => src.ModelYear >= 2011 ? src.CompSymbol : ""))
    //          .ForMember(dest => dest.CollSymbol, opts => opts.MapFrom(src => src.ModelYear >= 2011 ? src.CollSymbol : ""))
    //          .ForMember(dest => dest.BodyStyle, opts => opts.MapFrom(src => src.BodyStyle.Length > 0 ? src.BodyStyle : src.LiabBodyStyle.Length > 0 ? src.LiabBodyStyle : ""))
    //          .ForMember(dest => dest.PIPSymbol, opts => opts.MapFrom(src => src.ModelYear <= 1997 ? "970" : src.PIPSymbol))
    //          .ForMember(dest => dest.LiabilitySymbol, opts => opts.MapFrom(src => src.ModelYear <= 1997 ? "970" : src.LiabilitySymbol));
    //    }
    //}
}
