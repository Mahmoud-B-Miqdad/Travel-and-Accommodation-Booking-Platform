using AutoMapper;
using MediatR;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Application.ReviewsManagement.Queries;
using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ReviewsManagement.Handlers
{
    public class GetReviewByIdAndHotelIdQueryHandler 
        : IRequestHandler<GetReviewByIdAndHotelIdQuery, ReviewResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOwnershipValidator _hotelOwnershipValidator;
        private readonly IMapper _mapper;

        public GetReviewByIdAndHotelIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOwnershipValidator hotelOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelOwnershipValidator = hotelOwnershipValidator;
        }

        public async Task<ReviewResponse?> Handle
            (GetReviewByIdAndHotelIdQuery request, CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);
            await EnsureReviewBelongsToHotelAsync(request.ReviewId, request.HotelId);

            var review = await GetReviewOrThrowAsync(request.ReviewId);

            return _mapper.Map<ReviewResponse>(review);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException($"Hotel with ID {hotelId} doesn't exist.");
        }

        private async Task EnsureReviewBelongsToHotelAsync(Guid reviewId, Guid hotelId)
        {
            if (!await _hotelOwnershipValidator.IsReviewBelongsToHotelAsync(reviewId, hotelId))
                throw new NotFoundException($"Review with ID {reviewId} does not belong to hotel {hotelId}.");
        }

        private async Task<Review> GetReviewOrThrowAsync(Guid reviewId)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);
            if (review == null)
                throw new NotFoundException($"Review with Id {reviewId} was not found.");
            return review;
        }
    }
}