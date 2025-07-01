using AutoMapper;
using MediatR;
using TravelEase.Application.ReviewsManagement.DTOs.Commands;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Application.ReviewsManagement.Handlers
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReviewResponse?> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var reviewToAdd = _mapper.Map<Review>(request);
            var addedReview = await _unitOfWork.Reviews.AddAsync(reviewToAdd);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ReviewResponse>(addedReview);
        }
    }
}