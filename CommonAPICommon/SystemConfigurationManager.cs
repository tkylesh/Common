using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CommonAPICommon
{
    public sealed class SystemConfigurationManager
    {
        IConfigurationRoot config = null;

        public SystemConfigurationManager()
        {
            config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();

            //Example on how to get a whole section
            //var section = config.GetSection("WeatherClientConfig");
        }

        public string VSNoHitDefault
        {
            get
            {
                if (config == null)
                    return config.GetValue<string>("VSNoHitDefault");

                return "";
            }
        }

        public string VSInvalidVINDefault
        {
            get
            {
                if (config == null)
                    return config.GetValue<string>("VSInvalidVINDefault");

                return "";
            }
        }

        public Int32 Rapa2DefaultMSRPLimit
        {
            get
            {
                if (config == null)
                    return config.GetValue<Int32>("Rapa2DefaultMSRPLimit");

                return 0;
            }
        }
        public Int32 Rapa2DefaultWeightLimit
        {
            get
            {
                if (config == null)
                    return config.GetValue<Int32>("Rapa2DefaultWeightLimit");

                return 0;
            }
        }
        public bool Rapa2DefaultUseCappedSymbols
        {
            get
            {
                if (config == null)
                    return config.GetValue<bool>("Rapa2DefaultUseCappedSymbols");

                return false;
            }
        }

        public string VisionAppConnectionString
        {
            get
            {
                return config.GetConnectionString("VisionApp");
            }
        }

        public string AlfaVisionWebConnectionString
        {
            get
            {
                return config.GetConnectionString("AlfaVisionWeb");
            }
        }
    }
}