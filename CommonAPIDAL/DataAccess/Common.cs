using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;
using CommonAPIDAL.VisionAppModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;

namespace CommonAPIDAL.DataAccess
{
    public class Common
    {
        // private static readonly object UseISO;

        private static DbContextOptions<VisionAppEntities> ConnectionString
        {
            get
            {
                DbContextOptions<VisionAppEntities> optionsBuilder = new DbContextOptions<VisionAppEntities>();
                return optionsBuilder;
            }
        }
        //Returns all the makes from the VIN_Make table
        //These are not filtered by year.
        public static List<VehicleMakeDto> getMakesPrior1975(VisionAppEntities context)
        {

            //get all the records in the VIN_Make table as a distinct list
            var v = (from vm in context.VIN_Make
                     select new VehicleMakeDto
                     {
                         MakeAbbr = vm.MakeAbbr,
                         MakeName = vm.Make,
                         MakeId = vm.MakeID
                     }).Distinct().ToList();

            return v;
        }
        public static bool getISOproduct(int quoteId)
        {
            using (var con = new VisionAppEntities(ConnectionString))
            {

                var products = (from r in con.ProgramRevision
                                join sp in con.Staging_Policy on r.ProgramRevID equals sp.ProgramRevID
                                where sp.QuoteID == quoteId
                                select r.UseISO)
                                .SingleOrDefault();
                return products == null ? false : (bool)products;
            }

        }
        public static string GetRequestState(int quoteid)
        {
            string stateNumStr = string.Empty;

            using (var con = new VisionAppEntities(ConnectionString))
            {
                var sp = con.Staging_Policy.Where(p => p.QuoteID == quoteid).SingleOrDefault();
                if (sp != null)
                {
                    if (sp.FullPolNum != null)
                    {
                        stateNumStr = sp.FullPolNum.Substring(3, 2);
                    }
                    else
                    {
                        stateNumStr = sp.ProducerCode.Substring(0, 2);
                    }
                }
            }
            return stateNumStr;
        }
    }
}
