using CommonAPICommon;
using Microsoft.AspNetCore.Mvc;

namespace CommonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly SystemConfigurationManager _configuration;

        public TestController(ILogger<TestController> logger, SystemConfigurationManager config)
        {
            _logger = logger;
            _configuration = config;
        }
        [HttpGet("[controller]/log")]
        public async Task<IActionResult> testlog()
        {
            _logger.LogDebug("tested");
            return Ok("true");
        }
    }
}
