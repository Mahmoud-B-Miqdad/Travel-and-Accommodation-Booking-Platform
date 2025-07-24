using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.CityManagement.Commands;
using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Application.CityManagement.Handlers;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.CityManagement.Handlers
{
    public class CreateCityCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly CreateCityCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public CreateCityCommandHandlerTests()
        {
            _handler = new CreateCityCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenCityAlreadyExists()
        {
            var command = _fixture.Create<CreateCityCommand>();

            _unitOfWorkMock.Setup(u => u.Cities.ExistsAsync(command.Name))
                .ReturnsAsync(true);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage($"City with name '{command.Name}' already exists.");
        }

        [Fact]
        public async Task Handle_ShouldReturnCityResponse_WhenCityIsCreatedSuccessfully()
        {
            var command = _fixture.Create<CreateCityCommand>();
            var city = _fixture.Build<City>().With(c => c.Name, command.Name).Create();
            var cityResponse = _fixture.Create<CityWithoutHotelsResponse>();

            _unitOfWorkMock.Setup(u => u.Cities.ExistsAsync(command.Name))
                .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<City>(command)).Returns(city);
            _unitOfWorkMock.Setup(u => u.Cities.AddAsync(city)).ReturnsAsync(city);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None))
                .ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<CityWithoutHotelsResponse>(city)).Returns(cityResponse);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeEquivalentTo(cityResponse);
            _unitOfWorkMock.Verify(u => u.Cities.AddAsync(city), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}