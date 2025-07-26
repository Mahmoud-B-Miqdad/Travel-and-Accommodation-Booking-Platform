using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Handlers;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.UnitTests.HotelManagement.Handlers
{
    public class GetRecentlyVisitedHotelsForGuestQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetRecentlyVisitedHotelsForGuestQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetRecentlyVisitedHotelsForGuestQueryHandlerTests()
        {
            _handler = new GetRecentlyVisitedHotelsForGuestQueryHandler(
                _unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedHotels_WhenGuestExists()
        {
            var query = _fixture.Create<GetRecentlyVisitedHotelsForGuestQuery>();
            var hotels = _fixture.CreateMany<Hotel>(5).ToList();
            var mappedHotels = _fixture.CreateMany<HotelWithoutRoomsResponse>(5).ToList();

            _unitOfWorkMock.Setup(u => u.Users.ExistsAsync(query.GuestId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Hotels
                .GetRecentlyVisitedHotelsForGuestAsync(query.GuestId, query.Count))
                .ReturnsAsync(hotels);

            _mapperMock.Setup(m => m.Map<List<HotelWithoutRoomsResponse>>(hotels)).Returns(mappedHotels);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(mappedHotels);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoHotelsExist()
        {
            var query = _fixture.Create<GetRecentlyVisitedHotelsForGuestQuery>();
            var hotels = new List<Hotel>();
            var mapped = new List<HotelWithoutRoomsResponse>();

            _unitOfWorkMock.Setup(u => u.Users.ExistsAsync(query.GuestId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Hotels
                .GetRecentlyVisitedHotelsForGuestAsync(query.GuestId, query.Count))
                .ReturnsAsync(hotels);

            _mapperMock.Setup(m => m.Map<List<HotelWithoutRoomsResponse>>(hotels)).Returns(mapped);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenGuestDoesNotExist()
        {
            var query = _fixture.Create<GetRecentlyVisitedHotelsForGuestQuery>();
            _unitOfWorkMock.Setup(u => u.Users.ExistsAsync(query.GuestId)).ReturnsAsync(false);

            var act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"User With ID {query.GuestId} Doesn't Exists.");
        }
    }
}