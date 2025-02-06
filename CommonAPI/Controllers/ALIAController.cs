using System.Net;
using CommonAPICommon;
using Microsoft.AspNetCore.Mvc;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon.Dto;
using Newtonsoft.Json;
using System.Xml;

namespace CommonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ALIAController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly SystemConfigurationManager _configuration;
        public ALIAController(ILogger<TestController> logger, SystemConfigurationManager config)
        {
            _logger = logger;
            _configuration = config;
        }
        private IALIAService _service { get; set; }

        [HttpGet("[controller]/AddNewMember")]
        public async Task<IActionResult> Post(int quoteId)
        {
            QuoteDataForNewMember user = _service.GetAFFNewMemberData(quoteId);
            string response = _service.AddNewMembershipNumber(user, quoteId);
            return Ok();
        }

        [HttpGet("[controller]/Validate")]
        public async Task<IActionResult> Get(string memberNumber, string zipCode, string quoteId)
        {
            string response = _service.CallAlfaWebService(memberNumber, zipCode, quoteId);
            return Ok();
        }

        [HttpGet("[controller]/Search")]
        public async Task<IActionResult> Get(int quote)
        {
            QuoteDataForMemberSearch user = _service.GetMemberSearchData(quote);
            string response = _service.MemberSearch(user, quote);
            return Ok();
        }

        [HttpPost("[controller]/ValidateXml")]
        public async Task<IActionResult> ValidateXML(HttpRequestMessage request)
        {
            var contenttype = HttpContext.Request.ContentType; //Request.Content.Headers.ContentType.MediaType;    //HTTPheaders
            var data = request.Content.ReadAsStringAsync().Result;
            if (string.IsNullOrEmpty(data))
            {
                return BadRequest();
            }
            var memberInfo = new AliaAs400MemberDto();
            if (contenttype.Contains("xml"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data);

                data = JsonConvert.SerializeXmlNode(xmlDoc);

                data = data.Replace("@", "").Replace("{\"REQUEST\":", "").Replace("}}", "}");

                memberInfo = JsonConvert.DeserializeObject<AliaAs400MemberDto>(data);
            }
            ALResults response = _service.CallAlfaWebServiceAs400(memberInfo.MembershipNumber, memberInfo.PolicyNumber);
            return Ok(response);
        }
    }
}
