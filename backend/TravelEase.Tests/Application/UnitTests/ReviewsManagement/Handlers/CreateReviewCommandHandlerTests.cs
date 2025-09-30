using AutoFixture.AutoMoq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.ReviewsManagement.Commands;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Application.ReviewsManagement.Handlers;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;
using TravelEase.Domain.Aggregates.Reviews;

namespace TravelEase.Tests.Application.UnitTests.ReviewsManagement.Handlers
{
    public class CreateReviewCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IOwnershipValidator> _ownershipValidatorMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Fixture _fixture = new();
        private readonly CreateReviewCommandHandler _handler;

        public CreateReviewCommandHandlerTests()
        {
            _handler = new CreateReviewCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _ownershipValidatorMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnReviewResponse_WhenAllChecksPass()
        {
            var command = _fixture.Create<CreateReviewCommand>();
            var reviewEntity = _fixture.Create<Review>();
            var reviewResponse = _fixture.Create<ReviewResponse>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.ExistsAsync(command.BookingId)).ReturnsAsync(true);
            _ownershipValidatorMock.Setup(o => o.IsBookingBelongsToHotelAsync
            (command.BookingId, command.HotelId)).ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Bookings.IsBookingAccessibleToUserAsync
            (command.BookingId, command.GuestEmail!)).ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Reviews.IsExistsForBookingAsync(command.BookingId))
                .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<Review>(command)).Returns(reviewEntity);
            _unitOfWorkMock.Setup(u => u.Reviews.AddAsync(reviewEntity)).ReturnsAsync(reviewEntity);
            _mapperMock.Setup(m => m.Map<ReviewResponse>(reviewEntity)).Returns(reviewResponse);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeEquivalentTo(reviewResponse);

            _unitOfWorkMock.Verify(u => u.Hotels.ExistsAsync(command.HotelId), Times.Once);
            _unitOfWorkMock.Verify(u => u.Bookings.ExistsAsync(command.BookingId), Times.Once);
            _ownershipValidatorMock.Verify(o => o.IsBookingBelongsToHotelAsync
            (command.BookingId, command.HotelId), Times.Once);

            _unitOfWorkMock.Verify(u => u.Bookings.IsBookingAccessibleToUserAsync
            (command.BookingId, command.GuestEmail!), Times.Once);

            _unitOfWorkMock.Verify(u => u.Reviews.IsExistsForBookingAsync(command.BookingId), Times.Once);
            _unitOfWorkMock.Verify(u => u.Reviews.AddAsync(reviewEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var command = _fixture.Create<CreateReviewCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Hotel doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenBookingDoesNotExist()
        {
            var command = _fixture.Create<CreateReviewCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.ExistsAsync(command.BookingId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Booking with ID {command.BookingId} does not exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenBookingDoesNotBelongToHotel()
        {
            var command = _fixture.Create<CreateReviewCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.ExistsAsync(command.BookingId)).ReturnsAsync(true);
            _ownershipValidatorMock.Setup(o => o.IsBookingBelongsToHotelAsync
            (command.BookingId, command.HotelId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Booking does not belong to the specified hotel.");
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserNotAuthorized()
        {
            var command = _fixture.Create<CreateReviewCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.ExistsAsync(command.BookingId)).ReturnsAsync(true);
            _ownershipValidatorMock.Setup(o => o.IsBookingBelongsToHotelAsync
            (command.BookingId, command.HotelId)).ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Bookings.IsBookingAccessibleToUserAsync
            (command.BookingId, command.GuestEmail!)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("The authenticated user is not the one who booked this room.");
        }

        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenReviewAlreadyExists()
        {
            var command = _fixture.Create<CreateReviewCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.ExistsAsync(command.BookingId)).ReturnsAsync(true);
            _ownershipValidatorMock.Setup(o => o.IsBookingBelongsToHotelAsync
            (command.BookingId, command.HotelId)).ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Bookings.IsBookingAccessibleToUserAsync
            (command.BookingId, command.GuestEmail!)).ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Reviews.IsExistsForBookingAsync(command.BookingId)).ReturnsAsync(true);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage("You already submitted a review for this booking.");
        }
    }
}