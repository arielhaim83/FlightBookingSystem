using GrpcBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight.Api.Services
{
    public interface IBookingService
    {
        Task<InitializeFlightResponse> InitializeFlight(int flightId, int availableSeats);
        Task<ValidateBookingSessionResponse> ValidateAndEndBookingSessionAsync(int flightId, string bookingId);
    }
}
