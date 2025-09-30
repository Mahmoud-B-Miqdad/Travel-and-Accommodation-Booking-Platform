using AutoFixture.AutoMoq;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using TravelEase.Application.ImageManagement.ForCityEntity.Commands;
using TravelEase.Application.ImageManagement.ForCityEntity.Handlers;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;
using TravelEase.Domain.Common.Models.ImageModels;
using TravelEase.Domain.Enums;

namespace TravelEase.Tests.Application.UnitTests.ImageManagement.ForCityEntity.Handlers
{
    public class UploadCityThumbnailCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IImageService> _imageServiceMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly UploadCityThumbnailCommandHandler _handler;
        private readonly Fixture _fixture;

        public UploadCityThumbnailCommandHandlerTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _handler = new UploadCityThumbnailCommandHandler(
                _unitOfWorkMock.Object,
                _imageServiceMock.Object,
                _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUploadThumbnail_WhenCityExists()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
            fileMock.Setup(f => f.Length).Returns(3);
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[] { 1, 2, 3 }));
            fileMock.Setup(f => f.FileName).Returns("thumbnail.jpg");

            var command = new UploadCityThumbnailCommand
            {
                CityId = Guid.NewGuid(),
                File = fileMock.Object
            };

            _unitOfWorkMock.Setup(u => u.Cities.ExistsAsync(command.CityId)).ReturnsAsync(true);
            _imageServiceMock.Setup(s => s.UploadThumbnailAsync(It.IsAny<ImageCreationDTO>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(u => u.Cities.ExistsAsync(command.CityId), Times.Once);
            _imageServiceMock.Verify(s => s.UploadThumbnailAsync(It.Is<ImageCreationDTO>(
                dto => dto.EntityId == command.CityId && dto.Type == ImageType.Thumbnail)), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCityDoesNotExist()
        {
            var command = _fixture.Create<UploadCityThumbnailCommand>();

            _unitOfWorkMock.Setup(u => u.Cities.ExistsAsync(command.CityId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("City doesn't exist.");

            _imageServiceMock.Verify(s => s.UploadThumbnailAsync(It.IsAny<ImageCreationDTO>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}