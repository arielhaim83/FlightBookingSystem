using Flight.Domain.Exceptions;
using System;
using Xunit;

namespace Flight.UnitTests
{
    public class PassengerTest
    {
        [Fact]
        public void invalid_number_of_baggages_added()
        {
            var plane = new PlaneBuilder().Build();
            var flight = new Domain.Entities.Flight(plane, 
                100, 
                1, // Number of allowed baggages
                100, 
                DateTime.UtcNow);

            Assert.Throws<FlightDomainException>(() => {
                var passenger = new PassengerBuilder("Fake passenger", flight)
                .AddOne("first", 10)
                .AddOne("second", 10);                
            });
        }

        [Fact]
        public void invalid_weight_of_baggages_added()
        {
            var plane = new PlaneBuilder().Build();
            var flight = new Domain.Entities.Flight(plane,
                100,
                2, // Number of allowed baggages
                100, // Total weight of baggages
                DateTime.UtcNow);

            Assert.Throws<FlightDomainException>(() => {
                var passenger = new PassengerBuilder("Fake passenger", flight)
                .AddOne("first", 50)
                .AddOne("second", 60);
            });
        }
    }
}
