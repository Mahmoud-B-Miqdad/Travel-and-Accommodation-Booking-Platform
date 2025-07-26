using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Handlers;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Tests.Application.UnitTests.HotelManagement.Handlers
{
    public class GetAllHotelsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetAllHotelsQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetAllHotelsQueryHandlerTests()
        {
            _handler = new GetAllHotelsQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedPaginatedList()
        {
            var query = _fixture.Create<GetAllHotelsQuery>();

            var hotelList = _fixture.CreateMany<Hotel>(10);
            var paginatedHotels = new PaginatedList<Hotel>(
                hotelList.ToList(),
                new PageData(1, 10, 10));

            var responseList = _fixture.CreateMany<HotelWithoutRoomsResponse>(10);

            _unitOfWorkMock.Setup(u => u.Hotels.GetAllAsync(
                    query.SearchQuery, query.PageNumber, query.PageSize))
                .ReturnsAsync(paginatedHotels);

            _mapperMock.Setup(m => m.Map<List<HotelWithoutRoomsResponse>>(paginatedHotels.Items))
                .Returns(responseList.ToList());

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(10);
            result.PageData.Should().BeEquivalentTo(paginatedHotels.PageData);
            result.Items.Should().BeEquivalentTo(responseList);
        }
    }
}