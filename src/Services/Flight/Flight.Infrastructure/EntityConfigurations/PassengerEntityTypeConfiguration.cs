using Flight.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Infrastructure.EntityConfigurations
{
    class PassengerEntityTypeConfiguration : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.ToTable("Passengers");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("id")
                .UseIdentityColumn();

            builder.Property(p => p.Name)
                .HasColumnName("name");

            builder.HasOne(p => p.Flight)
                .WithMany(f => f.Passengers)
                .HasForeignKey(p => p.FlightId);
        }
    }
}
