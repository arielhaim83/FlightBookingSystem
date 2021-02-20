using Flight.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Infrastructure.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly FlightsContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public PassengerRepository(FlightsContext context)
        {
            _context = context;
        }
        public async Task<Passenger> GetAsync(int passengerId)
        {
            return await _context.Passengers.FirstOrDefaultAsync(p => p.Id == passengerId);
        }

        public Passenger Add(Passenger passenger)
        {
            return _context.Passengers.Add(passenger).Entity;
        }
    }
}
