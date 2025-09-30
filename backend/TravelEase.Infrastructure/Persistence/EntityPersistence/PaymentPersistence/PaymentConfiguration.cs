using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Payments;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TravelEase.Domain.Enums;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.PaymentPersistence
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .IsRequired();

            builder.Property(p => p.BookingId)
                   .IsRequired();

            builder.Property(p => p.Method)
                   .IsRequired()
                   .HasConversion(new EnumToStringConverter<PaymentMethod>())
                   .HasMaxLength(50);

            builder.Property(p => p.Status)
                   .IsRequired()
                   .HasConversion(new EnumToStringConverter<PaymentStatus>())
                   .HasMaxLength(50);

            builder.Property(p => p.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(p => p.Booking)
                   .WithOne(b => b.Payment)
                   .HasForeignKey<Payment>(p => p.BookingId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}