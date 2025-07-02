using AutoMapper;
using MediatR;
using TravelEase.Application.BookingManagement.Commands;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.BookingManagement.Handlers
{
    public class ReserveRoomCommandHandler : IRequestHandler<ReserveRoomCommand, BookingResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReserveRoomCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookingResponse?> Handle(ReserveRoomCommand request, CancellationToken cancellationToken)
        {
            var roomExists = await _unitOfWork.Rooms.ExistsAsync(request.RoomId);
            if (!roomExists)
                throw new NotFoundException($"Room with ID {request.RoomId} doesn't exist.");

            var isConflict = await _unitOfWork.Bookings.ExistsConflictingBookingAsync(
                request.RoomId, request.CheckInDate, request.CheckOutDate);
            if (isConflict)
            {
                var msg = $"Can't book a date between {request.CheckInDate:yyyy-MM-dd} and {request.CheckOutDate:yyyy-MM-dd}";
                throw new ConflictException(msg);
            }

            var booking = _mapper.Map<Booking>(request);
            var User = await _unitOfWork.Users.GetByEmailAsync(request.GuestEmail);
            booking.UserId = User.Id;
            booking.Price = await _unitOfWork.Rooms.GetPriceForRoomWithDiscount(request.RoomId);
            booking.BookingDate = DateTime.UtcNow;

            var addedBooking = await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BookingResponse>(addedBooking);
        }
    }
}