using AutoFixture;
using FluentAssertions;
using Moq;
using TravelEase.Application.ImageManagement.ForCityEntity.Handlers;
using TravelEase.Application.ImageManagement.ForCityEntity.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.ImageManagement.ForCityEntity.Handlers
{
    public class GetAllCityImagesQueryHandlerTests
    {
        private readonly Mock<IImageService> _imageServiceMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly GetAllCityImagesQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetAllCityImagesQueryHandlerTests()
        {
            _handler = new GetAllCityImagesQueryHandler(_imageServiceMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnImages_WhenCityExists()
        {
            var query = _fixture.Create<GetAllCityImagesQuery>();

            var expectedImages = new PaginatedList<string>(
                _fixture.CreateMany<string>(10).ToList(),
                new PageData(20, 10, 1));

            _unitOfWorkMock.Setup(x => x.Cities.ExistsAsync(query.CityId))
                .ReturnsAsync(true);

            _imageServiceMock.Setup(x => x.GetAllImagesAsync(
                query.CityId, query.PageNumber, query.PageSize))
                .ReturnsAsync(expectedImages);

            var result = await _handler.Handle(query, default);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(10);
            result.PageData.TotalItems.Should().Be(20);
            result.Should().BeEquivalentTo(expectedImages);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCityDoesNotExist()
        {
            var query = _fixture.Create<GetAllCityImagesQuery>();

            _unitOfWorkMock.Setup(x => x.Cities.ExistsAsync(query.CityId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("City doesn't exist.");
        }
    }
}