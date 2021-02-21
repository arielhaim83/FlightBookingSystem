using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Domain.Entities
{
    public class Baggage
    {
        public int Id { get; private set; }        
        public int PassengerId { get; private set; }        
        public string Label { get; private set; }
        public int Weight { get; private set; }

        public Passenger Passenger { get; private set; }

        protected Baggage()
        {

        }

        public Baggage(Passenger passenger, string label, int weight)
        {
            Passenger = passenger;
            Label = label;
            Weight = weight;
        }
    }
}
