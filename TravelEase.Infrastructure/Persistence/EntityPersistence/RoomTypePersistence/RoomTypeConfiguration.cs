using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Enums;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.RoomTypePersistence
{
    public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.PricePerNight)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(rt => rt.Category)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<RoomCategory>());

            builder.HasOne(rt => rt.Hotel)
                .WithMany() 
                .HasForeignKey(rt => rt.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(rt => rt.Discounts)
                .WithOne(d => d.RoomType)
                .HasForeignKey(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(rt => rt.Amenities)
                .WithMany(ra => ra.RoomTypes)
                .UsingEntity(j => j.ToTable("RoomTypeAmenities"));

            builder.ToTable(roomType =>
                roomType
                .HasCheckConstraint
                ("CK_RoomType_PriceRange", "[PricePerNight] >= 0"));
        }
    }
}