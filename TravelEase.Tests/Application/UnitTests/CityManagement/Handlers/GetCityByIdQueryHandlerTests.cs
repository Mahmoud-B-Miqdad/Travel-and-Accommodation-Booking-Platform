using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Application.CityManagement.Handlers;
using TravelEase.Application.CityManagement.Queries;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.UnitTests.CityManagement.Handlers
{
    public class GetCityByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetCityByIdQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetCityByIdQueryHandlerTests()
        {
            _handler = new GetCityByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCityDoesNotExist()
        {
            var query = _fixture.Create<GetCityByIdQuery>();

            _unitOfWorkMock.Setup(u => u.Cities.GetByIdAsync(query.Id))
                .ReturnsAsync((City?)null);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"City with Id {query.Id} was not found.");
        }

        [Fact]
        public async Task Handle_ShouldReturnCityResponse_WhenCityExists()
        {
            var query = _fixture.Create<GetCityByIdQuery>();
            var city = _fixture.Build<City>()
                .Without(c => c.Hotels)
                .Create();
            var cityResponse = _fixture.Create<CityResponse>();

            _unitOfWorkMock.Setup(u => u.Cities.GetByIdAsync(query.Id))
                .ReturnsAsync(city);

            _mapperMock.Setup(m => m.Map<CityResponse>(city))
                .Returns(cityResponse);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(cityResponse);
        }
    }
}