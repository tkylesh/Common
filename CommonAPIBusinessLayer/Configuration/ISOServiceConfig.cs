using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIBusinessLayer.Configuration
{
    public class ISOServiceConfig
    {
        private readonly IConfiguration _configuration;

        public ISOConfig Settings { get; }

        public ISOServiceConfig(IConfiguration configuration)
        {
            _configuration = configuration;
            //Move retrieval of this config setting to systemconfigurationmanager???
            Settings = _configuration.GetSection("ISOSettings").Get<ISOConfig>();
        }
    }
}
