using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.EntityPersistence.ReviewPersistence;

namespace TravelEase.Tests.Infrastructure.IntegrationTests.Repositories
{
    public class ReviewRepositoryTests
    {
        private TravelEaseDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TravelEaseDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TravelEaseDbContext(options);
        }

        [Fact]
        public async Task GetAllByHotelIdAsync_ShouldReturnFilteredReviews_WhenSearchQueryMatches()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var room = CreateValidRoom(roomType.Id);
            var user = CreateValidUser();
            var booking = CreateValidBooking(user.Id, room.Id);
            var review = CreateValidReview(booking.Id, "Amazing service!");

            await context.AddRangeAsync(hotel, roomType, room, user, booking, review);
            await context.SaveChangesAsync();

            var repo = new ReviewRepository(context);

            var result = await repo.GetAllByHotelIdAsync(hotel.Id, "Amazing", 1, 10);

            result.Items.Should().ContainSingle()
                .And.Contain(r => r.Id == review.Id);
        }

        [Fact]
        public async Task IsExistsForBookingAsync_ShouldReturnTrue_WhenReviewExists()
        {
            using var context = CreateDbContext();

            var booking = CreateValidBooking(Guid.NewGuid(), Guid.NewGuid());
            var review = CreateValidReview(booking.Id, "Nice stay!");

            await context.AddRangeAsync(booking, review);
            await context.SaveChangesAsync();

            var repo = new ReviewRepository(context);

            var result = await repo.IsExistsForBookingAsync(booking.Id);

            result.Should().BeTrue();
        }

        #region Helper Methods

        private Hotel CreateValidHotel()
        {
            return new Hotel
            {
                Id = Guid.NewGuid(),
                Name = "Elite Hotel",
                OwnerName = "John Owner",
                Description = "Luxury hotel with ocean view.",
                PhoneNumber = "1234567890",
                StreetAddress = "456 Ocean Drive"
            };
        }

        private RoomType CreateValidRoomType(Guid hotelId)
        {
            return new RoomType
            {
                Id = Guid.NewGuid(),
                HotelId = hotelId,
                PricePerNight = 200,
                Category = Domain.Enums.RoomCategory.Double
            };
        }

        private Room CreateValidRoom(Guid roomTypeId)
        {
            return new Room
            {
                Id = Guid.NewGuid(),
                RoomTypeId = roomTypeId,
                View = "Garden View"
            };
        }

        private User CreateValidUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice@example.com",
                PasswordHash = "hashedpwd",
                PhoneNumber = "0987654321",
                Salt = "saltvalue"
            };
        }

        private Booking CreateValidBooking(Guid userId, Guid roomId)
        {
            return new Booking
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoomId = roomId,
                BookingDate = DateTime.Today,
                CheckInDate = DateTime.Today.AddDays(1),
                CheckOutDate = DateTime.Today.AddDays(3),
                Price = 400
            };
        }

        private Review CreateValidReview(Guid bookingId, string comment)
        {
            return new Review
            {
                Id = Guid.NewGuid(),
                BookingId = bookingId,
                Rating = 4,
                Comment = comment
            };
        }

        #endregion
    }
}