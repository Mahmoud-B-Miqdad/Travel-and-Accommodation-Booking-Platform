using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelEase.Domain.Aggregates.Discounts;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.DiscountPersistence
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            
            builder.HasKey(d => d.Id);

            builder.Property(d => d.DiscountPercentage)
                .IsRequired();

            builder.Property(d => d.FromDate)
                .IsRequired();

            builder.Property(d => d.ToDate)
                .IsRequired();

            builder.HasOne(d => d.RoomType)
                   .WithMany(rt => rt.Discounts)
                   .HasForeignKey(d => d.RoomTypeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(d =>
                d.HasCheckConstraint
                ("CK_Discount_PercentageRange", "[DiscountPercentage] >= 0 AND [DiscountPercentage] <= 100"));
        }
    }
}