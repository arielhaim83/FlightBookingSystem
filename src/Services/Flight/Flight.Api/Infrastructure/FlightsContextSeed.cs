namespace Flight.API.Infrastructure
{
    using Flight.Domain.Entities;
    using Flight.Infrastructure;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Polly;
    using Polly.Retry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class FlightsContextSeed
    {
        public async Task SeedAsync(FlightsContext context, ILogger<FlightsContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(FlightsContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                             
                using (context)
                {
                    context.Database.Migrate();
                   
                    if (!context.Flights.Any())
                    {
                        context.Flights.AddRange(GetFlights());

                        await context.SaveChangesAsync();
                    }                                       
                }
            });
        }

        private IEnumerable<Flight> GetFlights()
        {
            return new List<Flight> {
                new Flight
                (   
                    GetPlanes().First(),
                    100,
                    2,
                    70,
                    DateTime.UtcNow                    
                )
            };
        }

        private IEnumerable<Plane> GetPlanes()
        {
            return new List<Plane> { 
                new Plane
                (                  
                    "Boeing 777",                    
                    100
                )
            };
        }       
               
        private AsyncRetryPolicy CreatePolicy(ILogger<FlightsContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}
