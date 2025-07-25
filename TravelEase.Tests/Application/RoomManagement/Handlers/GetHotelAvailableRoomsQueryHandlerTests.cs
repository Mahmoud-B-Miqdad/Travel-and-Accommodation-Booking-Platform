using AutoFixture.AutoMoq;
using AutoFixture;
using AutoMapper;
using Moq;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using TravelEase.Application.RoomManagement.Handlers;
using TravelEase.Application.RoomManagement.Queries;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Common.Interfaces;
using FluentAssertions;

namespace TravelEase.Tests.Application.RoomManagement.Handlers
{
    public class GetHotelAvailableRoomsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetHotelAvailableRoomsQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetHotelAvailableRoomsQueryHandlerTests()
        {
            _handler = new GetHotelAvailableRoomsQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnAvailableRooms()
        {
            var query = _fixture.Create<GetHotelAvailableRoomsQuery>();
            var roomEntities = _fixture.CreateMany<Room>(3).ToList();
            var roomResponses = _fixture.CreateMany<RoomResponse>(3).ToList();

            _unitOfWorkMock.Setup(x => x.Rooms.GetHotelAvailableRoomsAsync(
                query.HotelId, query.CheckInDate, query.CheckOutDate))
                .ReturnsAsync(roomEntities);

            _mapperMock.Setup(x => x.Map<List<RoomResponse>>(roomEntities))
                .Returns(roomResponses);

            var result = await _handler.Handle(query, default);

            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(roomResponses);

            _unitOfWorkMock.Verify(x => x.Rooms.GetHotelAvailableRoomsAsync(
                query.HotelId, query.CheckInDate, query.CheckOutDate), Times.Once);

            _mapperMock.Verify(x => x.Map<List<RoomResponse>>(roomEntities), Times.Once);
        }
    }
}