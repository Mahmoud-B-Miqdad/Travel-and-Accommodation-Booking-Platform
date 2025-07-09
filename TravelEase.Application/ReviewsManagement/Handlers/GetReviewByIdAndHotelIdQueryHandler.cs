using AutoMapper;
using MediatR;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Application.ReviewsManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ReviewsManagement.Handlers
{
    public class GetReviewByIdAndHotelIdQueryHandler : IRequestHandler<GetReviewByIdAndHotelIdQuery, ReviewResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHotelOwnershipValidator _hotelOwnershipValidator;
        private readonly IMapper _mapper;

        public GetReviewByIdAndHotelIdQueryHandler
            (IUnitOfWork unitOfWork, IMapper mapper, IHotelOwnershipValidator hotelOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelOwnershipValidator = hotelOwnershipValidator;
        }

        public async Task<ReviewResponse?> Handle(GetReviewByIdAndHotelIdQuery request, CancellationToken cancellationToken)
        {
            var hotelExists = await _unitOfWork.Hotels.ExistsAsync(request.HotelId);
            if (!hotelExists)
                throw new NotFoundException($"Hotel with ID {request.HotelId} doesn't exist.");

            var belongsToHotel = await _hotelOwnershipValidator
                .IsReviewBelongsToHotelAsync(request.ReviewId, request.HotelId);
            if (!belongsToHotel)
                throw new NotFoundException
                    ($"Review with ID {request.ReviewId} does not belong to hotel {request.HotelId}.");

            var review = await _unitOfWork.Reviews.GetByIdAsync(request.ReviewId);
            if (review == null)
                throw new NotFoundException($"Review with Id {request.ReviewId} was not found.");

            return _mapper.Map<ReviewResponse>(review);
        }
    }
}