using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.HotelManagement.Handlers;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.HotelSearchModels;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Tests.Application.HotelManagement.Handlers
{
    public class HotelSearchQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly HotelSearchQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public HotelSearchQueryHandlerTests()
        {
            _handler = new HotelSearchQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedHotels_WhenHotelsMatchSearch()
        {
            var query = _fixture.Create<HotelSearchQuery>();
            var parameters = _fixture.Create<HotelSearchParameters>();
            var searchResult = _fixture.Create<PaginatedList<HotelSearchResult>>();

            _mapperMock.Setup(m => m.Map<HotelSearchParameters>(query)).Returns(parameters);
            _unitOfWorkMock.Setup(u => u.Hotels.HotelSearchAsync(parameters))
                .ReturnsAsync(searchResult);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(searchResult);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyPaginatedList_WhenNoHotelsMatchSearch()
        {
            var query = _fixture.Create<HotelSearchQuery>();
            var parameters = _fixture.Create<HotelSearchParameters>();

            var emptyResult = new PaginatedList<HotelSearchResult>(
                new List<HotelSearchResult>(), new PageData(0, 10, 1));

            _mapperMock.Setup(m => m.Map<HotelSearchParameters>(query)).Returns(parameters);
            _unitOfWorkMock.Setup(u => u.Hotels.HotelSearchAsync(parameters))
                .ReturnsAsync(emptyResult);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Items.Should().BeEmpty();
            result.PageData.TotalItems.Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldCallMapper_WithCorrectQuery()
        {
            var query = _fixture.Create<HotelSearchQuery>();
            var parameters = _fixture.Create<HotelSearchParameters>();
            var searchResult = _fixture.Create<PaginatedList<HotelSearchResult>>();

            _mapperMock.Setup(m => m.Map<HotelSearchParameters>(query)).Returns(parameters);
            _unitOfWorkMock.Setup(u => u.Hotels.HotelSearchAsync(parameters))
                .ReturnsAsync(searchResult);

            var result = await _handler.Handle(query, CancellationToken.None);

            _mapperMock.Verify(m => m.Map<HotelSearchParameters>(query), Times.Once);
        }
    }
}