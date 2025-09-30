using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using TravelEase.Application.ImageManagement.Mappings;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.UnitTests.ImageManagement.Mapping
{
    public class ImageFormFileMapperTests
    {
        private readonly Fixture _fixture = new();

        [Theory]
        [InlineData("image/jpeg", ImageFormat.Jpeg)]
        [InlineData("image/png", ImageFormat.Png)]
        public async Task CreateFromFormFileAsync_ShouldReturnDto_WhenImageFormatIsSupported
            (string contentType, ImageFormat expectedFormat)
        {
            var fileContent = new byte[] { 1, 2, 3, 4, 5 };
            var fileMock = new Mock<IFormFile>();

            using var stream = new MemoryStream(fileContent);
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                    .Returns<Stream, CancellationToken>((s, _) => stream.CopyToAsync(s));
            fileMock.Setup(f => f.ContentType).Returns(contentType);

            var entityId = Guid.NewGuid();
            var type = ImageType.Gallery;

            var result = await ImageFormFileMapper.CreateFromFormFileAsync(entityId, fileMock.Object, type);

            result.Should().NotBeNull();
            result.EntityId.Should().Be(entityId);
            result.Type.Should().Be(type);
            result.Format.Should().Be(expectedFormat);

            var expectedBase64 = Convert.ToBase64String(fileContent);
            result.Base64Content.Should().Be(expectedBase64);
        }

        [Fact]
        public async Task CreateFromFormFileAsync_ShouldThrowException_WhenImageFormatIsNotSupported()
        {
            var fileMock = new Mock<IFormFile>();

            using var stream = new MemoryStream(new byte[] { 10, 20 });
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                    .Returns<Stream, CancellationToken>((s, _) => stream.CopyToAsync(s));
            fileMock.Setup(f => f.ContentType).Returns("image/gif");

            var entityId = Guid.NewGuid();
            var type = ImageType.Thumbnail;

            Func<Task> act = async () =>
                await ImageFormFileMapper.CreateFromFormFileAsync(entityId, fileMock.Object, type);

            await act.Should()
                .ThrowAsync<UnsupportedImageFormatException>();
        }
    }
}