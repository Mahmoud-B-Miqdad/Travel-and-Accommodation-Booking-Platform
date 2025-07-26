using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Handlers;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Tests.Application.UnitTests.HotelManagement.Handlers
{
    public class GetRecentlyVisitedHotelsForAuthenticatedGuestQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetRecentlyVisitedHotelsForAuthenticatedGuestQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetRecentlyVisitedHotelsForAuthenticatedGuestQueryHandlerTests()
        {
            _handler = new GetRecentlyVisitedHotelsForAuthenticatedGuestQueryHandler(
                _unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedHotels_WhenHotelsExist()
        {
            var query = _fixture.Create<GetRecentlyVisitedHotelsForAuthenticatedGuestQuery>();
            var hotels = _fixture.CreateMany<Hotel>(5).ToList();
            var hotelDtos = _fixture.CreateMany<HotelWithoutRoomsResponse>(5).ToList();

            _unitOfWorkMock.Setup(u =>
                u.Hotels.GetRecentlyVisitedHotelsForAuthenticatedGuestAsync(query.GuestEmail, query.Count))
                .ReturnsAsync(hotels);

            _mapperMock.Setup(m => m.Map<List<HotelWithoutRoomsResponse>>(hotels))
                .Returns(hotelDtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Should().BeEquivalentTo(hotelDtos);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoHotelsExist()
        {
            var query = _fixture.Create<GetRecentlyVisitedHotelsForAuthenticatedGuestQuery>();
            var hotels = new List<Hotel>();
            var hotelDtos = new List<HotelWithoutRoomsResponse>();

            _unitOfWorkMock.Setup(u =>
                u.Hotels.GetRecentlyVisitedHotelsForAuthenticatedGuestAsync(query.GuestEmail, query.Count))
                .ReturnsAsync(hotels);

            _mapperMock.Setup(m => m.Map<List<HotelWithoutRoomsResponse>>(hotels))
                .Returns(hotelDtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}