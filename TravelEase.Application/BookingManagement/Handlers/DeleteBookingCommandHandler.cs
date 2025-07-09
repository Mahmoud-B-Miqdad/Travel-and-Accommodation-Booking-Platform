using MediatR;
using TravelEase.Application.BookingManagement.Commands;
using TravelEase.Domain.Aggregates.Bookings;
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
            await EnsureHotelExists(request.HotelId);
            var booking = await GetExistingBooking(request.BookingId);
            await EnsureBookingBelongsToHotel(request.BookingId, request.HotelId);
            await EnsureUserCanAccessBooking(request.BookingId, request.GuestEmail);
            EnsureBookingCanBeCancelled(booking);

            _unitOfWork.Bookings.Remove(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureHotelExists(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }

        private async Task<Booking> GetExistingBooking(Guid bookingId)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
            if (booking == null)
                throw new NotFoundException("Booking doesn't exist to delete.");
            return booking;
        }

        private async Task EnsureBookingBelongsToHotel(Guid bookingId, Guid hotelId)
        {
            var belongs = await _hotelOwnershipValidator.IsBookingBelongsToHotelAsync(bookingId, hotelId);
            if (!belongs)
                throw new NotFoundException($"Booking with ID {bookingId} does not belong to hotel {hotelId}.");
        }

        private async Task EnsureUserCanAccessBooking(Guid bookingId, string guestEmail)
        {
            var isAccessible = await _unitOfWork.Bookings.IsBookingAccessibleToUserAsync(bookingId, guestEmail);
            if (!isAccessible)
                throw new UnauthorizedAccessException("You are not authorized to cancel this booking.");
        }

        private void EnsureBookingCanBeCancelled(Booking booking)
        {
            if (booking.CheckInDate <= DateTime.UtcNow.Date)
                throw new BookingCheckInDatePassedException("Cannot cancel booking after check-in date.");
        }
    }
}