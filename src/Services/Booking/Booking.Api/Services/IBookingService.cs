using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Api.Services
{
    public interface IBookingService
    {
        Task<bool> InitializeFlightAsync(int flightId, int availableSeats);
        Task<string> StartBookingSessionAsync(int flightId);
        Task<long> DecreaseFilghtAvailableSeatCount(string flightId);
        Task<long> IncreaseFilghtAvailableSeatCount(string flightId);
        Task ValidateAndEndBookingSessionAsync(int flightId, string bookingSessionId);

    }
}
