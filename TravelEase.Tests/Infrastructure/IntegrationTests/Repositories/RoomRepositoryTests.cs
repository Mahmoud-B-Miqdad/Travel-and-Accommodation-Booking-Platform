using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomPersistence;

namespace TravelEase.Tests.Infrastructure.IntegrationTests.Repositories
{
    public class RoomRepositoryTests
    {
        private TravelEaseDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TravelEaseDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TravelEaseDbContext(options);
        }

        [Fact]
        public async Task GetAllByHotelIdAsync_ShouldReturnFilteredRooms_WhenSearchQueryMatchesView()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);

            var room1 = CreateValidRoom(roomType.Id, view: "Sea View");
            var room2 = CreateValidRoom(roomType.Id, view: "Garden View");

            await context.AddRangeAsync(hotel, roomType, room1, room2);
            await context.SaveChangesAsync();

            var repo = new RoomRepository(context);

            var result = await repo.GetAllByHotelIdAsync(hotel.Id, "Sea", 1, 10);

            result.Items.Should().ContainSingle(r => r.Id == room1.Id);
        }

        [Fact]
        public async Task GetRoomWithTypeAndDiscountsAsync_ShouldReturnRoomWithIncludedNavigationProperties()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var room = CreateValidRoom(roomType.Id);

            await context.AddRangeAsync(hotel, roomType, room);
            await context.SaveChangesAsync();

            var repo = new RoomRepository(context);

            var result = await repo.GetRoomWithTypeAndDiscountsAsync(room.Id);

            result.Should().NotBeNull();
            result.RoomType.Should().NotBeNull();
            result.RoomType.HotelId.Should().Be(hotel.Id);
        }

        [Fact]
        public async Task GetHotelAvailableRoomsAsync_ShouldReturnAvailableRooms_WhenNoConflictsExist()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var room = CreateValidRoom(roomType.Id);

            await context.AddRangeAsync(hotel, roomType, room);
            await context.SaveChangesAsync();

            var repo = new RoomRepository(context);

            var result = await repo.GetHotelAvailableRoomsAsync
                (hotel.Id, DateTime.Today, DateTime.Today.AddDays(2));

            result.Should().ContainSingle(r => r.Id == room.Id);
        }

        [Fact]
        public async Task GetHotelAvailableRoomsAsync_ShouldExcludeRoom_WhenConflictingBookingExists()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var room = CreateValidRoom(roomType.Id);

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                RoomId = room.Id,
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(3),
                BookingDate = DateTime.Today,
                Price = 100
            };

            await context.AddRangeAsync(hotel, roomType, room, booking);
            await context.SaveChangesAsync();

            var repo = new RoomRepository(context);

            var result = await repo.GetHotelAvailableRoomsAsync
                (hotel.Id, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAvailableRoomsWithCapacity_ShouldReturnMatchingRooms_WhenNoConflicts()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var room = CreateValidRoom(roomType.Id, adults: 2, children: 1);

            await context.AddRangeAsync(hotel, roomType, room);
            await context.SaveChangesAsync();

            var repo = new RoomRepository(context);

            var result = repo.GetAvailableRoomsWithCapacity(2, 1, DateTime.Today, DateTime.Today.AddDays(1));

            result.Should().ContainSingle(r => r.Id == room.Id);
        }

        #region Helper Methods

        private Hotel CreateValidHotel()
        {
            return new Hotel
            {
                Id = Guid.NewGuid(),
                Name = "Test Hotel",
                Description = "Nice stay",
                StreetAddress = "Main Street",
                PhoneNumber = "123456789",
                OwnerName = "John"
            };
        }

        private RoomType CreateValidRoomType(Guid hotelId)
        {
            return new RoomType
            {
                Id = Guid.NewGuid(),
                HotelId = hotelId,
                PricePerNight = 120,
                Category = Domain.Enums.RoomCategory.Double
            };
        }

        private Room CreateValidRoom
            (Guid roomTypeId, string view = "Default View", int adults = 2, int children = 1)
        {
            return new Room
            {
                Id = Guid.NewGuid(),
                RoomTypeId = roomTypeId,
                View = view,
                AdultsCapacity = adults,
                ChildrenCapacity = children
            };
        }

        #endregion
    }
}