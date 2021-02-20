using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Domain.Entities
{
    public class Plane
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int NumberOfSeats { get; private set; }

        private readonly List<Flight> _flights = new List<Flight>();
        public IReadOnlyCollection<Flight> Flights => _flights;

        protected Plane()
        {

        }

        public Plane(string name, int numberOfSeats)
        {
            Name = name;
            NumberOfSeats = numberOfSeats;
        }
    }
}
