using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
//using System.Data.Entity.Core.Objects;
//using System.Data.Entity;
using CommonAPIDAL.VisionAppModels;
using Microsoft.EntityFrameworkCore;



namespace CommonAPIDAL.DataAccess
{
    public class ProgramMasterDataAccess
    {
        private static DbContextOptions<VisionAppEntities> VisionAppConnectionString
        {
            get
            {
                DbContextOptions<VisionAppEntities> optionsBuilder = new DbContextOptions<VisionAppEntities>();
                return optionsBuilder;

                //return ConfigurationManager.ConnectionStrings["VisionAppEntities"].ConnectionString;
            }
        }
        public ProgramMaster GetProgramMaster(int CarrierID, string State, string PolicyType)
        {
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                return context.ProgramMaster.Where(w => w.CarrierID == CarrierID && w.State == State && w.PolType == PolicyType).SingleOrDefault();
            }
        }

        public ProgramMaster GetProgramMaster(int QuoteID)
        {
            ProgramMaster rtnProgramMaster = null;
            //Staging_Policy policy;
            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                var policy = context.Staging_Policy.Where(w => w.QuoteID == QuoteID).SingleOrDefault();

                if (policy != null)
                {
                    if (policy.ProgramRevID > 0)
                    {
                        rtnProgramMaster = policy.ProgramRev.Program;
                    }
                }
            }

            return rtnProgramMaster;
        }

        public ProgramMaster GetProgramMasterByRevisionID(int ProgramRevisionID)
        {
            ProgramMaster rtnProgramMaster = null;

            using (var context = new VisionAppEntities(VisionAppConnectionString))
            {
                var prgRevision = context.ProgramRevision.Where(w => w.ProgramRevID == ProgramRevisionID).SingleOrDefault();

                if (prgRevision != null)
                {
                    if (prgRevision.ProgramRevID > 0)
                    {
                        rtnProgramMaster = context.ProgramMaster.Where(w => w.ProgramID == ProgramRevisionID).SingleOrDefault();
                        //rtnProgramMaster = prgRevision.ProgramMaster;
                    }
                }
            }

            return rtnProgramMaster;
        }
    }
}