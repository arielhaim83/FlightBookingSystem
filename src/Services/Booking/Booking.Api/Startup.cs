using Booking.Api.Configuration;
using Booking.Api.Services;
using GrpcBooking;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Newtonsoft.Json;
using Serilog;
using Booking.Api.Infrastructure.Filters;

namespace Booking.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = true;
            });
          
            services.AddControllers(options=> 
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddNewtonsoftJson(
                  options =>
                  {
                      options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                  });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking.Api", Version = "v1" });
            });


            services.Configure<BookingSettings>(Configuration);
            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                string EXPIRED_KEYS_CHANNEL = "__keyevent@0__:expired";
                var settings = sp.GetRequiredService<IOptions<BookingSettings>>().Value;
                var configuration = ConfigurationOptions.Parse(settings.RedisConnectionString, true);

                configuration.ResolveDns = true;

                var connection =  ConnectionMultiplexer.Connect(configuration);

                ISubscriber subscriber = connection.GetSubscriber();
                subscriber.Subscribe(EXPIRED_KEYS_CHANNEL, (channel, key) => {
                    using (var scope = sp.CreateScope())
                    {
                        var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();
                        Log.Information($"Expired key {key}");

                        if (key.ToString().ToLower().StartsWith("booking"))
                        {
                            var flightId = key.ToString().Split(':')[1];
                            bookingService.IncreaseFilghtAvailableSeatCount(flightId);
                        }
                    }                    
                });
                
                return connection;
            });

            services.AddScoped<IBookingService, BookingService>();            
            services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking.Api v1"));
            }
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GrpcBookingService>();
                endpoints.MapControllers();
            });
        }
    }
}
