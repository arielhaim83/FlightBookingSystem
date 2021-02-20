using Flight.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Flight.Infrastructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightsContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public FlightRepository(FlightsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        async public Task<Domain.Entities.Flight> GetAsync(int flightId)
        {
            return await _context.Flights
                .Include(f => f.Plane)
                .FirstOrDefaultAsync(f => f.Id == flightId);
        }

        public Domain.Entities.Flight Add(Domain.Entities.Flight flight)
        {
            return _context.Flights.Add(flight).Entity;
        }
    }
}
