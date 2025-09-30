using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Application.CityManagement.Handlers;
using TravelEase.Application.CityManagement.Queries;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Tests.Application.UnitTests.CityManagement.Handlers
{
    public class GetTrendingCitiesQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetTrendingCitiesQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetTrendingCitiesQueryHandlerTests()
        {
            _handler = new GetTrendingCitiesQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedTrendingCitiesList()
        {
            var query = _fixture.Create<GetTrendingCitiesQuery>();
            var cities = _fixture.CreateMany<City>(5).ToList();
            var mappedCities = _fixture.CreateMany<CityWithoutHotelsResponse>(5);

            _unitOfWorkMock.Setup(u => u.Cities.GetTrendingCitiesAsync(query.Count))
                .ReturnsAsync(cities);

            _mapperMock.Setup(m => m.Map<List<CityWithoutHotelsResponse>>(cities))
                .Returns(new List<CityWithoutHotelsResponse>(mappedCities));

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Should().BeEquivalentTo(mappedCities);
        }
    }
}