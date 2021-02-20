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
    class PlaneEntityTypeConfiguration : IEntityTypeConfiguration<Plane>
    {
        public void Configure(EntityTypeBuilder<Plane> builder)
        {
            builder.ToTable("Planes");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("id")
                .UseIdentityColumn();

            builder.Property(p => p.Name)
                .HasColumnName("name");            

            builder.Property(p => p.NumberOfSeats)
                .HasColumnName("number_of_seats");
                
        }
    }
}
