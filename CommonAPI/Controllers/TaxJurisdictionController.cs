using CommonAPIBusinessLayer.Services.Impl;
using CommonAPICommon;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace CommonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxJurisdictionController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly SystemConfigurationManager _configuration;

        public TaxJurisdictionController(ILogger<TestController> logger, SystemConfigurationManager config)
        {
            _logger = logger;
            _configuration = config;
        }

        [HttpPost]
        public async Task<IActionResult> GetTaxJurisdiction(JObject inputObject)
        {
            dynamic json = inputObject;
            return Ok(new TaxJurisdictionService().GetTaxJurisdiction(json.address.ToString(), json.city.ToString(), json.state.ToString(), json.zip.ToString(), json.effdate.ToString(), json.taxcodeselected.ToString()));
        }
    }
}
