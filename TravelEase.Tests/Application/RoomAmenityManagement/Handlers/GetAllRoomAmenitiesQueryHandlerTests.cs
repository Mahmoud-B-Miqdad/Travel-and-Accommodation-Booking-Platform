using AutoFixture;
using AutoMapper;
using Moq;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Application.RoomAmenityManagement.Handlers;
using TravelEase.Application.RoomAmenityManagement.Query;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using FluentAssertions;

namespace TravelEase.Tests.Application.RoomAmenityManagement.Handlers
{
    public class GetAllRoomAmenitiesQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetAllRoomAmenitiesQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetAllRoomAmenitiesQueryHandlerTests()
        {
            _handler = new GetAllRoomAmenitiesQueryHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedRoomAmenities()
        {
            var query = _fixture.Create<GetAllRoomAmenitiesQuery>();

            var roomAmenities = _fixture.CreateMany<RoomAmenity>(10).ToList();
            var paginatedEntities = new PaginatedList<RoomAmenity>(
                roomAmenities,
                new PageData(totalItems: 10, pageSize: 5, currentPage: 1));

            var mappedResponses = _fixture.CreateMany<RoomAmenityResponse>(10).ToList();

            _unitOfWorkMock.Setup(u => u.RoomAmenities.GetAllAsync(
                    query.SearchQuery,
                    query.PageNumber,
                    query.PageSize))
                .ReturnsAsync(paginatedEntities);

            _mapperMock.Setup(m => m.Map<List<RoomAmenityResponse>>(roomAmenities))
                .Returns(mappedResponses);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(10);
            result.Items.Should().BeEquivalentTo(mappedResponses);
            result.PageData.Should().BeEquivalentTo(paginatedEntities.PageData);

            _unitOfWorkMock.Verify(u => u.RoomAmenities.GetAllAsync(
                query.SearchQuery, query.PageNumber, query.PageSize), Times.Once);

            _mapperMock.Verify(m => m.Map<List<RoomAmenityResponse>>(roomAmenities), Times.Once);
        }
    }
}