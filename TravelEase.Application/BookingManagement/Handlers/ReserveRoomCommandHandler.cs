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
        private readonly IPricingService _pricingService;
        private readonly IMapper _mapper;

        public ReserveRoomCommandHandler(IUnitOfWork unitOfWork, IPricingService pricingService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _pricingService = pricingService;
            _mapper = mapper;
        }

        public async Task<BookingResponse?> Handle(ReserveRoomCommand request, CancellationToken cancellationToken)
        {
            var hotelExists = await _unitOfWork.Hotels.ExistsAsync(request.HotelId);
            if (!hotelExists)
                throw new NotFoundException("Hotel doesn't exists.");

            var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId);
            if (room == null)
                throw new NotFoundException($"Room with ID {request.RoomId} doesn't exist.");

            var isRoomInHotel = await _unitOfWork.RoomTypes
                .CheckRoomTypeExistenceForHotelAsync(request.HotelId, room.RoomTypeId);

            if (!isRoomInHotel)
                throw new NotFoundException("Room does not belong to the specified hotel.");

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

            var totalPrice = await _pricingService.CalculateTotalPriceAsync(request.RoomId, request.CheckInDate, request.CheckOutDate);
            booking.Price = totalPrice;

            booking.BookingDate = DateTime.UtcNow;

            var addedBooking = await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BookingResponse>(addedBooking);
        }
    }
}