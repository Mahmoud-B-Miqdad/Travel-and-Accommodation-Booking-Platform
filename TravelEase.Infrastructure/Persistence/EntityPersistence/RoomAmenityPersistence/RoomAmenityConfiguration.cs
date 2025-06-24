using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelEase.Domain.Aggregates.RoomAmenities;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.RoomAmenityPersistence
{
    public class RoomAmenityConfiguration : IEntityTypeConfiguration<RoomAmenity>
    {
        public void Configure(EntityTypeBuilder<RoomAmenity> builder)
        {
            builder.HasKey(ra => ra.Id);

            builder.Property(ra => ra.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ra => ra.Description)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}