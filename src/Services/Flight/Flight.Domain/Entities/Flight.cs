using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Domain.Entities
{
    public class Flight
    {
        public int Id { get; private set; }
        public int PlaneId { get; private set; }
        public int AvailableSeats { get; private set; }
        public int BaggagesLimitPerPassenger { get; private set; }
        public int BaggagesWeightLimitPerPassenger { get; private set; }
        public DateTime FlightDate { get; private set; }
        public Plane Plane { get; private set; }
        private readonly List<Passenger> _passengers = new List<Passenger>();

        public IReadOnlyCollection<Passenger> Passengers => _passengers;

        protected Flight()
        {
        }

        public Flight(Plane plane, int availableSeats, int baggagesLimitPerPassenger, int baggagesWeightLimitPerPassenger,
            DateTime flightDate)
        {
            Plane = plane;
            AvailableSeats = availableSeats;
            BaggagesLimitPerPassenger = baggagesLimitPerPassenger;
            BaggagesWeightLimitPerPassenger = baggagesWeightLimitPerPassenger;
            FlightDate = flightDate;
        }
    }
}
