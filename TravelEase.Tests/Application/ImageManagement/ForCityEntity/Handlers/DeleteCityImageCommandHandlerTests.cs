using AutoFixture;
using FluentAssertions;
using Moq;
using TravelEase.Application.ImageManagement.ForCityEntity.Commands;
using TravelEase.Application.ImageManagement.ForCityEntity.Handlers;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.ImageManagement.ForCityEntity.Handlers
{
    public class DeleteCityImageCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IImageService> _imageServiceMock = new();
        private readonly DeleteCityImageCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public DeleteCityImageCommandHandlerTests()
        {
            _handler = new DeleteCityImageCommandHandler(
                _unitOfWorkMock.Object,
                _imageServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteImage_WhenCityExists()
        {
            var command = _fixture.Create<DeleteCityImageCommand>();

            _unitOfWorkMock.Setup(u => u.Cities.ExistsAsync(command.CityId)).ReturnsAsync(true);

            await _handler.Handle(command, default);

            _imageServiceMock.Verify(s => s.DeleteImageAsync(command.CityId, command.ImageId), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCityDoesNotExist()
        {
            var command = _fixture.Create<DeleteCityImageCommand>();

            _unitOfWorkMock.Setup(u => u.Cities.ExistsAsync(command.CityId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("City doesn't exist.");
        }
    }
}