using AutoFixture;
using FluentAssertions;
using Moq;
using TravelEase.Application.ImageManagement.ForHotelEntity.Commands;
using TravelEase.Application.ImageManagement.ForHotelEntity.Handlers;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.UnitTests.ImageManagement.ForHotelEntity.Handlers
{
    public class DeleteHotelImageCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IImageService> _imageServiceMock = new();
        private readonly DeleteHotelImageCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public DeleteHotelImageCommandHandlerTests()
        {
            _handler = new DeleteHotelImageCommandHandler(
                _unitOfWorkMock.Object,
                _imageServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteImage_WhenHotelExists()
        {
            var command = _fixture.Create<DeleteHotelImageCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);

            await _handler.Handle(command, default);

            _imageServiceMock.Verify(s => s.DeleteImageAsync(command.HotelId, command.ImageId), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var command = _fixture.Create<DeleteHotelImageCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Hotel doesn't exist.");

            _imageServiceMock.Verify(s => s.DeleteImageAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Never);
        }
    }
}