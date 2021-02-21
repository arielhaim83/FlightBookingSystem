using Flight.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Domain.Entities
{
    public class Passenger
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int FlightId { get; private set; }
        public Flight Flight { get; private set; }

        private readonly List<Baggage> _baggages = new List<Baggage>();
        public IReadOnlyCollection<Baggage> Baggages => _baggages;

        // Used by EF
        protected Passenger()
        {
        }

        public Passenger(string name, Flight flight)
        {
            Name = name;
            Flight = flight;
        }
        
        public void AddBaggage(string label, int weight)
        {            
            if (_baggages.Count == Flight.BaggagesLimitPerPassenger)
            {
                BaggagesLimitExceededException();
            }

            var currentWeight = _baggages.Sum(b => b.Weight);
            if ((currentWeight + weight) > Flight.BaggagesWeightLimitPerPassenger)
            {
                BaggagesWeightExceededException();
            }

            _baggages.Add(new Baggage
            (
                this,
                label,
                weight
            ));
        }

        private void BaggagesLimitExceededException()
        {
            throw new FlightDomainException($"Is not possible to add more than {Flight.BaggagesLimitPerPassenger} baggages");
        }

        private void BaggagesWeightExceededException()
        {
            throw new FlightDomainException($"Is not possible to add more than {Flight.BaggagesWeightLimitPerPassenger}kg of baggages");
        }
    }
}
