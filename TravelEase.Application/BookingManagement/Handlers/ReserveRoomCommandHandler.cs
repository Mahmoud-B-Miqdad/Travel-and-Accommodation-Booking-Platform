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
        private readonly IHotelOwnershipValidator _hotelOwnershipValidator;
        private readonly IMapper _mapper;

        public ReserveRoomCommandHandler(
            IUnitOfWork unitOfWork,
            IPricingService pricingService,
            IMapper mapper,
            IHotelOwnershipValidator hotelOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _pricingService = pricingService;
            _mapper = mapper;
            _hotelOwnershipValidator = hotelOwnershipValidator;
        }

        public async Task<BookingResponse?> Handle(ReserveRoomCommand request, CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);
            await EnsureRoomExistsAndBelongsToHotelAsync(request.RoomId, request.HotelId);
            await EnsureNoConflictingBookingAsync(request.RoomId, request.CheckInDate, request.CheckOutDate);

            var booking = await CreateBookingAsync(request);
            var addedBooking = await _unitOfWork.Bookings.AddAsync(booking);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<BookingResponse>(addedBooking);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }

        private async Task EnsureRoomExistsAndBelongsToHotelAsync(Guid roomId, Guid hotelId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null)
                throw new NotFoundException($"Room with ID {roomId} doesn't exist.");

            var belongs = await _hotelOwnershipValidator.IsRoomBelongsToHotelAsync(roomId, hotelId);
            if (!belongs)
                throw new NotFoundException($"Room with ID {roomId} does not belong to hotel {hotelId}.");
        }

        private async Task EnsureNoConflictingBookingAsync(Guid roomId, DateTime checkIn, DateTime checkOut)
        {
            var isConflict = await _unitOfWork.Bookings.ExistsConflictingBookingAsync(roomId, checkIn, checkOut);
            if (isConflict)
            {
                var msg = $"Can't book a date between {checkIn:yyyy-MM-dd} and {checkOut:yyyy-MM-dd}";
                throw new ConflictException(msg);
            }
        }

        private async Task<Booking> CreateBookingAsync(ReserveRoomCommand request)
        {
            var booking = _mapper.Map<Booking>(request);

            var user = await _unitOfWork.Users.GetByEmailAsync(request.GuestEmail);
            booking.UserId = user.Id;

            var totalPrice = await _pricingService
                .CalculateTotalPriceAsync(request.RoomId, request.CheckInDate, request.CheckOutDate);

            booking.Price = totalPrice;
            booking.BookingDate = DateTime.UtcNow;

            return booking;
        }
    }
}