using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CardLimits.Core.Config;
using CardLimits.Core.Config.Extensions;
using CardLimits.Core.Data;

using Microsoft.EntityFrameworkCore;


namespace CardLimits.Core.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppServices(
            this IServiceCollection @this, IConfiguration configuration)
        {
            @this.AddSingleton<AppConfig>(
                configuration.ReadAppConfiguration());

            // AddScoped
            @this.AddDbContext<CardDbContext>(
                (serviceProvider, optionsBuilder) => {
                    var appConfig = serviceProvider.GetRequiredService<AppConfig>();

                    optionsBuilder.UseSqlServer(appConfig.ConnectionString);
                });

            @this.AddScoped<ICardService, CardService>();
            @this.AddScoped<ILimitService, LimitService>();
        }

    }
}
