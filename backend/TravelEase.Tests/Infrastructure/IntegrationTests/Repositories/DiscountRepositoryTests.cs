using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Enums;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.EntityPersistence.DiscountPersistence;

namespace TravelEase.Tests.Infrastructure.IntegrationTests.Repositories
{
    public class DiscountRepositoryTests
    {
        private TravelEaseDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TravelEaseDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TravelEaseDbContext(options);
        }

        [Fact]
        public async Task GetAllByRoomTypeIdAsync_ShouldReturnPaginatedDiscounts_WhenExist()
        {
            using var context = CreateDbContext();

            var roomType = CreateValidRoomType();
            var discount = CreateValidDiscount(roomType.Id);

            await context.AddRangeAsync(roomType, discount);
            await context.SaveChangesAsync();

            var repo = new DiscountRepository(context);

            var result = await repo.GetAllByRoomTypeIdAsync(roomType.Id, pageNumber: 1, pageSize: 10);

            result.Items.Should().ContainSingle()
                .And.Contain(d => d.Id == discount.Id);
        }

        [Fact]
        public async Task ExistsConflictingDiscountAsync_ShouldReturnTrue_WhenConflictExists()
        {
            using var context = CreateDbContext();

            var roomType = CreateValidRoomType();

            var existingDiscount = new Discount
            {
                Id = Guid.NewGuid(),
                RoomTypeId = roomType.Id,
                FromDate = new DateTime(2025, 7, 20),
                ToDate = new DateTime(2025, 7, 30),
                DiscountPercentage = 10
            };

            await context.AddRangeAsync(roomType, existingDiscount);
            await context.SaveChangesAsync();

            var repo = new DiscountRepository(context);

            var result = await repo.ExistsConflictingDiscountAsync(
                roomType.Id,
                fromDate: new DateTime(2025, 7, 25),
                toDate: new DateTime(2025, 8, 1)
            );

            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsConflictingDiscountAsync_ShouldReturnFalse_WhenNoConflictExists()
        {
            using var context = CreateDbContext();

            var roomType = CreateValidRoomType();

            var existingDiscount = new Discount
            {
                Id = Guid.NewGuid(),
                RoomTypeId = roomType.Id,
                FromDate = new DateTime(2025, 7, 1),
                ToDate = new DateTime(2025, 7, 5),
                DiscountPercentage = 15
            };

            await context.AddRangeAsync(roomType, existingDiscount);
            await context.SaveChangesAsync();

            var repo = new DiscountRepository(context);

            var result = await repo.ExistsConflictingDiscountAsync(
                roomType.Id,
                fromDate: new DateTime(2025, 7, 10),
                toDate: new DateTime(2025, 7, 15)
            );

            result.Should().BeFalse();
        }

        #region Helper Methods

        private RoomType CreateValidRoomType()
        {
            return new RoomType
            {
                Id = Guid.NewGuid(),
                HotelId = Guid.NewGuid(),
                PricePerNight = 100,
                Category = RoomCategory.Single
            };
        }

        private Discount CreateValidDiscount(Guid roomTypeId)
        {
            return new Discount
            {
                Id = Guid.NewGuid(),
                RoomTypeId = roomTypeId,
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(5),
                DiscountPercentage = 20
            };
        }

        #endregion
    }
}