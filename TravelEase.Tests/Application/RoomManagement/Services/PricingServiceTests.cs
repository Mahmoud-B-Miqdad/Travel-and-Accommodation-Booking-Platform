using FluentAssertions;
using Moq;
using TravelEase.Application.RoomManagement.Services;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.RoomManagement.Services
{
    public class PricingServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly PricingService _pricingService;

        public PricingServiceTests()
        {
            _pricingService = new PricingService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task CalculateTotalPriceAsync_ShouldThrowNotFoundException_WhenRoomNotFound()
        {
            _unitOfWorkMock.Setup(u => u.Rooms.GetRoomWithTypeAndDiscountsAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Room?)null);

            Func<Task> act = async () =>
                await _pricingService.CalculateTotalPriceAsync
                (Guid.NewGuid(), DateTime.Today, DateTime.Today.AddDays(1));

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Room not found");
        }

        [Fact]
        public async Task 
            CalculateTotalPriceAsync_ShouldThrowInvalidOperationException_WhenCheckOutBeforeCheckIn()
        {
            var dummyRoom = new Room
            {
                RoomType = new RoomType
                {
                    PricePerNight = 100,
                    Discounts = new List<Discount>()
                }
            };

            _unitOfWorkMock.Setup(u => u.Rooms.GetRoomWithTypeAndDiscountsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(dummyRoom);

            Func<Task> act = async () =>
                await _pricingService.CalculateTotalPriceAsync(Guid.NewGuid(), DateTime.Today, DateTime.Today);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Check-out date must be after check-in date.");
        }

        [Fact]
        public async Task CalculateTotalPriceAsync_ShouldCalculateCorrectPrice_WithoutDiscounts()
        {
            var roomType = new RoomType
            {
                PricePerNight = 100,
                Discounts = new List<Discount>()
            };
            var room = new Room { RoomType = roomType };

            _unitOfWorkMock.Setup(u => u.Rooms.GetRoomWithTypeAndDiscountsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(room);

            var checkIn = new DateTime(2025, 8, 1);
            var checkOut = new DateTime(2025, 8, 4); // 3 nights

            var price = await _pricingService.CalculateTotalPriceAsync(Guid.NewGuid(), checkIn, checkOut);

            price.Should().Be(300); // 100 * 3 nights, no discounts
        }

        [Fact]
        public async Task CalculateTotalPriceAsync_ShouldCalculateCorrectPrice_WithDiscounts()
        {
            var discounts = new List<Discount>
            {
                new Discount
                {
                    FromDate = new DateTime(2025, 8, 2),
                    ToDate = new DateTime(2025, 8, 3),
                    DiscountPercentage = 0.1f
                }
            };

            var roomType = new RoomType
            {
                PricePerNight = 100,
                Discounts = discounts
            };

            var room = new Room { RoomType = roomType };

            _unitOfWorkMock.Setup(u => u.Rooms.GetRoomWithTypeAndDiscountsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(room);

            var checkIn = new DateTime(2025, 8, 1);
            var checkOut = new DateTime(2025, 8, 4); // 3 nights: Aug 1, Aug 2, Aug 3

            // Expected:
            // Aug 1: no discount -> 100
            // Aug 2: 10% discount -> 90
            // Aug 3: 10% discount -> 90
            // Total = 280

            var price = await _pricingService.CalculateTotalPriceAsync(Guid.NewGuid(), checkIn, checkOut);

            price.Should().BeApproximately(280, 0.01f);
        }
    }
}