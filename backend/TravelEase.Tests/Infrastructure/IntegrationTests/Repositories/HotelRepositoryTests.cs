using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.EntityPersistence.HotelPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.UserPersistence;

namespace TravelEase.Tests.Infrastructure.IntegrationTests.Repositories
{
    public class HotelRepositoryTests
    {
        private TravelEaseDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TravelEaseDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TravelEaseDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnFilteredPaginatedHotels_WhenSearchQueryMatches()
        {
            using var context = CreateDbContext();

            var hotel1 = CreateValidHotel("Sea View Hotel");
            var hotel2 = CreateValidHotel("Mountain Lodge");

            await context.Hotels.AddRangeAsync(hotel1, hotel2);
            await context.SaveChangesAsync();

            var repo = new HotelRepository(context, new RoomRepository(context), new UserRepository(context));

            var result = await repo.GetAllAsync("Sea", 1, 10);

            result.Items.Should().ContainSingle(h => h.Name == "Sea View Hotel");
        }

        [Fact]
        public async Task GetRecentlyVisitedHotelsForGuestAsync_ShouldReturnOrderedHotels()
        {
            using var context = CreateDbContext();

            var user = CreateValidUser();
            var hotel = CreateValidHotel("Visited Hotel");
            var roomType = CreateValidRoomType(hotel.Id);
            var room = CreateValidRoom(roomType.Id);

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                RoomId = room.Id,
                UserId = user.Id,
                CheckInDate = DateTime.Today.AddDays(-2),
                CheckOutDate = DateTime.Today.AddDays(1),
                BookingDate = DateTime.Today.AddDays(-5),
                Price = 100
            };

            await context.Users.AddAsync(user);
            await context.AddRangeAsync(hotel, roomType, room, booking);
            await context.SaveChangesAsync();

            var repo = new HotelRepository(context, new RoomRepository(context), new UserRepository(context));

            var result = await repo.GetRecentlyVisitedHotelsForGuestAsync(user.Id, 5);

            result.Should().ContainSingle();
            result[0].Id.Should().Be(hotel.Id);
        }

        #region Helper Methods

        private Hotel CreateValidHotel(string name)
        {
            return new Hotel
            {
                Id = Guid.NewGuid(),
                Name = name,
                OwnerName = "Owner",
                Description = "Hotel Description",
                PhoneNumber = "1234567890",
                StreetAddress = "123 Beach Road",
                CityId = Guid.NewGuid(),
                FloorsNumber = 5,
                Rating = 4.5f
            };
        }

        private RoomType CreateValidRoomType(Guid hotelId)
        {
            return new RoomType
            {
                Id = Guid.NewGuid(),
                HotelId = hotelId,
                PricePerNight = 150,
                Category = Domain.Enums.RoomCategory.Single
            };
        }

        private Room CreateValidRoom(Guid roomTypeId)
        {
            return new Room
            {
                Id = Guid.NewGuid(),
                RoomTypeId = roomTypeId,
                View = "Sea View"
            };
        }

        private User CreateValidUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PasswordHash = "hashed_pw",
                Salt = "salt",
                PhoneNumber = "9876543210"
            };
        }

        #endregion
    }
}