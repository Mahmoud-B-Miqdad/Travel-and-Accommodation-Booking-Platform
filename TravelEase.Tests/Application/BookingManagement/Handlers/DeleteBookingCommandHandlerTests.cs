using AutoFixture;
using FluentAssertions;
using Moq;
using TravelEase.Application.BookingManagement.Commands;
using TravelEase.Application.BookingManagement.Handlers;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.BookingManagement.Handlers
{
    public class DeleteBookingCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IOwnershipValidator> _ownershipValidatorMock;
        private readonly DeleteBookingCommandHandler _handler;
        private readonly Fixture _fixture;

        public DeleteBookingCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _ownershipValidatorMock = new Mock<IOwnershipValidator>();
            _handler = new DeleteBookingCommandHandler(_unitOfWorkMock.Object, _ownershipValidatorMock.Object);
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldDeleteBooking_WhenAllChecksPass()
        {
            var command = _fixture.Build<DeleteBookingCommand>()
                .With(c => c.GuestEmail, "guest@example.com")
                .With(c => c.HotelId, Guid.NewGuid())
                .With(c => c.BookingId, Guid.NewGuid())
                .Create();

            var booking = _fixture.Build<Booking>()
                .With(b => b.Id, command.BookingId)
                .With(b => b.CheckInDate, DateTime.UtcNow.AddDays(1))
                .Create();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.GetByIdAsync(command.BookingId)).ReturnsAsync(booking);
            _ownershipValidatorMock.Setup(v => v.IsBookingBelongsToHotelAsync
            (command.BookingId, command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.IsBookingAccessibleToUserAsync
            (command.BookingId, command.GuestEmail)).ReturnsAsync(true);

            await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(u => u.Bookings.Remove(booking), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var command = _fixture.Create<DeleteBookingCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Hotel doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenBookingDoesNotExist()
        {
            var command = _fixture.Create<DeleteBookingCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.GetByIdAsync(command.BookingId)).ReturnsAsync((Booking)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Booking doesn't exist to delete.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenBookingDoesNotBelongToHotel()
        {
            var command = _fixture.Create<DeleteBookingCommand>();
            var booking = _fixture.Build<Booking>()
                .With(b => b.Id, command.BookingId)
                .With(b => b.CheckInDate, DateTime.UtcNow.AddDays(1))
                .Create();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.GetByIdAsync(command.BookingId)).ReturnsAsync(booking);
            _ownershipValidatorMock.Setup(v => v.IsBookingBelongsToHotelAsync
            (command.BookingId, command.HotelId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Booking with ID {command.BookingId} does not belong to hotel {command.HotelId}.");
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserCannotAccessBooking()
        {
            var command = _fixture.Create<DeleteBookingCommand>();
            var booking = _fixture.Build<Booking>()
                .With(b => b.Id, command.BookingId)
                .With(b => b.CheckInDate, DateTime.UtcNow.AddDays(1))
                .Create();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.GetByIdAsync(command.BookingId)).ReturnsAsync(booking);
            _ownershipValidatorMock.Setup(v => v.IsBookingBelongsToHotelAsync
            (command.BookingId, command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.IsBookingAccessibleToUserAsync
            (command.BookingId, command.GuestEmail)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("You are not authorized to cancel this booking.");
        }

        [Fact]
        public async Task Handle_ShouldThrowBookingCheckInDatePassedException_WhenCheckInDateIsPastOrToday()
        {
            var command = _fixture.Create<DeleteBookingCommand>();
            var booking = _fixture.Build<Booking>()
                .With(b => b.Id, command.BookingId)
                .With(b => b.CheckInDate, DateTime.UtcNow.Date) 
                .Create();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.GetByIdAsync(command.BookingId)).ReturnsAsync(booking);
            _ownershipValidatorMock.Setup(v => v.IsBookingBelongsToHotelAsync
            (command.BookingId, command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.IsBookingAccessibleToUserAsync
            (command.BookingId, command.GuestEmail)).ReturnsAsync(true);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<BookingCheckInDatePassedException>()
                .WithMessage("Cannot cancel booking after check-in date.");
        }
    }
}