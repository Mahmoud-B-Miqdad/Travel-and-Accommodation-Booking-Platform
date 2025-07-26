using AutoFixture.AutoMoq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Application.ReviewsManagement.Handlers;
using TravelEase.Application.ReviewsManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;
using TravelEase.Domain.Aggregates.Reviews;

namespace TravelEase.Tests.Application.UnitTests.ReviewsManagement.Handlers
{
    public class GetAllReviewsByHotelIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetAllReviewsByHotelIdQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetAllReviewsByHotelIdQueryHandlerTests()
        {
            _handler = new GetAllReviewsByHotelIdQueryHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedReviews_WhenHotelExists()
        {
            var query = _fixture.Create<GetAllReviewsByHotelIdQuery>();
            var domainReviews = _fixture.CreateMany<Review>(10).ToList();
            var mappedReviews = _fixture.CreateMany<ReviewResponse>(10).ToList();

            var pageData = new PageData(100, 10, 1);
            var paginatedDomainReviews = new PaginatedList<Review>(domainReviews, pageData);

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.Reviews.GetAllByHotelIdAsync(
                    query.HotelId,
                    query.SearchQuery,
                    query.PageNumber,
                    query.PageSize))
                .ReturnsAsync(paginatedDomainReviews);

            _mapperMock.Setup(m => m.Map<List<ReviewResponse>>(domainReviews))
                .Returns(mappedReviews);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(10);
            result.PageData.Should().BeEquivalentTo(pageData);
            result.Items.Should().BeEquivalentTo(mappedReviews);

            _unitOfWorkMock.Verify(u => u.Hotels.ExistsAsync(query.HotelId), Times.Once);
            _unitOfWorkMock.Verify(u => u.Reviews.GetAllByHotelIdAsync(
                query.HotelId,
                query.SearchQuery,
                query.PageNumber,
                query.PageSize), Times.Once);

            _mapperMock.Verify(m => m.Map<List<ReviewResponse>>(domainReviews), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var query = _fixture.Create<GetAllReviewsByHotelIdQuery>();

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Hotel doesn't exist.");

            _unitOfWorkMock.Verify(x => x.Reviews.GetAllByHotelIdAsync(
                It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}