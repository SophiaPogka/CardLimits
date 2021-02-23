using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardLimits.Core.Config.Extensions
{
    public static class ConfigurationExtensions
    {
        public static AppConfig ReadAppConfiguration(
            this IConfiguration @this)
        {
            var connectionString = @this.GetConnectionString("myDatabase");

            return new AppConfig()
            {
                ConnectionString = connectionString
            };
        }
    }
}
