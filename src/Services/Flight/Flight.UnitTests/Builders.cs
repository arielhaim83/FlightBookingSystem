using Flight.Domain.Entities;
using System;

namespace Flight.UnitTests
{
    public class PlaneBuilder
    {
        public Plane Build()
        {
            return new Plane("Fake Plane", 100);
        }
    }
    public class FlightBuilder
    {
        private readonly Domain.Entities.Flight flight;
        public FlightBuilder(Plane plane)
        {
            flight = new Domain.Entities.Flight(
                plane, 
                plane.NumberOfSeats, 
                2, 
                100, 
                DateTime.UtcNow);
        }
        
        public Domain.Entities.Flight Build()
        {
            return flight;
        }
    }

    public class PassengerBuilder
    {
        private readonly Passenger passenger;

        public PassengerBuilder(string name, Domain.Entities.Flight flight)
        {
            passenger = new Passenger(name, flight);
        }

        public PassengerBuilder AddOne(string label, int weight)
        {
            passenger.AddBaggage(label, weight);
            return this;
        }

        public Passenger Build()
        {
            return passenger;
        }
    }
}
