using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.CityManagement.Commands;
using TravelEase.Application.CityManagement.Handlers;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.UnitTests.CityManagement.Handlers
{
    public class UpdateCityCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly UpdateCityCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public UpdateCityCommandHandlerTests()
        {
            _handler = new UpdateCityCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCityDoesNotExist()
        {
            var command = _fixture.Create<UpdateCityCommand>();

            _unitOfWorkMock.Setup(u => u.Cities.ExistsAsync(command.Id))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"City with ID {command.Id} doesn't exist to update.");
        }

        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenCityNameExists()
        {
            var command = _fixture.Create<UpdateCityCommand>();

            _unitOfWorkMock.Setup(u => u.Cities.ExistsAsync(command.Id))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Cities.ExistsAsync(command.Name))
                .ReturnsAsync(true);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage($"City with name '{command.Name}' already exists.");
        }

        [Fact]
        public async Task Handle_ShouldUpdateCityAndSave_WhenValidRequest()
        {
            var command = _fixture.Create<UpdateCityCommand>();
            var city = _fixture.Create<City>();

            _unitOfWorkMock.Setup(u => u.Cities.ExistsAsync(command.Id))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Cities.ExistsAsync(command.Name))
                .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<City>(command))
                .Returns(city);

            _unitOfWorkMock.Setup(u => u.Cities.Update(city));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None))
                .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(u => u.Cities.Update(city), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}