using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
//using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CommonAPICommon;
using CommonAPICommon.Dto;
using CommonAPIDAL.VisionAppModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;


namespace CommonAPIDAL.DataAccess
{
    public class Rapa2DataAccess
    {
        SystemConfigurationManager Configuration = new SystemConfigurationManager();

        private static DbContextOptions<VisionAppEntities> ConnectionString
        {
            get
            {
                DbContextOptions<VisionAppEntities> optionsBuilder = new DbContextOptions<VisionAppEntities>();
                return optionsBuilder;
            }
        }
        public string GetBodyStyleDesc(string bodyStyleCode)
        {
            string bodyStyleDesc = string.Empty;
            if (bodyStyleCode != null)
            {
                using (var context = new VisionAppEntities(ConnectionString))
                {
                    bodyStyleDesc = context.Rapa2_BodyStyle.SingleOrDefault(bs => bs.BodyStyleCode == bodyStyleCode).BodyStyleDesc;
                }
            }
            return bodyStyleDesc;

        }

        public string GetMakeDesc(string makeIsoCode)
        {
            string makeDesc = string.Empty;
            using (var context = new VisionAppEntities(ConnectionString))
            {
                makeDesc = context.Rapa2_Make.FirstOrDefault(m => m.MakeIsoCode == makeIsoCode).MakeDesc;
            }
            return makeDesc;
        }

        public Rapa2LimitsDto GetRapa2Limits(string state)
        {
            Rapa2LimitsDto Limits = new Rapa2LimitsDto();
            Limits.MSRPLimit = Configuration.Rapa2DefaultMSRPLimit; //Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Rapa2DefaultMSRPLimit"]);
            Limits.WeightLimit = Configuration.Rapa2DefaultWeightLimit; //Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Rapa2DefaultWeightLimit"]);
            Limits.UseCappedSymbols = Configuration.Rapa2DefaultUseCappedSymbols; //Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Rapa2DefaultUseCappedSymbols"]);
            Limits.Id = 0;
            Limits.State = state;
            using (var context = new VisionAppEntities(ConnectionString))
            {
                var v = context.Rapa2_Limits.FirstOrDefault(m => m.State == state);
                if (v != null)
                {
                    Limits.MSRPLimit = v.MSRPLimit;
                    Limits.WeightLimit = v.WeightLimit;
                    Limits.UseCappedSymbols = v.UseCappedSymbols;
                    Limits.Id = v.Id;
                    Limits.State = v.State;
                }
            }
            return Limits;
        }

        public string GetOldMakeDesc(string makeIsoCode)
        {
            string makeDesc = string.Empty;
            using (var context = new VisionAppEntities(ConnectionString))
            {
                makeDesc = context.VIN_Make.Single(m => m.MakeAbbr == makeIsoCode).Make;
            }
            return makeDesc;
        }

        public int GetOldMakeID(string makeIsoCode)
        {
            int makeID = 0;
            using (var context = new VisionAppEntities(ConnectionString))
            {
                makeID = context.VIN_Make.Single(m => m.MakeAbbr == makeIsoCode).MakeID;
            }
            return makeID;
        }
        public int Rapa2WriteAuditHdr(string vsr, Rapa2VinResponseDto response, int quoteId, string policyNbr, string vin, string rapaparm)
        {
            using (var context = new VisionAppEntities(ConnectionString))
            {
                Rapa2_VinSearchAuditHdr Hdr = new Rapa2_VinSearchAuditHdr();
                if (response.Header != null)
                {
                    Hdr.TransactionID = response.Header.TransactionId;
                }
                else { Hdr.TransactionID = string.Empty; }
                Hdr.Vin = vin;
                Hdr.PolicyNbr = policyNbr;
                Hdr.QuoteId = quoteId;
                Hdr.Response = vsr;
                Hdr.RapaParm = rapaparm;
                Hdr.CreateDate = DateTime.Now;
                context.Rapa2_VinSearchAuditHdr.Add(Hdr);
                context.SaveChanges();
                int id = Hdr.HdrId;
                return id;
            }
        }

        public void Rapa2WriteAuditDetail(Rapa2VinResponseDto response, int hdrId, string vin)
        {
            bool selected = false;
            if (response.Body.Count() == 1) { selected = true; }
            int sequence = 0;
            using (var context = new VisionAppEntities(ConnectionString))
            {
                Rapa2_VinSearchAuditDtl Dtl = new Rapa2_VinSearchAuditDtl();

                foreach (Body body in response.Body)
                {
                    sequence++;
                    Dtl.HdrId = hdrId;
                    Dtl.Selected = selected;
                    Dtl.VIN = body.Vehicle.VIN;
                    Dtl.MakeCode = GetOldMakeID(body.Vehicle.Make);
                    Dtl.BasicModelName = body.Vehicle.BasicModelName;
                    Dtl.ModelYear = int.TryParse(body.Vehicle.ModelYear, out int i) ? i : 0;
                    Dtl.DistributionDate = int.TryParse(body.Vehicle.DistributionDate, out int dd) ? dd : 0;
                    Dtl.Restraint = body.Vehicle.Restraint;
                    Dtl.AntiLockBrakes = body.Vehicle.AntiLockBrakes;
                    Dtl.EngineCylinders = body.Vehicle.EngineCylinders;
                    Dtl.EngineType = body.Vehicle.EngineType;
                    Dtl.BodyStyle = body.Vehicle.BodyStyle;
                    Dtl.EngineSize = decimal.TryParse(body.Vehicle.EngineSize, out decimal es) ? es : 0;
                    Dtl.FourWheelDriveIndicator = body.Vehicle.FourWheelDriveIndicator;
                    Dtl.ElectronicStabilityControl = body.Vehicle.ElectronicStabilityControl;
                    Dtl.FullModelName = body.Vehicle.FullModelName;
                    Dtl.ClassCode = int.TryParse(body.Vehicle.ClassCode, out int cc) ? cc : 0;
                    Dtl.AntiTheftIndicator = body.Vehicle.AntiTheftIndicator;
                    Dtl.CurbWeight = int.TryParse(body.Vehicle.CurbWeight, out int cw) ? cw : 0;
                    Dtl.GrossVehicleWeight = int.TryParse(body.Vehicle.GrossVehicleWeight, out int gvw) ? gvw : 0;
                    Dtl.Horsepower = int.TryParse(body.Vehicle.Horsepower, out int hp) ? hp : 0;
                    Dtl.StateException = body.Vehicle.StateException;
                    Dtl.VMPerformanceIndicator = body.Vehicle.VMPerformanceIndicator;
                    Dtl.SpecialHandlingIndicator = body.Vehicle.SpecialHandlingIndicator;
                    Dtl.InterimIndicator = body.Vehicle.InterimIndicator;
                    Dtl.SpecialInfoSelector = body.Vehicle.SpecialInfoSelector;
                    Dtl.ModelSeriesInfo = body.Vehicle.ModelSeriesInfo;
                    Dtl.BodyInfo = body.Vehicle.BodyInfo;
                    Dtl.EngineInfo = body.Vehicle.EngineInfo;
                    Dtl.RestraintInfo = body.Vehicle.RestraintInfo;
                    Dtl.ReleaseDate = int.TryParse(body.Vehicle?.ReleaseDate, out int rd) ? rd : 0;
                    Dtl.Sequence = sequence;
                    Dtl.MSRP = int.TryParse(body.Vehicle.BaseMSRP, out int ms) ? ms : 0; ;
                    context.Rapa2_VinSearchAuditDtl.Add(Dtl);
                    context.SaveChanges();

                }
            }

        }

        public void Rapa2UpdateSelectedRecord(int hdrId, int Seq)
        {

            using (var context = new VisionAppEntities(ConnectionString))
            {
                var dtlToUpdate = context.Rapa2_VinSearchAuditDtl.Where(h => h.HdrId == hdrId && h.Sequence == Seq);
                foreach (var dtl in dtlToUpdate) { dtl.Selected = true; }
                context.SaveChanges();
            }
        }
        public void SetSelectedRapa2Vin(int quoteid, string vin, int seq)
        {
            using (var context = new VisionAppEntities(ConnectionString))
            {
                context.Procedures.spRAPA2UpdateSelectedDetailAsync(quoteid, vin, seq);
            }
        }
    }
}
