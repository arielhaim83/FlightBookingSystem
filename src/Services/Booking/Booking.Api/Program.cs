using System.IO;
using System.Net;
using Booking.Api.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Booking.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            CreateHostBuilder(args).Build().Run();
        }        

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>                
            WebHost.CreateDefaultBuilder(args)
             .ConfigureKestrel(options =>
             {
                 var configuration = GetConfiguration();
                 var ports = GetDefinedPorts(configuration);

                 options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
                 {
                     listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                 });

                 options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
                 {
                     listenOptions.Protocols = HttpProtocols.Http2;
                 });
             })
            .UseStartup<Startup>()
            .UseSerilog((builderContext, config) =>
            {
                config.WriteTo.Console();
            });    
        
        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();            

            return builder.Build();
        }

        private static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        {
            var grpcPort = config.GetValue("GRPC_PORT", 4101);
            var port = config.GetValue("PORT", 4100);
            return (port, grpcPort);
        }
    }
}
