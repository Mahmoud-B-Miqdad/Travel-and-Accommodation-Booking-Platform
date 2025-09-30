using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Aggregates.Images;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.ImagePersistence
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .IsRequired();

            builder.Property(i => i.EntityId)
                .IsRequired();

            builder.Property(i => i.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.Type)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<ImageType>())
                .HasMaxLength(50);

            builder.Property(i => i.Format)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<ImageFormat>())
                .HasMaxLength(50);

            builder.HasIndex(i => i.EntityId);
            builder.HasIndex(i => i.Type);
        }
    }
}