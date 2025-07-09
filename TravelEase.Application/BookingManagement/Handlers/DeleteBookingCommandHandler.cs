using MediatR;
using TravelEase.Application.BookingManagement.Commands;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.BookingManagement.Handlers
{
    public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHotelOwnershipValidator _hotelOwnershipValidator;

        public DeleteBookingCommandHandler(IUnitOfWork unitOfWork, IHotelOwnershipValidator hotelOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _hotelOwnershipValidator = hotelOwnershipValidator;
        }

        public async Task Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            var hotelExists = await _unitOfWork.Hotels.ExistsAsync(request.HotelId);
            if (!hotelExists)
                throw new NotFoundException("Hotel doesn't exists.");

            var existingBooking = await _unitOfWork.Bookings.GetByIdAsync(request.BookingId);
            if (existingBooking == null)
                throw new NotFoundException("Booking Doesn't Exists To Delete");

            var belongsToHotel = await _hotelOwnershipValidator
                .IsBookingBelongsToHotelAsync(request.BookingId, request.HotelId);

            if (!belongsToHotel)
                throw new NotFoundException($"Booking with ID {request.BookingId} does not belong to hotel {request.HotelId}.");


            var isAccessible = await _unitOfWork.Bookings.IsBookingAccessibleToUserAsync(
            request.BookingId, request.GuestEmail);

            if (!isAccessible)
                throw new UnauthorizedAccessException("You are not authorized to cancel this booking");

            if (existingBooking.CheckInDate <= DateTime.UtcNow.Date)
                throw new BookingCheckInDatePassedException("Cannot cancel booking after check-in date");

            _unitOfWork.Bookings.Remove(existingBooking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}