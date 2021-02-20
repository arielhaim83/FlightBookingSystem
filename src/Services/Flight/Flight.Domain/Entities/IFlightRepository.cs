using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Domain.Entities
{
    public interface IFlightRepository : IRepository<Flight>
    {
        Task<Flight> GetAsync(int flightId);
        Flight Add(Flight flight);
    }
}
