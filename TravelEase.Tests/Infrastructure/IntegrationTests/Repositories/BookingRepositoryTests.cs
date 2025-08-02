using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.EntityPersistence.BookingPersistence;

namespace TravelEase.Tests.Infrastructure.IntegrationTests.Repositories
{
    public class BookingRepositoryTests
    {
        private TravelEaseDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TravelEaseDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TravelEaseDbContext(options);
        }

        [Fact]
        public async Task GetAllByHotelIdAsync_ShouldReturnBookings_WhenHotelHasBookings()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var room = CreateValidRoom(roomType.Id);
            var user = CreateValidUser();

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                RoomId = room.Id,
                UserId = user.Id,
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(2),
                BookingDate = DateTime.Today,
                Price = 200
            };

            await context.AddRangeAsync(hotel, roomType, room, user, booking);
            await context.SaveChangesAsync();

            var repo = new BookingRepository(context);

            var result = await repo.GetAllByHotelIdAsync(hotel.Id, 1, 10);

            result.Items.Should().ContainSingle()
                .And.Contain(b => b.Id == booking.Id);
        }

        [Fact]
        public async Task IsBookingAccessibleToUserAsync_ShouldReturnTrue_WhenBookingBelongsToUser()
        {
            using var context = CreateDbContext();

            var user = CreateValidUser();
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                RoomId = Guid.NewGuid(),
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(2),
                BookingDate = DateTime.Today,
                Price = 150
            };

            await context.Users.AddAsync(user);
            await context.Bookings.AddAsync(booking);
            await context.SaveChangesAsync();

            var repo = new BookingRepository(context);

            var result = await repo.IsBookingAccessibleToUserAsync(booking.Id, user.Email);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsConflictingBookingAsync_ShouldReturnTrue_WhenConflictExists()
        {
            using var context = CreateDbContext();

            var roomId = Guid.NewGuid();

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                CheckInDate = new DateTime(2025, 7, 30),
                CheckOutDate = new DateTime(2025, 8, 3),
                BookingDate = new DateTime(2025, 7, 20),
                Price = 300
            };

            await context.Bookings.AddAsync(booking);
            await context.SaveChangesAsync();

            var repo = new BookingRepository(context);

            var result = await repo.ExistsConflictingBookingAsync(
                roomId,
                new DateTime(2025, 8, 1),
                new DateTime(2025, 8, 5)
            );

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetInvoiceByBookingIdAsync_ShouldReturnInvoice_WhenBookingExists()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var room = CreateValidRoom(roomType.Id);

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                RoomId = room.Id,
                Price = 500,
                BookingDate = DateTime.Today,
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(2)
            };

            await context.AddRangeAsync(hotel, roomType, room, booking);
            await context.SaveChangesAsync();

            var repo = new BookingRepository(context);

            var invoice = await repo.GetInvoiceByBookingIdAsync(booking.Id);

            invoice.Should().NotBeNull();
            invoice.BookingId.Should().Be(booking.Id);
            invoice.Price.Should().Be(500);
            invoice.HotelName.Should().Be(hotel.Name);
            invoice.OwnerName.Should().Be(hotel.OwnerName);
        }

        #region Helper Methods to create valid entities

        private User CreateValidUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = "guest@test.com",
                PasswordHash = "hashedPassword",
                PhoneNumber = "1234567890",
                Salt = "randomSalt"
            };
        }

        private Hotel CreateValidHotel()
        {
            return new Hotel
            {
                Id = Guid.NewGuid(),
                Name = "Grand Hotel",
                OwnerName = "Mr. X",
                Description = "Some description",
                PhoneNumber = "0987654321",
                StreetAddress = "123 Street"
            };
        }

        private RoomType CreateValidRoomType(Guid hotelId)
        {
            return new RoomType
            {
                Id = Guid.NewGuid(),
                HotelId = hotelId,
                PricePerNight = 100,
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
        #endregion
    }
}