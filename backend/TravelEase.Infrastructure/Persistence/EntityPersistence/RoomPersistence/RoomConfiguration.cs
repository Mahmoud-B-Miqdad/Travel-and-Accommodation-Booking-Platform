using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelEase.Domain.Aggregates.Rooms;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.RoomPersistence
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {

            builder.HasKey(r => r.Id);

            builder.Property(r => r.AdultsCapacity)
                .HasDefaultValue(2)
                .IsRequired();

            builder.Property(r => r.ChildrenCapacity)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(r => r.View)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Rating)
                .IsRequired();

            builder.HasOne(r => r.RoomType)
                .WithMany() 
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(room =>
                room.HasCheckConstraint
                ("CK_Room_RatingRange", "[Rating] >= 0 AND [Rating] <= 5"));
        }
    }
}