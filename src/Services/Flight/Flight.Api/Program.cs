using Flight.API.Infrastructure;
using Flight.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Flight.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.MigrateDbContext<FlightsContext>((context, services) => {
                var logger = services.GetService<ILogger<FlightsContextSeed>>();

                new FlightsContextSeed()
                           .SeedAsync(context, logger)
                           .Wait();
            });
            host.Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((builderContext, config) => {
                    config.WriteTo.Console();
                });
    }
}
