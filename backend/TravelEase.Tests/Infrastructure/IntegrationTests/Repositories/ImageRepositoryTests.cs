using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelEase.Domain.Aggregates.Images;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.EntityPersistence.ImagePersistence;

namespace TravelEase.Tests.Infrastructure.IntegrationTests.Repositories
{
    public class ImageRepositoryTests
    {
        private TravelEaseDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TravelEaseDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TravelEaseDbContext(options);
        }

        [Fact]
        public async Task GetAllImageUrlsByEntityIdAsync_ShouldReturnPagedUrls()
        {
            using var context = CreateDbContext();

            var entityId = Guid.NewGuid();

            var images = new[]
            {
                new Image { Id = Guid.NewGuid(), EntityId = entityId, Url = "http://image1.jpg" },
                new Image { Id = Guid.NewGuid(), EntityId = entityId, Url = "http://image2.jpg" },
                new Image { Id = Guid.NewGuid(), EntityId = Guid.NewGuid(), Url = "http://image3.jpg" }
            };

            await context.Images.AddRangeAsync(images);
            await context.SaveChangesAsync();

            var repo = new ImageRepository(context);

            var result = await repo.GetAllImageUrlsByEntityIdAsync(entityId, pageNumber: 1, pageSize: 2);

            result.Items.Should().HaveCount(2);
            result.Items.Should().Contain(new[] { "http://image1.jpg", "http://image2.jpg" });
            result.PageData.TotalItems.Should().Be(2);
        }

        [Fact]
        public async Task GetSingleOrDefaultAsync_ShouldReturnImage_WhenPredicateMatches()
        {
            using var context = CreateDbContext();

            var image = new Image
            {
                Id = Guid.NewGuid(),
                EntityId = Guid.NewGuid(),
                Url = "http://uniqueimage.jpg"
            };

            await context.Images.AddAsync(image);
            await context.SaveChangesAsync();

            var repo = new ImageRepository(context);

            Expression<Func<Image, bool>> predicate = img => img.Url == "http://uniqueimage.jpg";

            var result = await repo.GetSingleOrDefaultAsync(predicate);

            result.Should().NotBeNull();
            result!.Id.Should().Be(image.Id);
            result.Url.Should().Be(image.Url);
        }

        [Fact]
        public async Task GetSingleOrDefaultAsync_ShouldReturnNull_WhenNoMatch()
        {
            using var context = CreateDbContext();

            var repo = new ImageRepository(context);

            Expression<Func<Image, bool>> predicate = img => img.Url == "nonexistent.jpg";

            var result = await repo.GetSingleOrDefaultAsync(predicate);

            result.Should().BeNull();
        }
    }
}