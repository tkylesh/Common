using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon.Dto;
using CommonAPIDAL.Repository.Impl;
using CommonAPIDAL.Repository.Interface;
using Newtonsoft.Json;
using System.Configuration;
using CommonAPICommon;

namespace CommonAPIBusinessLayer.Services.Impl
{
    public class VehicleScoreService : IVehicleScoreService
    {

        private static readonly log2sql log = new log2sql(nameof(VehicleScoreService));
        private IVehicleScoringRepository DALVSRepository { get; set; }
        public VehicleScoreService()
        {
            DALVSRepository = new VehicleScoringRepository();
        }

        public VehicleScoreService(IVehicleScoringRepository dalDALVSRepository)
        {
            DALVSRepository = dalDALVSRepository;
        }
        public VehicleScoreResultsDto GetVehicleScores(int quoteid, List<VINItem> VINS, string polnum, string rescoreFlag = "N")
        {
            VehicleRiskResultsDto vsr = new VehicleRiskResultsDto();
            List<VehicleScoreDto> vss = new List<VehicleScoreDto>();
            VehicleScoreResultsDto vsrDto = new VehicleScoreResultsDto();
            string serInstance = string.Empty;
            string ret = string.Empty;
            TimeSpan ts = new TimeSpan();
            var requestState = string.Empty;
            if (polnum == null)
            {
                requestState = DALVSRepository.GetRequestState(quoteid);
            }
            else
            {
                requestState = polnum.Substring(3, 2);
            }
            try
            {
                var vsNoHitDefault = ConfigurationManager.AppSettings["VSNoHitDefault"];
                var vsInvalidVINDefault = ConfigurationManager.AppSettings["vsInvalidVINDefault"];
                List<VehicleScoreDto> vsCheckDtos = new List<VehicleScoreDto>();
                VINRequestDto unscoredvehs = new VINRequestDto();
                unscoredvehs.RequestState = requestState;
                foreach (VINItem veh in VINS)
                {
                    veh.vin = veh.vin.Trim();
                    VehicleScoreDto vsd = null;
                    if (rescoreFlag == "N")
                    {
                        vsd = DALVSRepository.CheckExistingVehicleScore(veh.vin);
                    }
                    if (vsd == null)
                    {
                        if (unscoredvehs.vins == null)
                        {
                            unscoredvehs.vins = new List<VINItem>();
                        }
                        unscoredvehs.vins.Add(veh);
                    }
                    else
                    {
                        vsCheckDtos.Add(vsd);
                    }

                }
                if (unscoredvehs.vins == null || unscoredvehs.vins.Count == 0)
                {
                    vsrDto.VehicleScoreResults = vsCheckDtos;
                    vsrDto.ErrorMessage = string.Empty;
                    return vsrDto;
                }

                serInstance = JsonConvert.SerializeObject(unscoredvehs);
                serInstance = serInstance.Replace("VIN", "vin").Replace("{\"", "{ \"");

                DateTime startTime = DateTime.Now;
                WebClient wc = new WebClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                wc.Headers.Clear();
                wc.Headers.Add("Content-Type", "application/json");
                wc.Headers.Add("Accept", "application/json");
                string baseuri = ConfigurationManager.AppSettings["BaseVehicleScoreURI"];
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                var request = baseuri + "api/VehicleScoring";
                ret = wc.UploadString(request, serInstance);
                ts = new TimeSpan(DateTime.Now.Ticks - startTime.Ticks);//Capturing Time Span to get a response from Red Mountain
                ServicePointManager.ServerCertificateValidationCallback = null;
                vsr = JsonConvert.DeserializeObject<VehicleRiskResultsDto>(ret);  // DeserializeVehicleScoreResponse(ret);
                int hdrId = DALVSRepository.StoreRawAndMaster(quoteid, vsr, "", ts.Ticks, vsr.ErrorMessage, "", serInstance, ret, unscoredvehs.RequestState);


                if (vsr.VehicleRiskResults != null)
                {
                    for (int i = 0; i < vsr.VehicleRiskResults.Count(); i++)
                    {
                        VehicleScoreDto vs = new VehicleScoreDto();

                        vs.VehicleId = vsr.VehicleRiskResults[i].VIN;
                        if (string.IsNullOrEmpty(vsr.VehicleRiskResults[i].ErrorMessage))
                        {
                            vs.Score = string.IsNullOrEmpty(vsr.VehicleRiskResults[i].Results) ? vsInvalidVINDefault : vsr.VehicleRiskResults[i].Results;
                        }
                        else
                        {
                            var s = vsr.VehicleRiskResults[i].ErrorMessage.ToLower();

                            if (s.StartsWith("invalid"))
                            {
                                vs.Score = string.IsNullOrEmpty(vsr.VehicleRiskResults[i].Results) ? vsInvalidVINDefault : vsr.VehicleRiskResults[i].Results;
                            }
                            else if (s.StartsWith("no historical activity") || s.StartsWith("no data available"))
                            {
                                vs.Score = string.IsNullOrEmpty(vsr.VehicleRiskResults[i].Results) ? vsNoHitDefault : vsr.VehicleRiskResults[i].Results;
                            }
                            else
                            {
                                vs.Score = string.IsNullOrEmpty(vsr.VehicleRiskResults[i].Results) ? vsInvalidVINDefault : vsr.VehicleRiskResults[i].Results;
                            }


                        }
                        vs.ScoreDate = DateTime.Now.ToShortDateString();
                        vs.hdrId = hdrId;
                        vs.ErrorMessage = string.IsNullOrEmpty(vsr.VehicleRiskResults[i].ErrorMessage) ? string.Empty : vsr.VehicleRiskResults[i].ErrorMessage;

                        vss.Add(vs);
                    }
                }
                else
                {
                    VehicleScoreDto vs = new VehicleScoreDto()
                    {
                        VehicleId = "",
                        Score = "",
                        hdrId = 0,
                        ErrorMessage = "",
                        ScoreDate = string.Empty
                    };

                    vss.Add(vs);
                }
                if (vsCheckDtos != null && vsCheckDtos.Count > 0)
                {
                    for (int i = 0; i < vsCheckDtos.Count; i++)
                    {
                        vss.Add(vsCheckDtos[i]);
                    }
                }
                vsrDto.VehicleScoreResults = vss;
                vsrDto.ErrorMessage = string.IsNullOrEmpty(vsr.ErrorMessage) ? string.Empty : vsr.ErrorMessage;
            }
            catch (Exception ex)
            {
                var eMsg = string.Empty;
                if (ex.InnerException == null)
                {
                    eMsg = ex.Message.ToString();
                }
                else
                {
                    eMsg = ex.InnerException.Message.ToString();
                }

                VehicleScoreDto vs = new VehicleScoreDto()
                {
                    VehicleId = "",
                    Score = "",
                    hdrId = 0,
                    ErrorMessage = ""
                };
                vss.Add(vs);
                vsrDto.VehicleScoreResults = vss;
                vsrDto.ErrorMessage = string.IsNullOrEmpty(eMsg) ? "" : eMsg;
                log.Error(eMsg, ex);
                int HdrId = 0;
                if (string.IsNullOrEmpty(vsr.ErrorMessage))
                {
                    HdrId = DALVSRepository.StoreRawAndMaster(quoteid, vsr, "", ts.Ticks, vsrDto.ErrorMessage, "", serInstance, ret, requestState);
                }
                else
                {
                    HdrId = DALVSRepository.StoreRawAndMaster(quoteid, vsr, "", ts.Ticks, vsr.ErrorMessage, "", serInstance, ret, requestState);
                }

            }
            return vsrDto;
        }
        public bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            //Note: You need an AppSettings key called IgnoreSslErrors in the web.config

            //System.Net.HttpWebRequest request = sender;
            //request.KeepAlive = false;

            return true;

        }
    }
}
