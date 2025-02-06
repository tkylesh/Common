using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

//using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;
using CommonAPICommon;
using CommonAPIDAL.BBDBModels;
using CommonAPIDAL.VisionAppModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;

namespace CommonAPIDAL.DataAccess
{
    public class VehicleScoringDataAccess
    {

        private static DbContextOptions<BBDBEntities> BBDBConnectionString
        {
            get
            {
                DbContextOptions<BBDBEntities> optionsBuilder = new DbContextOptions<BBDBEntities>();
                return optionsBuilder;
            }
        }
        internal static int StoreRawRMXML(int quoteId, VehicleRiskResultsDto vsr, string xmlFromRM, double responseTime, string errorMessage,
                                            string xmlToRM, string jsTo, string jsFrom)
        {
            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                //OutputParameter<int> retVal = new OutputParameter<int>("RetVal", 0);

                BBDBModels.OutputParameter<int?> retVal = new BBDBModels.OutputParameter<int?>();
                context.Procedures.uspVSRawXML_InsertAsync(quoteId, string.Format("{0:Mdyyyyhhmmsstt}.xml", DateTime.Now), "",
                                            (int)responseTime, "", "ALFV0000" + string.Format("{0:yyyymmddhhmmssfff}", DateTime.Now), errorMessage, jsTo, jsFrom,  retVal);

                return Convert.ToInt32(retVal.Value);
            }
        }

        internal static void StoreRMMaster(int hdrId, VehicleRiskResultsDto vsr, int quoteId, string requestState)
        {
            bool IsWebSource = true;
            if (quoteId == 0)
            {
                IsWebSource = false;
            }

            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                if (vsr.VehicleRiskResults != null)
                {
                    for (int i = 0; i < vsr.VehicleRiskResults.Count(); i++)
                    {
                        var rmtVehicleScores = new RMTVehicleScores();
                        rmtVehicleScores.VIN = vsr.VehicleRiskResults[i].VIN;
                        rmtVehicleScores.VINScore = string.IsNullOrEmpty(vsr.VehicleRiskResults[i].Results) ? "" : vsr.VehicleRiskResults[i].Results;
                        rmtVehicleScores.OrderDate = DateTime.Now;
                        rmtVehicleScores.HdrId = hdrId;
                        rmtVehicleScores.ScoreError = string.IsNullOrWhiteSpace(vsr.VehicleRiskResults[i].ErrorMessage) ? "" : vsr.VehicleRiskResults[i].ErrorMessage;
                        if (IsWebSource)
                        {
                            rmtVehicleScores.Source = "W";
                        }
                        else
                        {
                            rmtVehicleScores.Source = "A";
                        }
                        rmtVehicleScores.Quoteid = quoteId;
                        rmtVehicleScores.RequestState = Convert.ToByte(requestState);
                        context.RMTVehicleScores.Add(rmtVehicleScores);
                    }
                    context.SaveChanges();
                }
            }
        }
        internal static VehicleScoreDto CheckExistingVehicleScore(string VIN)
        {
            var Configuration = new SystemConfigurationManager();
            var vsNoHitDefault = Configuration.VSNoHitDefault; //ConfigurationManager.AppSettings["VSNoHitDefault"];
            var vsInvalidVINDefault = Configuration.VSInvalidVINDefault; //ConfigurationManager.AppSettings["VSInvalidVINDefault"];
            VehicleScoreDto vsDto = new VehicleScoreDto();

            using (var context = new BBDBEntities(BBDBConnectionString))
            {
                var rmtVs = context.RMTVehicleScores.Where(v => v.VIN == VIN).OrderByDescending(o => o.OrderDate).FirstOrDefault();

                if (rmtVs == null)
                {
                    if (VIN.Length < 11)
                    {
                        vsDto.VehicleId = VIN;
                        vsDto.Score = vsNoHitDefault;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (rmtVs.OrderDate > DateTime.Now.AddDays(-30))
                    {
                        vsDto.VehicleId = rmtVs.VIN;
                        vsDto.Score = string.IsNullOrEmpty(rmtVs.VINScore) ? vsInvalidVINDefault : rmtVs.VINScore;
                        vsDto.ErrorMessage = string.IsNullOrWhiteSpace(rmtVs.ScoreError) ? string.Empty : rmtVs.ScoreError;
                        vsDto.hdrId = rmtVs.HdrId;
                        vsDto.ScoreDate = rmtVs.OrderDate.ToShortDateString();
                    }
                    else
                    {
                        return null;
                    }
                }
                return vsDto;
            }
        }

    }
}
