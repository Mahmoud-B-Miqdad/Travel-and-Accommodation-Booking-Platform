using AutoFixture.AutoMoq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Application.ReviewsManagement.Handlers;
using TravelEase.Application.ReviewsManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;
using TravelEase.Domain.Aggregates.Reviews;

namespace TravelEase.Tests.Application.UnitTests.ReviewsManagement.Handlers
{
    public class GetReviewByIdAndHotelIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IOwnershipValidator> _ownershipValidatorMock = new();
        private readonly GetReviewByIdAndHotelIdQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetReviewByIdAndHotelIdQueryHandlerTests()
        {
            _handler = new GetReviewByIdAndHotelIdQueryHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _ownershipValidatorMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedReview_WhenAllChecksPass()
        {
            var query = _fixture.Create<GetReviewByIdAndHotelIdQuery>();
            var review = _fixture.Create<Review>();
            var mappedResponse = _fixture.Create<ReviewResponse>();

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(true);

            _ownershipValidatorMock.Setup(x =>
                    x.IsReviewBelongsToHotelAsync(query.ReviewId, query.HotelId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.Reviews.GetByIdAsync(query.ReviewId))
                .ReturnsAsync(review);

            _mapperMock.Setup(x => x.Map<ReviewResponse>(review))
                .Returns(mappedResponse);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(mappedResponse);

            _unitOfWorkMock.Verify(x => x.Hotels.ExistsAsync(query.HotelId), Times.Once);
            _ownershipValidatorMock.Verify(x => x.IsReviewBelongsToHotelAsync
            (query.ReviewId, query.HotelId), Times.Once);

            _unitOfWorkMock.Verify(x => x.Reviews.GetByIdAsync(query.ReviewId), Times.Once);
            _mapperMock.Verify(x => x.Map<ReviewResponse>(review), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var query = _fixture.Create<GetReviewByIdAndHotelIdQuery>();

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Hotel with ID {query.HotelId} doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenReviewNotBelongsToHotel()
        {
            var query = _fixture.Create<GetReviewByIdAndHotelIdQuery>();

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(true);

            _ownershipValidatorMock.Setup(x =>
                    x.IsReviewBelongsToHotelAsync(query.ReviewId, query.HotelId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Review with ID {query.ReviewId} does not belong to hotel {query.HotelId}.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenReviewDoesNotExist()
        {
            var query = _fixture.Create<GetReviewByIdAndHotelIdQuery>();

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(true);

            _ownershipValidatorMock.Setup(x =>
                    x.IsReviewBelongsToHotelAsync(query.ReviewId, query.HotelId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.Reviews.GetByIdAsync(query.ReviewId))
                .ReturnsAsync((Review?)null);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Review with Id {query.ReviewId} was not found.");
        }
    }
}