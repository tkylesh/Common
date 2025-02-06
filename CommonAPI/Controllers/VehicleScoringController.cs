using System.Net;
using System.Xml;
using CommonAPIBusinessLayer.Services.Impl;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon;
using CommonAPICommon.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleScoringController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly SystemConfigurationManager _configuration;
        private IVehicleScoreService _service { get; set; }


        public VehicleScoringController(ILogger<TestController> logger, SystemConfigurationManager config)
        {
            _logger = logger;
            _configuration = config;
            _service = new VehicleScoreService();
        }

        // GET: api/VehicleScoring
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        // GET: api/VehicleScoring/5
        [Route("[controller]/{id}")]
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("[controller]/")]      //vehiclescore?vin=1&98687678
        public async Task<IActionResult> SendRequestforVehicleScore(int quoteid, [FromBody] List<VINItem> vins)
        {
            var headers = Request.Headers;
            VehicleScoreResultsDto vehs = new VehicleScoreResultsDto();

            vehs = _service.GetVehicleScores(quoteid, vins, null);
            return Ok(vehs);

            //old version has a try/catch error
        }

        [HttpPost("[controller]/xml")]
        public async Task<IActionResult> SendRequestforVehicleScoreXml(HttpRequestMessage request)
        {
            string rescoreflag = string.Empty;
            var rescoreheader = Request.Headers.FirstOrDefault(x => x.Key == "Rescore");
            if (rescoreheader.Value.Count() > 0)
            {
                rescoreflag = rescoreheader.Value.FirstOrDefault().ToUpper();
            }

            var contenttype = HttpContext.Request.ContentType; //Request.Content.Headers.ContentType.MediaType;    //HttpHeader
            var data = request.Content.ReadAsStringAsync().Result;
            if (string.IsNullOrEmpty(data))
            {
                return BadRequest();
            }

            var vinitems = new VINAS400RequestDto();
            if (contenttype.Contains("xml"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data);

                data = JsonConvert.SerializeXmlNode(xmlDoc);
                if (data.Contains("["))
                {
                    data = data.Replace("@", "").Replace("{\"REQUEST\":", "").Replace("\"VEHICLE\":", "\"VINS\":").Replace("}}", "}");
                }
                else
                {
                    data = data.Replace("@", "").Replace("{\"REQUEST\":", "").Replace("\"VEHICLE\":", "\"VINS\":[").Replace("}}}", "}]}").Replace(",\"POLICYNUMBER", "],\"POLICYNUMBER").Replace("}}", "}");
                }

                vinitems = JsonConvert.DeserializeObject<VINAS400RequestDto>(data);
                VehicleScoreResultsDto vehs = new VehicleScoreResultsDto();

                if (rescoreflag == "Y")
                {
                    vehs = _service.GetVehicleScores(0, vinitems.vins, vinitems.PolicyNumber, rescoreflag);
                }
                else
                {
                    vehs = _service.GetVehicleScores(0, vinitems.vins, vinitems.PolicyNumber);
                }

                return Ok(vehs);

                //old version has try/catch error
            }

            return BadRequest("Missing Headers");
        }        
    }
}
