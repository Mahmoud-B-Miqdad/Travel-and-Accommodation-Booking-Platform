using AutoMapper;
using MediatR;
using TravelEase.Application.ReviewsManagement.DTOs.Commands;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

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
            var bookingExists = await _unitOfWork.Bookings.IsExistsAsync(request.BookingId);
            if (!bookingExists)
                throw new NotFoundException($"Booking with ID {request.BookingId} does not exist.");

            var isAuthorized = await _unitOfWork.Bookings.IsBookingAccessibleToUserAsync
                (request.BookingId, request.GuestEmail!);
            if (!isAuthorized)
                throw new UnauthorizedAccessException("The authenticated user is not the one who booked this room.");

            var reviewExists = await _unitOfWork.Reviews.IsExistsForBookingAsync(request.BookingId);
            if (reviewExists)
                throw new ConflictException("You already submitted a review for this booking.");

            var reviewToAdd = _mapper.Map<Review>(request);
            var addedReview = await _unitOfWork.Reviews.AddAsync(reviewToAdd);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ReviewResponse>(addedReview);
        }
    }
}