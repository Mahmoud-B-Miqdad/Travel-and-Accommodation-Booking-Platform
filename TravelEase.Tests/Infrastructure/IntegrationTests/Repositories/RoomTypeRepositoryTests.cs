using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Enums;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomTypePersistence;

namespace TravelEase.Tests.Infrastructure.IntegrationTests.Repositories
{
    public class RoomTypeRepositoryTests
    {
        private TravelEaseDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TravelEaseDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TravelEaseDbContext(options);
        }

        [Fact]
        public async Task GetAllByHotelIdAsync_ShouldReturnRoomTypesWithAmenities_WhenIncludeAmenitiesIsTrue()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var amenity = CreateValidAmenity();

            roomType.Amenities.Add(amenity);

            await context.AddRangeAsync(hotel, roomType, amenity);
            await context.SaveChangesAsync();

            var repo = new RoomTypeRepository(context);

            var result = await repo.GetAllByHotelIdAsync(hotel.Id, includeAmenities: true, 1, 10);

            result.Items.Should().ContainSingle(rt => rt.Id == roomType.Id);
            result.Items.First().Amenities.Should().ContainSingle(a => a.Id == amenity.Id);
        }

        [Fact]
        public async Task 
            GetAllByHotelIdAsync_ShouldReturnRoomTypesWithoutAmenities_WhenIncludeAmenitiesIsFalse()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var amenity = CreateValidAmenity();

            roomType.Amenities.Add(amenity);

            await context.AddRangeAsync(hotel, roomType, amenity);
            await context.SaveChangesAsync();

            var repo = new RoomTypeRepository(context);

            var result = await repo.GetAllByHotelIdAsync(hotel.Id, includeAmenities: false, 1, 10);

            result.Items.Should().ContainSingle(rt => rt.Id == roomType.Id);
            result.Items.First().Amenities.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task ExistsByHotelAndCategoryAsync_ShouldReturnTrue_WhenRoomTypeExists()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);

            await context.AddRangeAsync(hotel, roomType);
            await context.SaveChangesAsync();

            var repo = new RoomTypeRepository(context);

            var exists = await repo.ExistsByHotelAndCategoryAsync(hotel.Id, roomType.Category);

            exists.Should().BeTrue();
        }

        [Fact]
        public async Task HasRoomsAsync_ShouldReturnTrue_WhenRoomTypeHasRooms()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var room = CreateValidRoom(roomType.Id);

            await context.AddRangeAsync(hotel, roomType, room);
            await context.SaveChangesAsync();

            var repo = new RoomTypeRepository(context);

            var hasRooms = await repo.HasRoomsAsync(roomType.Id);

            hasRooms.Should().BeTrue();
        }

        #region Helper Methods

        private Hotel CreateValidHotel()
        {
            return new Hotel
            {
                Id = Guid.NewGuid(),
                Name = "Hotel Test",
                OwnerName = "Owner",
                Description = "Nice",
                StreetAddress = "123 Street",
                PhoneNumber = "123456789"
            };
        }

        private RoomType CreateValidRoomType(Guid hotelId)
        {
            return new RoomType
            {
                Id = Guid.NewGuid(),
                HotelId = hotelId,
                PricePerNight = 150,
                Category = RoomCategory.Double,
                Amenities = new List<RoomAmenity>()
            };
        }

        private RoomAmenity CreateValidAmenity()
        {
            return new RoomAmenity
            {
                Id = Guid.NewGuid(),
                Name = "Wi-Fi",
                Description = "Fast internet"
            };
        }

        private Room CreateValidRoom(Guid roomTypeId)
        {
            return new Room
            {
                Id = Guid.NewGuid(),
                RoomTypeId = roomTypeId,
                View = "City",
                AdultsCapacity = 2,
                ChildrenCapacity = 1
            };
        }

        #endregion
    }
}