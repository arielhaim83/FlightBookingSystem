using Flight.Api.Configuration;
using GrpcBooking;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight.Api.Services
{
    public class BookingService : IBookingService
    {        
        private readonly Booking.BookingClient _bookingClient;

        public BookingService(Booking.BookingClient bookingClient)
        {            
            _bookingClient = bookingClient;
        }

        public async Task<InitializeFlightResponse> InitializeFlight(int flightId, int availableSeats)
        {           
            return await _bookingClient.InitializeFlightAsync(new InitializeFlightRequest
            {
                FlightId = flightId,
                AvailableSeats = availableSeats
            });                       
        }

        public async Task<ValidateBookingSessionResponse> ValidateAndEndBookingSessionAsync(int flightId, string bookingId)
        {           
            return await _bookingClient.ValidateAndEndBookingSessionAsync(new ValidateBookingSessionRequest
            {
                FlightId = flightId,
                BookingId = bookingId
            });            
        }
    }
}
