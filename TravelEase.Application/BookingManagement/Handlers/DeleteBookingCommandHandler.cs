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
        public DeleteBookingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            var existingBooking = await _unitOfWork.Bookings.GetByIdAsync(request.Id);
            if (existingBooking == null)
                throw new NotFoundException("Booking Doesn't Exists To Delete");

            var isAccessible = await _unitOfWork.Bookings.IsBookingAccessibleToUserAsync(
            request.Id, request.GuestEmail);

            if (!isAccessible)
                throw new UnauthorizedAccessException("You are not authorized to cancel this booking");

            if (existingBooking.CheckInDate <= DateTime.UtcNow.Date)
                throw new BookingCheckInDatePassedException("Cannot cancel booking after check-in date");

            _unitOfWork.Bookings.Remove(existingBooking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}