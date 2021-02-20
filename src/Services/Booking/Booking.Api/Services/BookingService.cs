using Booking.Api.Configuration;
using Booking.Api.Infrastructure.Exceptions;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Booking.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly BookingSettings _settings;
        private readonly IDatabase _database;
        

        public BookingService(ConnectionMultiplexer redis, 
            IOptions<BookingSettings> settings)
        {
            _redis = redis;
            _settings = settings.Value;
            _database = redis.GetDatabase();                        
        }
       
        async public Task<string> StartBookingSessionAsync(int flightId)
        {
            var seats = await DecreaseFilghtAvailableSeatCount(flightId.ToString());

            if (seats < 0)
            {
                throw new BookingDomainException($"Flight {flightId} has no available seats");
            }

            var bookingSessionId = Guid.NewGuid().ToString();
            await _database.StringSetAsync($"booking:{flightId}:{bookingSessionId}", 
                1, 
                TimeSpan.FromMinutes(_settings.BookingSessionDurationInMinutes));

            return bookingSessionId;
        }

        async public Task<bool> InitializeFlightAsync(int flightId, int availableSeats)
        {
            return await _database.StringSetAsync($"flight:{flightId}", availableSeats);
        }

        public async Task<long> DecreaseFilghtAvailableSeatCount(string flightId)
        {
            return await _database.StringDecrementAsync($"flight:{flightId}");
        }

        public async Task<long> IncreaseFilghtAvailableSeatCount(string flightId)
        {
            return await _database.StringIncrementAsync($"flight:{flightId}");
        }

        public async Task ValidateAndEndBookingSessionAsync(int flightId, string bookingSessionId)
        {
            var session = await _database.StringGetAsync($"booking:{flightId}:{bookingSessionId}");

            if (session == RedisValue.Null)
            {
                throw new BookingDomainException("Session expired");
            }

            await _database.KeyDeleteAsync($"booking:{flightId}:{bookingSessionId}");
        }
    }
}
