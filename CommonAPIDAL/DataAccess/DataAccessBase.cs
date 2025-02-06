using CommonAPICommon;
using CommonAPIDAL.AlfaVisionWebModels;
using CommonAPIDAL.VisionAppModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIDAL.DataAccess
{
    public class DataAccessBase
    {
        SystemConfigurationManager _systemConfigurationManager;
        public DataAccessBase()
        {
            _systemConfigurationManager = new SystemConfigurationManager();
        }
        public DbContextOptions<VisionAppEntities> VisionAppConnectionString
        {
            get
            {
                DbContextOptionsBuilder<VisionAppEntities> optionsBuilder = new DbContextOptionsBuilder<VisionAppEntities>();
                optionsBuilder.UseSqlServer(_systemConfigurationManager.VisionAppConnectionString, options => options.EnableRetryOnFailure
                                                (
                                                    maxRetryCount: 5,
                                                    maxRetryDelay: TimeSpan.FromSeconds(30),
                                                    errorNumbersToAdd: null)
                                                );

                return optionsBuilder.Options;
            }
        }

        public DbContextOptions<AlfaVisionWebEntities> AlfaVisionWebConnectionString
        {
            get
            {
                DbContextOptionsBuilder<AlfaVisionWebEntities> optionsBuilder = new DbContextOptionsBuilder<AlfaVisionWebEntities>();
                optionsBuilder.UseSqlServer(_systemConfigurationManager.AlfaVisionWebConnectionString, options => options.EnableRetryOnFailure
                                                (
                                                    maxRetryCount: 5,
                                                    maxRetryDelay: TimeSpan.FromSeconds(30),
                                                    errorNumbersToAdd: null)
                                                );
                return optionsBuilder.Options;
            }
        }
    }
}
