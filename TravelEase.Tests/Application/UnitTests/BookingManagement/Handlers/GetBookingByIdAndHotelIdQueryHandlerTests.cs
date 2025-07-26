using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Application.BookingManagement.Handlers;
using TravelEase.Application.BookingManagement.Queries;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.UnitTests.BookingManagement.Handlers
{
    public class GetBookingByIdAndHotelIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IOwnershipValidator> _ownershipValidatorMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetBookingByIdAndHotelIdQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetBookingByIdAndHotelIdQueryHandlerTests()
        {
            _handler = new GetBookingByIdAndHotelIdQueryHandler
                (_unitOfWorkMock.Object, _mapperMock.Object, _ownershipValidatorMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnBookingResponse_WhenAllChecksPass()
        {
            var query = _fixture.Build<GetBookingByIdAndHotelIdQuery>()
                .With(q => q.HotelId, Guid.NewGuid())
                .With(q => q.BookingId, Guid.NewGuid())
                .Create();

            var booking = _fixture.Build<Booking>()
                .With(b => b.Id, query.BookingId)
                .Create();

            var bookingResponse = _fixture.Create<BookingResponse>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId)).ReturnsAsync(true);
            _ownershipValidatorMock.Setup(v => v.IsBookingBelongsToHotelAsync
            (query.BookingId, query.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.GetByIdAsync(query.BookingId)).ReturnsAsync(booking);
            _mapperMock.Setup(m => m.Map<BookingResponse>(booking)).Returns(bookingResponse);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(bookingResponse);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var query = _fixture.Create<GetBookingByIdAndHotelIdQuery>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Hotel with ID {query.HotelId} doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenBookingDoesNotBelongToHotel()
        {
            var query = _fixture.Create<GetBookingByIdAndHotelIdQuery>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId)).ReturnsAsync(true);
            _ownershipValidatorMock.Setup(v => v.IsBookingBelongsToHotelAsync
            (query.BookingId, query.HotelId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Booking with ID {query.BookingId} does not belong to hotel {query.HotelId}.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenBookingNotFound()
        {
            var query = _fixture.Create<GetBookingByIdAndHotelIdQuery>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId)).ReturnsAsync(true);
            _ownershipValidatorMock.Setup(v => v.IsBookingBelongsToHotelAsync
            (query.BookingId, query.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.GetByIdAsync(query.BookingId)).ReturnsAsync((Booking)null);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Booking with ID {query.BookingId} was not found.");
        }
    }
}