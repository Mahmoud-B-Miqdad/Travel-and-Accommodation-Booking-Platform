using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Application.BookingManagement.Handlers;
using TravelEase.Application.BookingManagement.Queries;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.BookingManagement.Handlers
{
    public class GetAllBookingsByHotelIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllBookingsByHotelIdQueryHandler _handler;
        private readonly Fixture _fixture;

        public GetAllBookingsByHotelIdQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllBookingsByHotelIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedBookings_WhenHotelExists()
        {
            var query = _fixture.Build<GetAllBookingsByHotelIdQuery>()
                .With(q => q.HotelId, Guid.NewGuid())
                .With(q => q.PageNumber, 1)
                .With(q => q.PageSize, 10)
                .Create();

            var bookings = _fixture.CreateMany<Booking>(10).ToList();
            var pageData = new PageData(1, 10, 10);
            var paginatedBookings = new PaginatedList<Booking>(bookings, pageData);

            var bookingResponses = _fixture.CreateMany<BookingResponse>(10);
            var expectedResponse = new PaginatedList<BookingResponse>(bookingResponses.ToList(), pageData);

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.GetAllByHotelIdAsync
            (query.HotelId, query.PageNumber, query.PageSize))
                .ReturnsAsync(paginatedBookings);

            _mapperMock.Setup(m => m.Map<List<BookingResponse>>(bookings))
                .Returns(bookingResponses.ToList());

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(10);
            result.PageData.Should().BeEquivalentTo(pageData);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var query = _fixture.Build<GetAllBookingsByHotelIdQuery>()
                .With(q => q.HotelId, Guid.NewGuid())
                .Create();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Hotel doesn't exist.");
        }
    }
}