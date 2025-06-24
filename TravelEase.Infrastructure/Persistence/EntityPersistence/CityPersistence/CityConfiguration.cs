using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelEase.Domain.Aggregates.Cities;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.CityPersistence
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.CountryName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.CountryCode)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(c => c.PostOffice)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasMany(c => c.Hotels)
                   .WithOne(h => h.City)
                   .HasForeignKey(h => h.CityId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}