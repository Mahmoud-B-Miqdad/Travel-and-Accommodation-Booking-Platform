using CloudinaryDotNet;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TravelEase.Domain.Aggregates.Images;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Common.Models.SettingModels;
using TravelEase.Domain.Exceptions;
using TravelEase.Infrastructure.Persistence.Services.ImageServices;

namespace TravelEase.Tests.Infrastructure.UnitTests.ImageServices
{
    public class CloudinaryImageServiceTests
    {
        private readonly Mock<IImageRepository> _imageRepoMock;
        private readonly Mock<ILogger<CloudinaryImageService>> _loggerMock;
        private readonly CloudinaryImageService _service;

        private readonly string _base64Image = "iVBORw0KGgoAAAANSUhEUgA=";

        public CloudinaryImageServiceTests()
        {
            _imageRepoMock = new Mock<IImageRepository>();
            _loggerMock = new Mock<ILogger<CloudinaryImageService>>();

            var settings = Options.Create(new CloudinarySettings
            {
                CloudName = "test",
                ApiKey = "key",
                ApiSecret = "secret"
            });

            var cloudinaryMock = new Mock<Cloudinary>(new Account("test", "key", "secret"));
            _service = new CloudinaryImageService(settings, _imageRepoMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllImagesAsync_ShouldCallRepositoryAndReturnResult()
        {
            var expected = new PaginatedList<string>(new List<string> 
            { "url1", "url2" }, new PageData(1, 2, 1));

            _imageRepoMock.Setup(r => r.GetAllImageUrlsByEntityIdAsync(It.IsAny<Guid>(), 1, 10))
                .ReturnsAsync(expected);

            var result = await _service.GetAllImagesAsync(Guid.NewGuid(), 1, 10);

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task DeleteImageAsync_ShouldThrowNotFound_IfImageNotFound()
        {
            var entityId = Guid.NewGuid();
            var imageId = Guid.NewGuid();

            _imageRepoMock.Setup(r => r.GetSingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Image, bool>>>()))
                .ReturnsAsync((Image)null!);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.DeleteImageAsync(entityId, imageId));
        }
    }
}