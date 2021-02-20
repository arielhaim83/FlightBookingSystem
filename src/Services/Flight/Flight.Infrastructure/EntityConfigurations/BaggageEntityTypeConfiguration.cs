using Flight.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flight.Infrastructure.EntityConfigurations
{
    class BaggageEntityTypeConfiguration : IEntityTypeConfiguration<Baggage>
    {
        public void Configure(EntityTypeBuilder<Baggage> builder)
        {
            builder.ToTable("Baggages");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .HasColumnName("id")
                .UseIdentityColumn();

            builder.Property(b => b.PassengerId)
                .HasColumnName("passenger_id");    

            builder.Property(p => p.Label)
                .HasColumnName("label");

            builder.Property(p => p.Weight)
                .HasColumnName("weight");

            builder.HasOne(b => b.Passenger)
                .WithMany(p => p.Baggages)
                .HasForeignKey(b => b.PassengerId);
                
        }
    }
}
