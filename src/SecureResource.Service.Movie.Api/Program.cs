using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Movies.API.Data;
using SecureResource.Service.Movie.Extensions;

namespace Movies.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            SeedDatabase(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                if (! context.HostingEnvironment.IsDevelopment())
                {
                    config.ConfigureProductionKeyVault();
                }
                else
                {
                    config.ConfigureDevelopmentKeyVault();
                }
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();

                string dsn = string.Empty;
                if (webBuilder.GetSetting("Environment") == Environments.Production)
                {
                    dsn = "https://d63100a0921f4ee1b1890a1da4f15cc8@o1295482.ingest.sentry.io/6521122";
                }
                webBuilder.UseSentry(o=>
                {
                    o.Dsn = dsn;
                    // When configuring for the first time, to see what the SDK is doing:
                    o.Debug = true;
                    // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
                    // We recommend adjusting this value in production.
                    o.TracesSampleRate = 1.0;
                });
            });

        private static void SeedDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var moviesContext = services.GetRequiredService<MoviesContext>();
                MoviesContextSeed.SeedAsync(moviesContext);
            }
        }

    }
}
