using CommonAPIBusinessLayer.Services.Impl;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon;
using CommonAPICommon.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoverageVerifierController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly SystemConfigurationManager _configuration;
        private IISOService ISOBll { get; set; }

        public CoverageVerifierController(ILogger<TestController> logger, SystemConfigurationManager config)
        {
            _logger = logger;
            _configuration = config;
            ISOBll = new ISOService();
        }        

        //CoverageVerifierV1Controller on old version

        [HttpPost("[controller]/{quoteId:int}")]
        public async Task<IActionResult> Post(JObject covVer, int quoteId)
        {
            var covVerDto = JsonConvert.DeserializeObject<CoverageVerifierDto>(covVer.ToString());
            return Ok(ISOBll.CallISOCV(covVerDto, quoteId));
        }
    }
}
