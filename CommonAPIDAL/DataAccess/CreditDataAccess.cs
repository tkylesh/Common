using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPIDAL.AlfaVisionWebModels;
using CommonAPIDAL.BBDBModels;
using CommonAPIDAL.VisionAppModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;

namespace CommonAPIDAL.DataAccess
{
    internal class CreditDataAccess
    {
        private static DbContextOptions<BBDBEntities> BBDBConnectionString
        {
            get
            {
                DbContextOptions<BBDBEntities> optionsBuilder = new DbContextOptions<BBDBEntities>();
                return optionsBuilder;
            }
        }

        private static DbContextOptions<AlfaVisionWebEntities> AlfaVisionWebConnectionString
        {
            get
            {
                DbContextOptions<AlfaVisionWebEntities> optionsBuilder = new DbContextOptions<AlfaVisionWebEntities>();
                return optionsBuilder; ;
            }
        }

        internal static int SearchCreditOrderBySSN_DL(dynamic searchInfo)
        {
            int rmId = 0;
            string ssn = searchInfo.SSN;
            string ln = searchInfo.LicenseNumber;
            DateTime start = searchInfo.DOB.AddDays(-1);
            DateTime dob = searchInfo.DOB.AddDays(1);
            DateTime cutOffDate = DateTime.Now.AddDays(-30);

            using (var con = new BBDBEntities(BBDBConnectionString))
            {
                var resultmaster = con.ResultMaster.Where(rm => rm.NI_SSN == ssn && rm.NI_LicenseNum == ln &&
                                                                rm.NI_DOB > start && rm.NI_DOB < dob && rm.OrderDate >= cutOffDate).OrderByDescending(o => o.rmID).FirstOrDefault();

                if (resultmaster != null)
                {
                    rmId = resultmaster.rmID;
                }
                //rmId = (from rm in con.ResultMasters
                //        where rm.NI_SSN == ssn && rm.NI_LicenseNum == ln && rm.NI_DOB == dob && rm.OrderDate >= cutOffDate
                //        select rm.rmID).SingleOrDefault();
            }

            return rmId;
        }

        internal static int SearchCreditOrderBySSN(dynamic searchInfo)
        {
            int rmId = 0;
            string ssn = searchInfo.SSN;
            ssn = ssn.Replace("-", "");
            DateTime start = searchInfo.DOB.AddDays(-1);
            DateTime dob = searchInfo.DOB.AddDays(1);
            DateTime cutOffDate = DateTime.Now.AddDays(-30);

            using (var con = new BBDBEntities(BBDBConnectionString))
            {
                var resultmaster = con.ResultMaster.Where(rm => rm.NI_SSN == ssn &&
                                                                rm.NI_DOB > start && rm.NI_DOB < dob && rm.OrderDate >= cutOffDate).OrderByDescending(o => o.rmID).FirstOrDefault();

                if (resultmaster != null)
                {
                    rmId = resultmaster.rmID;
                }
                //rmId = (from rm in con.ResultMasters
                //        where rm.NI_SSN == ssn && rm.NI_DOB == dob && rm.OrderDate >= cutOffDate
                //        select rm.rmID).SingleOrDefault();
            }
            return rmId;
        }

        internal static int SearchCreditOrderByDL(dynamic searchInfo)
        {
            int rmId = 0;
            string ln = searchInfo.LicenseNumber;
            DateTime start = searchInfo.DOB.AddDays(-1);
            DateTime dob = searchInfo.DOB.AddDays(1);
            DateTime cutOffDate = DateTime.Now.AddDays(-30);

            using (var con = new BBDBEntities(BBDBConnectionString))
            {
                var resultmaster = con.ResultMaster.Where(rm => rm.NI_LicenseNum == ln &&
                                                                rm.NI_DOB > start && rm.NI_DOB < dob && rm.OrderDate >= cutOffDate).OrderByDescending(o => o.rmID).FirstOrDefault();

                if (resultmaster != null)
                {
                    rmId = resultmaster.rmID;
                }
                //rmId = (from rm in con.ResultMasters
                //        where rm.NI_LicenseNum == ln && rm.NI_DOB == dob && rm.OrderDate >= cutOffDate
                //        select rm.rmID).SingleOrDefault();
            }

            return rmId;
        }

        internal static int SearchByCreditOrderByName_Address(dynamic searchInfo)
        {
            int rmId = 0;
            string nameFirst = searchInfo.NameFirst;
            string nameLast = searchInfo.NameLast;
            string address = searchInfo.StreetName.Length < 21 ? searchInfo.StreetName : searchInfo.StreetName.Substring(0, 20);
            string state = searchInfo.State;
            string city = searchInfo.City;
            string Zip = searchInfo.Zip;//substring this
            DateTime start = searchInfo.DOB.AddDays(-1);
            DateTime dob = searchInfo.DOB.AddDays(1);
            DateTime cutOffDate = DateTime.Now.AddDays(-30);

            using (var con = new BBDBEntities(BBDBConnectionString))
            {
                var resultmaster = con.ResultMaster.Where(rm => rm.NameFirst == nameFirst &&
                                                                rm.NameLast == nameLast &&
                                                                rm.Address == address && rm.City == city &&
                                                                rm.State == state && rm.Zip == Zip.Substring(0, 5) &&
                                                                rm.NI_DOB > start && rm.NI_DOB < dob && rm.OrderDate >= cutOffDate).OrderByDescending(o => o.rmID).FirstOrDefault();
                if (resultmaster != null)
                {
                    rmId = resultmaster.rmID;
                }
                //rmId = (from rm in con.ResultMasters
                //        where rm.NameFirst == nameFirst && rm.NameLast == nameLast &&
                //        rm.Address == address && rm.City == city &&
                //        rm.State == state && rm.Zip == Zip.Substring(0, 5) &&
                //        rm.NI_DOB == dob && rm.OrderDate >= cutOffDate
                //        select rm.rmID).SingleOrDefault();
            }

            return rmId;

        }
        internal static string GetCreditPolType(int rmid)
        {
            string retPolType = string.Empty;

            using (var con = new BBDBEntities(BBDBConnectionString))
            {
                var creditReport = con.CreditReport.Where(cr => cr.rmID == rmid).SingleOrDefault();


                if (creditReport != null)
                {
                    retPolType = creditReport.PolType;
                }

                return retPolType;
            }
        }
    }
}
