using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using TravelEase.Application.ImageManagement.ForCityEntity.Commands;
using TravelEase.Application.ImageManagement.ForCityEntity.Handlers;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.ImageModels;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.ImageManagement.ForCityEntity.Handlers
{
    public class UploadCityImageCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IImageService> _imageServiceMock = new();
        private readonly UploadCityImageCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public UploadCityImageCommandHandlerTests()
        {
            _handler = new UploadCityImageCommandHandler(_unitOfWorkMock.Object, _imageServiceMock.Object);

            _fixture.Customize(new AutoMoqCustomization());

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldUploadImage_WhenCityExists()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
            fileMock.Setup(f => f.Length).Returns(3);
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[] { 1, 2, 3 }));
            fileMock.Setup(f => f.FileName).Returns("test.jpg");

            var command = new UploadCityImageCommand
            {
                CityId = Guid.NewGuid(),
                File = fileMock.Object
            };

            _unitOfWorkMock.Setup(u => u.Cities.ExistsAsync(command.CityId))
                .ReturnsAsync(true);

            _imageServiceMock.Setup(s => s.UploadImageAsync(It.IsAny<ImageCreationDTO>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(u => u.Cities.ExistsAsync(command.CityId), Times.Once);
            _imageServiceMock.Verify(s => s.UploadImageAsync(It.IsAny<ImageCreationDTO>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCityDoesNotExist()
        {
            var command = _fixture.Create<UploadCityImageCommand>();

            _unitOfWorkMock.Setup(u => u.Cities.ExistsAsync(command.CityId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("City doesn't exist.");

            _imageServiceMock.Verify(s => s.UploadImageAsync(It.IsAny<ImageCreationDTO>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}