using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Api.Configuration
{
    public class BookingSettings
    {
        public string RedisConnectionString { get; set; }
        public int BookingSessionDurationInMinutes { get; set; }
    }
}
