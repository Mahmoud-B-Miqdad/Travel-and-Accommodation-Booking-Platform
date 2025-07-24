using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.BookingManagement.Commands;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Application.BookingManagement.Handlers;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.BookingManagement.Handlers
{
    public class ReserveRoomCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IPricingService> _pricingServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IOwnershipValidator> _ownershipValidatorMock = new();
        private readonly ReserveRoomCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public ReserveRoomCommandHandlerTests()
        {
            _handler = new ReserveRoomCommandHandler(
                _unitOfWorkMock.Object,
                _pricingServiceMock.Object,
                _mapperMock.Object,
                _ownershipValidatorMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var command = _fixture.Create<ReserveRoomCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Hotel doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomNotFound()
        {
            var command = _fixture.Create<ReserveRoomCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Rooms.GetByIdAsync(command.RoomId)).ReturnsAsync((Room?)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Room with ID {command.RoomId} doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomNotBelongsToHotel()
        {
            var command = _fixture.Create<ReserveRoomCommand>();
            var room = _fixture.Create<Room>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Rooms.GetByIdAsync(command.RoomId)).ReturnsAsync(room);
            _ownershipValidatorMock.Setup(v => v.IsRoomBelongsToHotelAsync(command.RoomId, command.HotelId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Room with ID {command.RoomId} does not belong to hotel {command.HotelId}.");
        }

        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenBookingConflictExists()
        {
            var command = _fixture.Create<ReserveRoomCommand>();
            var room = _fixture.Create<Room>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Rooms.GetByIdAsync(command.RoomId)).ReturnsAsync(room);
            _ownershipValidatorMock.Setup(v => v.IsRoomBelongsToHotelAsync(command.RoomId, command.HotelId))
                .ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.ExistsConflictingBookingAsync(
                command.RoomId, command.CheckInDate, command.CheckOutDate)).ReturnsAsync(true);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage($"Can't book a date between " +
                $"{command.CheckInDate:yyyy-MM-dd} and {command.CheckOutDate:yyyy-MM-dd}");
        }

        [Fact]
        public async Task Handle_ShouldReturnBookingResponse_WhenValidRequest()
        {
            var command = _fixture.Create<ReserveRoomCommand>();
            var room = _fixture.Create<Room>();
            var user = _fixture.Build<User>().With(u => u.Email, command.GuestEmail).Create();
            var booking = _fixture.Create<Booking>();
            var bookingResponse = _fixture.Create<BookingResponse>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Rooms.GetByIdAsync(command.RoomId)).ReturnsAsync(room);
            _ownershipValidatorMock.Setup(v => v.IsRoomBelongsToHotelAsync
            (command.RoomId, command.HotelId)).ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Bookings.ExistsConflictingBookingAsync
            (command.RoomId, command.CheckInDate, command.CheckOutDate)).ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<Booking>(command)).Returns(booking);
            _unitOfWorkMock.Setup(u => u.Users.GetByEmailAsync
            (command.GuestEmail)).ReturnsAsync(user);

            _pricingServiceMock.Setup(p => p.CalculateTotalPriceAsync
            (command.RoomId, command.CheckInDate, command.CheckOutDate)).ReturnsAsync(100);

            _unitOfWorkMock.Setup(u => u.Bookings.AddAsync(It.IsAny<Booking>())).ReturnsAsync(booking);
            _mapperMock.Setup(m => m.Map<BookingResponse>(booking)).Returns(bookingResponse);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeEquivalentTo(bookingResponse);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}