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
        private readonly IHotelOwnershipValidator _hotelOwnershipValidator;
        private readonly IMapper _mapper;

        public CreateReviewCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHotelOwnershipValidator hotelOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelOwnershipValidator = hotelOwnershipValidator;
        }

        public async Task<ReviewResponse?> Handle
            (CreateReviewCommand request, CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);
            await EnsureBookingExistsAsync(request.BookingId);
            await EnsureBookingBelongsToHotelAsync(request.BookingId, request.HotelId);
            await EnsureUserIsAuthorizedAsync(request.BookingId, request.GuestEmail!);
            await EnsureNoExistingReviewAsync(request.BookingId);

            var review = _mapper.Map<Review>(request);
            var addedReview = await _unitOfWork.Reviews.AddAsync(review);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ReviewResponse>(addedReview);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }

        private async Task EnsureBookingExistsAsync(Guid bookingId)
        {
            if (!await _unitOfWork.Bookings.ExistsAsync(bookingId))
                throw new NotFoundException($"Booking with ID {bookingId} does not exist.");
        }

        private async Task EnsureBookingBelongsToHotelAsync(Guid bookingId, Guid hotelId)
        {
            if (!await _hotelOwnershipValidator.IsBookingBelongsToHotelAsync(bookingId, hotelId))
                throw new NotFoundException("Booking does not belong to the specified hotel.");
        }

        private async Task EnsureUserIsAuthorizedAsync(Guid bookingId, string guestEmail)
        {
            if (!await _unitOfWork.Bookings.IsBookingAccessibleToUserAsync(bookingId, guestEmail))
                throw new UnauthorizedAccessException("The authenticated user is not the one who booked this room.");
        }

        private async Task EnsureNoExistingReviewAsync(Guid bookingId)
        {
            if (await _unitOfWork.Reviews.IsExistsForBookingAsync(bookingId))
                throw new ConflictException("You already submitted a review for this booking.");
        }
    }
}