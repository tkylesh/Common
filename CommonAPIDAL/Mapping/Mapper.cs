using CommonAPICommon.Dto;
using CommonAPIDAL.VisionAppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIDAL.Mapping
{
    internal static class Mapper
    {

        
        public static VINMasterWithMakeDto Map(uspVinLookup_MannufacturerID_TypeResult input, string supp)
        {
            if (input == null)
                return null;


            VINMasterWithMakeDto DTO = new VINMasterWithMakeDto();

            DTO.VinID = input.VinID;
            DTO.VIN = input.VIN;
            DTO.ModelYear = input.ModelYear;
            DTO.EffDate = input.EffDate ?? default(DateTime);
            DTO.MakeCode = input.MakeCode;
            DTO.MakeID = input.MakeID ?? 0;
            DTO.Make = input.Make;
            DTO.MakeAbbr = input.MakeAbbr;
            DTO.ShortModel = input.ShortModel;
            DTO.FullModel = input.FullModel;
            DTO.ISOSymbol = input.ISONumber;
            DTO.CompSymbol = input.CompSymbol;
            DTO.CollSymbol = input.CollSymbol;
            DTO.BodyStyleDesc = input.BodyStyleDesc.Replace('–', '-');
            DTO.BiSymbol = input.BiSymbol;
            DTO.PDSymbol = input.PDSymbol;
            DTO.MedPaySymbol = input.MedPaySymbol;
            DTO.PIPSymbol = input.PIPSymbol;
            DTO.RestraintInd = input.RestraintInd;
            DTO.AntiTheftInd = input.AntiLockInd;
            DTO.FourWheelDriveInd = input.FourWheelDriveInd;
            DTO.ISONumber = input.ISONumber;
            DTO.Cylinders = input.Cylinders;
            DTO.EngineType = input.EngineType;
            DTO.EngineSize = input.EngineSize;
            DTO.EngineInfo = input.EngineInfo;
            DTO.ClassCode = input.ClassCode;
            DTO.DaytimeRunningLightsInd = input.DayTimeRunningLightInd;
            DTO.AntiLockInd = input.AntiLockInd;
            DTO.WheelbaseInfo = input.WheelbaseInfo;
            DTO.BodyStyle = input.BodyStyle;
            DTO.TransmissionInfo = input.TransmissionInfo;
            DTO.StateException = input.StateException;
            DTO.NCIC_Manufacturer = input.NCIC_Manufacturer;
            DTO.SpecialInfoSelector = input.SpecialInfoSelector;
            //DTO.LiabilitySymbol = input.li;
            DTO.VINChangeInd = Convert.ToChar(input.VINChangeInd);
            DTO.PayloadCapacity = input.PayloadCapacity;
            DTO.TonnageInd = input.TonnageInd;
            DTO.Supplemental = supp == "P" ? false : true;
            return DTO;
        }

        public static VINMasterWithMakeDto Map(uspGetVINDataByManufacturerIDLegacyResult input)
        {
            if (input == null)
                return null;
            VINMasterWithMakeDto DTO = new VINMasterWithMakeDto();

            DTO.VinID = input.vinID;
            DTO.VIN = input.VIN;
            DTO.ModelYear = input.ModelYear;
            DTO.EffDate = input.EffDate ?? default(DateTime);
            DTO.MakeCode = input.MakeCode;
            DTO.MakeID = input.MakeID ?? 0;
            DTO.Make = input.Make;
            DTO.MakeAbbr = input.MakeAbbr;
            DTO.ShortModel = input.ShortModel;
            DTO.FullModel = input.FullModel;
            //DTO.ISOSymbol = input.ISONumber;
            DTO.CompSymbol = input.CompSymbol;
            DTO.CollSymbol = input.CollSymbol;
            DTO.BodyStyleDesc = input.BodyStyleDesc.Replace('–', '-');
            //DTO.BiSymbol = input.BiSymbol;
            //DTO.PDSymbol = input.PDSymbol;
            //DTO.MedPaySymbol = input.sy;
            DTO.PIPSymbol = input.PIPSymbol;
            DTO.RestraintInd = input.RestraintInd;
            DTO.AntiTheftInd = input.AntiLockInd;
            DTO.FourWheelDriveInd = input.FourWheelDriveInd;
            //DTO.ISONumber = input.sio;
            DTO.Cylinders = input.Cylinders;
            DTO.EngineType = input.EngineType;
            DTO.EngineSize = input.EngineSize;
            DTO.EngineInfo = input.EngineInfo;
            DTO.ClassCode = input.ClassCode;
            DTO.DaytimeRunningLightsInd = input.DayTimeRunningLightInd;
            DTO.AntiLockInd = input.AntiLockInd;
            DTO.WheelbaseInfo = input.WheelBaseInfo;
            DTO.BodyStyle = input.BodyStyle;
            DTO.TransmissionInfo = input.TransmissionInfo;
            DTO.StateException = input.StateException;
            //DTO.NCIC_Manufacturer = input.NCIC_Manufacturer;
            DTO.SpecialInfoSelector = input.SpecialInfoSelector;
            //DTO.LiabilitySymbol = input.li;
            //DTO.VINChangeInd = Convert.ToChar(input.VINChangeInd);
            //DTO.PayloadCapacity = input.PayloadCapacity;
            //DTO.TonnageInd = input.TonnageInd;
            return DTO;
        }
    }
}
