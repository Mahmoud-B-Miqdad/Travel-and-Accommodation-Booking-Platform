using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Payments;
using TravelEase.Domain.Aggregates.Reviews;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.BookingPersistence
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {

            builder.HasKey(b => b.Id);

            builder.Property(b => b.CheckInDate)
                .IsRequired();

            builder.Property(b => b.CheckOutDate)
                .IsRequired();

            builder.Property(b => b.BookingDate)
                .IsRequired();

            builder.Property(b => b.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");


            builder.HasOne(b => b.User)
                .WithMany(u => u.Bookings)  
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Room)
                .WithMany()  
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Review)
                .WithOne(r => r.Booking)
                .HasForeignKey<Review>(r => r.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Payment)
                .WithOne(p => p.Booking)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}