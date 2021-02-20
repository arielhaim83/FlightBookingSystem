using Flight.Api.Infrastructure.Dtos;
using Flight.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight.Api.ViewModel
{
    public class CheckinViewModel
    {
        public string PassengerName { get; set; }
        public int FlightId { get; set; }
        public string BookingSessionId { get; set; }
        public List<BaggageDto> Baggages { get; set; }
    }
}
