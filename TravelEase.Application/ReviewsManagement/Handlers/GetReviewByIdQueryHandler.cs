using AutoMapper;
using MediatR;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Application.ReviewsManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ReviewsManagement.Handlers
{
    public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdAndHotelIdQuery, ReviewResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetReviewByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReviewResponse?> Handle(GetReviewByIdAndHotelIdQuery request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(request.Id);
            if (review == null)
                throw new NotFoundException($"Review with Id {request.Id} was not found.");

            return _mapper.Map<ReviewResponse>(review);
        }
    }
}