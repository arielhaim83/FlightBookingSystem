using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Domain.Entities
{
    public class Baggage
    {
        public int Id { get; set; }        
        public int PassengerId { get; set; }        
        public string Label { get; set; }
        public int Weight { get; set; }

        public Passenger Passenger { get; set; }
    }
}
