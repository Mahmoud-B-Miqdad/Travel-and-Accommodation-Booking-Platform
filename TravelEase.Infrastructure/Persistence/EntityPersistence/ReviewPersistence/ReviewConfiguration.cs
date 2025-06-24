using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelEase.Domain.Aggregates.Reviews;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.ReviewPersistence
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Comment)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(r => r.ReviewDate)
                .IsRequired();

            builder.Property(r => r.Rating)
                .IsRequired();

            builder.HasOne(r => r.Booking)
                .WithOne(b => b.Review)
                .HasForeignKey<Review>(r => r.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(review =>
                review.HasCheckConstraint
                ("CK_Review_RatingRange", "[Rating] >= 0 AND [Rating] <= 5"));
        }
    }
}