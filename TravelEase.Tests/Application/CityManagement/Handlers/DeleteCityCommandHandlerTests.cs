using FluentAssertions;
using Moq;
using TravelEase.Application.CityManagement.Commands;
using TravelEase.Application.CityManagement.Handlers;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.CityManagement.Handlers
{
    public class DeleteCityCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly DeleteCityCommandHandler _handler;

        public DeleteCityCommandHandlerTests()
        {
            _handler = new DeleteCityCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCityDoesNotExist()
        {
            var command = new DeleteCityCommand { Id = Guid.NewGuid() };

            _unitOfWorkMock.Setup(u => u.Cities.GetByIdAsync(command.Id))
                .ReturnsAsync((City?)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("City doesn't exist to delete.");
        }

        [Fact]
        public async Task Handle_ShouldRemoveCityAndSaveChanges_WhenCityExists()
        {
            var command = new DeleteCityCommand { Id = Guid.NewGuid() };
            var city = new City { Id = command.Id, Name = "Test City" };

            _unitOfWorkMock.Setup(u => u.Cities.GetByIdAsync(command.Id))
                .ReturnsAsync(city);

            _unitOfWorkMock.Setup(u => u.Cities.Remove(city));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None))
                .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(u => u.Cities.Remove(city), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}