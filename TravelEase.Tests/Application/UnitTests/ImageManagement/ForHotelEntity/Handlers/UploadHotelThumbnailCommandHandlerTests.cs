using AutoFixture.AutoMoq;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using TravelEase.Application.ImageManagement.ForHotelEntity.Commands;
using TravelEase.Application.ImageManagement.ForHotelEntity.Handlers;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.ImageModels;
using TravelEase.Domain.Exceptions;
using TravelEase.Domain.Enums;

namespace TravelEase.Tests.Application.UnitTests.ImageManagement.ForHotelEntity.Handlers
{
    public class UploadHotelThumbnailCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IImageService> _imageServiceMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly UploadHotelThumbnailCommandHandler _handler;
        private readonly Fixture _fixture;

        public UploadHotelThumbnailCommandHandlerTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _handler = new UploadHotelThumbnailCommandHandler(
                _unitOfWorkMock.Object,
                _imageServiceMock.Object,
                _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUploadThumbnail_WhenHotelExists()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
            fileMock.Setup(f => f.Length).Returns(3);
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[] { 1, 2, 3 }));
            fileMock.Setup(f => f.FileName).Returns("thumbnail.jpg");

            var command = new UploadHotelThumbnailCommand
            {
                HotelId = Guid.NewGuid(),
                File = fileMock.Object
            };

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _imageServiceMock.Setup(s => s.UploadThumbnailAsync(It.IsAny<ImageCreationDTO>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(u => u.Hotels.ExistsAsync(command.HotelId), Times.Once);
            _imageServiceMock.Verify(s => s.UploadThumbnailAsync(It.Is<ImageCreationDTO>(
                dto => dto.EntityId == command.HotelId && dto.Type == ImageType.Thumbnail)), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var command = _fixture.Create<UploadHotelThumbnailCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Hotel doesn't exist.");

            _imageServiceMock.Verify(s => s.UploadThumbnailAsync(It.IsAny<ImageCreationDTO>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}