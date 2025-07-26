using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomAmenityPersistence;

namespace TravelEase.Tests.Infrastructure.IntegrationTests.Repositories
{
    public class RoomAmenityRepositoryTests
    {
        private TravelEaseDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TravelEaseDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TravelEaseDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnFilteredPaginatedResults_WhenSearchQueryMatches()
        {
            using var context = CreateDbContext();

            var amenity1 = CreateValidAmenity("Free WiFi", "High speed internet");
            var amenity2 = CreateValidAmenity("TV", "Smart TV with apps");
            var amenity3 = CreateValidAmenity("Mini Bar", "Includes snacks and drinks");

            await context.RoomAmenities.AddRangeAsync(amenity1, amenity2, amenity3);
            await context.SaveChangesAsync();

            var repo = new RoomAmenityRepository(context);

            var result = await repo.GetAllAsync("WiFi", 1, 10);

            result.Items.Should().ContainSingle()
                .And.Contain(a => a.Name == "Free WiFi");
        }

        [Fact]
        public async Task GetByIdsAsync_ShouldReturnMatchingAmenities_WhenIdsProvided()
        {
            using var context = CreateDbContext();

            var amenity1 = CreateValidAmenity("AC", "Air conditioning");
            var amenity2 = CreateValidAmenity("Heater", "Room heating system");
            var amenity3 = CreateValidAmenity("Workspace", "Desk and chair");

            await context.RoomAmenities.AddRangeAsync(amenity1, amenity2, amenity3);
            await context.SaveChangesAsync();

            var repo = new RoomAmenityRepository(context);

            var result = await repo.GetByIdsAsync(new List<Guid> { amenity1.Id, amenity3.Id });

            result.Should().HaveCount(2)
                .And.Contain(r => r.Id == amenity1.Id)
                .And.Contain(r => r.Id == amenity3.Id);
        }

        #region Helper

        private RoomAmenity CreateValidAmenity(string name, string description)
        {
            return new RoomAmenity
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description
            };
        }

        #endregion
    }
}