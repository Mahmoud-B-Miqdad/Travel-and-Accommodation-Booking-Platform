using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Application.CityManagement.Handlers;
using TravelEase.Application.CityManagement.Queries;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Tests.Application.UnitTests.CityManagement.Handlers
{
    public class GetAllCitiesQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetAllCitiesQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetAllCitiesQueryHandlerTests()
        {
            _handler = new GetAllCitiesQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedPaginatedList()
        {
            var query = _fixture.Create<GetAllCitiesQuery>();

            var citiesList = _fixture.CreateMany<City>(10);
            var paginatedCities = new PaginatedList<City>(
                new List<City>(citiesList),
                new PageData(1, 10, 10));

            var cityResponses = _fixture.CreateMany<CityResponse>(10);

            _unitOfWorkMock.Setup(u => u.Cities.GetAllAsync(
                    query.IncludeHotels, query.SearchQuery, query.PageNumber, query.PageSize))
                .ReturnsAsync(paginatedCities);

            _mapperMock.Setup(m => m.Map<List<CityResponse>>(paginatedCities.Items))
                .Returns(new List<CityResponse>(cityResponses));

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(10);
            result.PageData.Should().BeEquivalentTo(paginatedCities.PageData);
            result.Items.Should().BeEquivalentTo(cityResponses);
        }
    }
}