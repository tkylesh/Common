using CommonAPIBusinessLayer.Services.Impl;
using CommonAPICommon;
using CommonAPICommon.Dto;
using JWT.Authorization.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly SystemConfigurationManager _configuration;

        public CreditController(ILogger<TestController> logger, SystemConfigurationManager config)
        {
            _logger = logger;
            _configuration = config;
        }

        [HttpGet("[controller]/")]
        public async Task<IActionResult> Post(JObject creditOrderDto)
        {
            //var headers = Request.Headers;
            //StringValues token;
            //headers.TryGetValue("Authorization", out token);    //HTTPHeaders
            var token = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            var jwttoken = TokenHelper.ParseToken(token.Value.First());
            var agent = JsonConvert.DeserializeObject<Dictionary<string, string>>(jwttoken["agent"].ToString());
            var order = JsonConvert.DeserializeObject<CreditOrderDto>(creditOrderDto.ToString());

            int rmId = new CreditOrderService().OrderCredit(order);
            if (rmId > 0)
            {
                return Ok(rmId);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
