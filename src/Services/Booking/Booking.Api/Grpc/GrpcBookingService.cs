using Booking.Api.Infrastructure.Exceptions;
using Booking.Api.Services;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcBooking
{
    public class GrpcBookingService : Booking.BookingBase
    {
        private readonly IBookingService _bookingService;

        public GrpcBookingService(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        public override async Task<InitializeFlightResponse> InitializeFlight(InitializeFlightRequest request, ServerCallContext context)
        {
            var created = await _bookingService.InitializeFlightAsync(request.FlightId, request.AvailableSeats);

            if (!created)
            {
                context.Status = new Status(StatusCode.Internal, $"There was an error ehile initializing flight {request.FlightId}");
            }

            return new InitializeFlightResponse();
        }

        public override async Task<StartBookingSessionResponse> StartBookingSession(StartBookingSessionRequest request, ServerCallContext context)
        {
            try
            {
                var bookingSessionId = await _bookingService.StartBookingSessionAsync(request.FlightId);
                return new StartBookingSessionResponse
                {
                    BookingId = bookingSessionId
                };
            }
            catch (BookingDomainException ex)
            {
                context.Status = new Status(StatusCode.OutOfRange, ex.Message);
                return new StartBookingSessionResponse();
            }           
        }

        public override async Task<ValidateBookingSessionResponse> ValidateAndEndBookingSession(ValidateBookingSessionRequest request, ServerCallContext context)
        {
            try
            {
                await _bookingService.ValidateAndEndBookingSessionAsync(request.FlightId, request.BookingId);
            }
            catch (BookingDomainException ex)
            {
                context.Status = new Status(StatusCode.NotFound, ex.Message);
            }

            return new ValidateBookingSessionResponse();
        }
    }
}
