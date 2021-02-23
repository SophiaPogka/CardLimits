using CardLimits.Core.Config.Extensions;
using CardLimits.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace CardLimits.Migrations
{
    public class DbContextFactory : IDesignTimeDbContextFactory<CardDbContext>
    {
        public CardDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
                .AddJsonFile("appsettings.json", false)
                .Build();

            var config = configuration.ReadAppConfiguration();

            var optionsBuilder = new DbContextOptionsBuilder<CardDbContext>();

            optionsBuilder.UseSqlServer(
                config.ConnectionString,
                options => {
                    options.MigrationsAssembly("CardLimits.Migrations");
                });

            return new CardDbContext(optionsBuilder.Options);
        }
    }
}
