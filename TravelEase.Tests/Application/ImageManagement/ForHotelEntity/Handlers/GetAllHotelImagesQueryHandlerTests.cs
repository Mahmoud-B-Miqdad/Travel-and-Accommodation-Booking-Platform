using AutoFixture;
using FluentAssertions;
using Moq;
using TravelEase.Application.ImageManagement.ForHotelEntity.Handlers;
using TravelEase.Application.ImageManagement.ForHotelEntity.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.ImageManagement.ForHotelEntity.Handlers
{
    public class GetAllHotelImagesQueryHandlerTests
    {
        private readonly Mock<IImageService> _imageServiceMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly GetAllHotelImagesQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetAllHotelImagesQueryHandlerTests()
        {
            _handler = new GetAllHotelImagesQueryHandler(_imageServiceMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnImages_WhenHotelExists()
        {
            var query = _fixture.Create<GetAllHotelImagesQuery>();

            var expectedImages = new PaginatedList<string>(
                _fixture.CreateMany<string>(10).ToList(),
                new PageData(20, 10, 1));

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(true);

            _imageServiceMock.Setup(x => x.GetAllImagesAsync(query.HotelId, query.PageNumber, query.PageSize))
                .ReturnsAsync(expectedImages);

            var result = await _handler.Handle(query, default);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(10);
            result.PageData.TotalItems.Should().Be(20);
            result.Should().BeEquivalentTo(expectedImages);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var query = _fixture.Create<GetAllHotelImagesQuery>();

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Hotel doesn't exist.");
        }
    }
}