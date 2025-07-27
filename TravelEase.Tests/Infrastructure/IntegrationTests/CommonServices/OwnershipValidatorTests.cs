using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.Services.CommonServices;

namespace TravelEase.Tests.Infrastructure.IntegrationTests.CommonServices
{
    public class OwnershipValidatorTests
    {
        private TravelEaseDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TravelEaseDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TravelEaseDbContext(options);
        }

        [Fact]
        public async Task IsRoomBelongsToHotelAsync_ShouldReturnTrue_WhenRoomBelongsToHotel()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var room = CreateValidRoom(roomType.Id);

            await context.AddRangeAsync(hotel, roomType, room);
            await context.SaveChangesAsync();

            var validator = new OwnershipValidator(context);

            var result = await validator.IsRoomBelongsToHotelAsync(room.Id, hotel.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsBookingBelongsToHotelAsync_ShouldReturnTrue_WhenBookingBelongsToHotel()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var room = CreateValidRoom(roomType.Id);
            var booking = CreateValidBooking(room.Id);

            await context.AddRangeAsync(hotel, roomType, room, booking);
            await context.SaveChangesAsync();

            var validator = new OwnershipValidator(context);

            var result = await validator.IsBookingBelongsToHotelAsync(booking.Id, hotel.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsReviewBelongsToHotelAsync_ShouldReturnTrue_WhenReviewBelongsToHotel()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var room = CreateValidRoom(roomType.Id);
            var booking = CreateValidBooking(room.Id);
            var review = CreateValidReview(booking.Id);

            await context.AddRangeAsync(hotel, roomType, room, booking, review);
            await context.SaveChangesAsync();

            var validator = new OwnershipValidator(context);

            var result = await validator.IsReviewBelongsToHotelAsync(review.Id, hotel.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsRoomTypeBelongsToHotelAsync_ShouldReturnTrue_WhenRoomTypeBelongsToHotel()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);

            await context.AddRangeAsync(hotel, roomType);
            await context.SaveChangesAsync();

            var validator = new OwnershipValidator(context);

            var result = await validator.IsRoomTypeBelongsToHotelAsync(roomType.Id, hotel.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsDiscountBelongsToRoomTypeAsync_ShouldReturnTrue_WhenDiscountBelongsToRoomType()
        {
            using var context = CreateDbContext();

            var hotel = CreateValidHotel();
            var roomType = CreateValidRoomType(hotel.Id);
            var discount = CreateValidDiscount(roomType.Id);

            await context.AddRangeAsync(hotel, roomType, discount);
            await context.SaveChangesAsync();

            var validator = new OwnershipValidator(context);

            var result = await validator.IsDiscountBelongsToRoomTypeAsync(discount.Id, roomType.Id);

            result.Should().BeTrue();
        }

        #region Helpers

        private Hotel CreateValidHotel() =>
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Hotel 1",
                OwnerName = "Owner",
                PhoneNumber = "0000",
                Description = "Test",
                StreetAddress = "Street"
            };

        private RoomType CreateValidRoomType(Guid hotelId) =>
            new()
            {
                Id = Guid.NewGuid(),
                HotelId = hotelId,
                PricePerNight = 100,
                Category = TravelEase.Domain.Enums.RoomCategory.Single
            };

        private Room CreateValidRoom(Guid bookingId) =>
            new()
            {
                Id = Guid.NewGuid(),
                RoomTypeId = bookingId,
                View = "Sea View"
            };

        private Booking CreateValidBooking(Guid roomId) =>
            new()
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                BookingDate = DateTime.Today,
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(1),
                Price = 300
            };

        private Review CreateValidReview(Guid bookingId) =>
            new()
            {
                Id = Guid.NewGuid(),
                BookingId = bookingId,
                Rating = 5,
                Comment = "Excellent"
            };

        private Discount CreateValidDiscount(Guid roomTypeId) =>
            new()
            {
                Id = Guid.NewGuid(),
                RoomTypeId = roomTypeId,
                DiscountPercentage = 0.15f,
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(10)
            };

        #endregion
    }
}