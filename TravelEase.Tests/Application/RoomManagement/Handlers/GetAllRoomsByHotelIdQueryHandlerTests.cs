using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using TravelEase.Application.RoomManagement.Handlers;
using TravelEase.Application.RoomManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;
using TravelEase.Domain.Aggregates.Rooms;

namespace TravelEase.Tests.Application.RoomManagement.Handlers
{
    public class GetAllRoomsByHotelIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetAllRoomsByHotelIdQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetAllRoomsByHotelIdQueryHandlerTests()
        {
            _handler = new GetAllRoomsByHotelIdQueryHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedRoomResponses_WhenHotelExists()
        {
            var query = _fixture.Create<GetAllRoomsByHotelIdQuery>();

            var roomEntities = _fixture.CreateMany<Room>(5).ToList();

            var paginatedRooms = new PaginatedList<Room>(
                roomEntities,
                new PageData(totalItems: 5, pageSize: 5, currentPage: 1));

            var roomResponseList = _fixture.CreateMany<RoomResponse>(5).ToList();

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.Rooms.GetAllByHotelIdAsync(
                    query.HotelId,
                    query.SearchQuery,
                    query.PageNumber,
                    query.PageSize))
                .ReturnsAsync(paginatedRooms); 

            _mapperMock.Setup(x => x.Map<List<RoomResponse>>(roomEntities))
                .Returns(roomResponseList);


            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(5);
            result.PageData.TotalItems.Should().Be(5);

            _unitOfWorkMock.Verify(x => x.Hotels.ExistsAsync(query.HotelId), Times.Once);
            _unitOfWorkMock.Verify(x => x.Rooms.GetAllByHotelIdAsync(
                query.HotelId, query.SearchQuery, query.PageNumber, query.PageSize), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var query = _fixture.Create<GetAllRoomsByHotelIdQuery>();

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Hotel doesn't exist.");
        }
    }
}