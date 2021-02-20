using Flight.Domain.Entities;
using Flight.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Flight.Infrastructure
{
    public class FlightsContext : DbContext, IUnitOfWork
    {
        public FlightsContext(DbContextOptions<FlightsContext> options) : base(options) { }

        public DbSet<Domain.Entities.Flight> Flights { get; set; }
        public DbSet<Plane> Planes { get; set; }
        public DbSet<Passenger> Passengers { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FlightEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlaneEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PassengerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BaggageEntityTypeConfiguration());            
        }
    }
}
