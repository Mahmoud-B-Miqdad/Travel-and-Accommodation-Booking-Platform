using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelEase.Domain.Aggregates.Hotels;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.HotelPersistence
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(h => h.Rating)
                .IsRequired();

            builder.Property(h => h.StreetAddress)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(h => h.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(h => h.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(h => h.FloorsNumber)
                .IsRequired();

            builder.Property(h => h.OwnerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(h => h.City)
                   .WithMany(c => c.Hotels)
                   .HasForeignKey(h => h.CityId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(h =>
                    h.HasCheckConstraint
                    ("CK_Hotel_RatingRange", "[Rating] >= 0 AND [Rating] <= 5"));
        }
    }
}