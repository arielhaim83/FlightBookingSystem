using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Flight.Domain.Entities;

namespace Flight.Infrastructure.EntityConfigurations
{
    class FlightEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Entities.Flight>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Flight> builder)
        {
            builder.ToTable("Flights");
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .HasColumnName("id")
                .UseIdentityColumn();

            builder.Property(f => f.FlightDate)
                .HasColumnName("flight_date");

            builder.Property(f => f.AvailableSeats)
                .HasColumnName("available_seats");

            builder.Property(f => f.PlaneId)
                .HasColumnName("plane_id");

            builder.Property(f => f.BaggagesLimitPerPassenger)
                .HasColumnName("baggages_limit_per_passenger");

            builder.Property(f => f.BaggagesWeightLimitPerPassenger)
                .HasColumnName("baggages_weight_limit_per_passenger");

            builder.HasOne(f => f.Plane)
                .WithMany(p => p.Flights)
                .HasForeignKey(f => f.PlaneId);           
        }
    }
}
