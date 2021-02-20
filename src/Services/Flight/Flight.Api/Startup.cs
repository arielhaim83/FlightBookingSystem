using System;
using System.Reflection;
using AutoMapper;
using Flight.Api.Configuration;
using Flight.Api.Infrastructure;
using Flight.Api.Infrastructure.Filters;
using Flight.Api.Services;
using Flight.Domain.Entities;
using Flight.Infrastructure;
using Flight.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Flight.Api
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
            services.AddTransient<GrpcExceptionInterceptor>();
            services.AddGrpcClient<GrpcBooking.Booking.BookingClient>((services, options) =>
            {
                var bookingApi = services.GetRequiredService<IOptions<UrlsConfig>>().Value.GrpcBookingService;
                options.Address = new Uri(bookingApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddControllers(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                    options.Filters.Add(typeof(ValidateModelStateFilter));

                }).AddNewtonsoftJson(options =>
                {
                      options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flight.Api", Version = "v1" });
            });

            services.AddDbContext<FlightsContext>(options =>
            {
                options.UseNpgsql(Configuration["ConnectionString"],
                  npgServerOptions =>
                  {
                      npgServerOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                  });
            },
                ServiceLifetime.Scoped
            );

            services.Configure<UrlsConfig>(Configuration.GetSection("urls"));
            services.AddScoped<IFlightRepository, FlightRepository>();
            services.AddScoped<IPassengerRepository, PassengerRepository>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddOptions();

            var mapperConfig = new MapperConfiguration(mc => {
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flight.Api v1"));
            }
            
            app.UseRouting();            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
