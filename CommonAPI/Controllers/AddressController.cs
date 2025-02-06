using System.Net;
using CommonAPICommon;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using CommonAPIBusinessLayer.Services.Impl;

namespace CommonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly SystemConfigurationManager _configuration;
        public AddressController(ILogger<TestController> logger, SystemConfigurationManager config)
        {
            _logger = logger;
            _configuration = config;
        }

        [HttpGet]
        public async Task<IActionResult> Verify(JObject addressobject)
        {
            dynamic json = addressobject;

            var address = new string[7];
            address[0] = json.AddressLine1;
            address[1] = json.AddressLine2;
            address[2] = "";
            address[3] = json.City;
            address[4] = json.State;
            address[5] = json.Zip;
            address[6] = "";

            //addressverificationservice under construction
            //var result = new CommonAPIBusinessLayer.Services.Impl.AddressVerificationService().VerifyAddress(address); 
            //return Ok(result);

            return Ok();
        }
    }
}
