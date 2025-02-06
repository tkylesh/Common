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
    public class PrefillController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly SystemConfigurationManager _configuration;
        private IISOService ISOBll { get; set; }

        //PrefillV1Controller on old version
        public PrefillController(ILogger<TestController> logger, SystemConfigurationManager config)
        {
            _logger = logger;
            _configuration = config;
            ISOBll = new ISOService();
        }

        //[HttpPost("[controller]/")]
        //public Task<IActionResult> KeepAlive()
        //{
        //    // Endpoint to be called by service to keep from going to sleep.
        //    return Ok("I'm still alive");   //may not need?
        //}

        [HttpPost("[controller]/{quoteId:int}")]
        public async Task<IActionResult> Post(JObject applicant, int quoteId)
        {
            var applicantDto = JsonConvert.DeserializeObject<ApplicantDto>(applicant.ToString());
            return Ok(ISOBll.CallISOPrefill(applicantDto, quoteId));
        }
    }
}

