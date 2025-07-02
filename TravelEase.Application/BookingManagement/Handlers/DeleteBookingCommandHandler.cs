using MediatR;
using TravelEase.Application.BookingManagement.Commands;
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

            _unitOfWork.Bookings.Remove(existingBooking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}