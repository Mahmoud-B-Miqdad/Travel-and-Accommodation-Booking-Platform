using AutoMapper;
using MediatR;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Application.BookingManagement.Queries;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.BookingManagement.Handlers
{
    public class GetBookingByIdAndHotelIdQueryHandler
    : IRequestHandler<GetBookingByIdAndHotelIdQuery, BookingResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHotelOwnershipValidator _hotelOwnershipValidator;
        private readonly IMapper _mapper;

        public GetBookingByIdAndHotelIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHotelOwnershipValidator hotelOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelOwnershipValidator = hotelOwnershipValidator;
        }

        public async Task<BookingResponse?> Handle(GetBookingByIdAndHotelIdQuery request, CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);
            await EnsureBookingBelongsToHotelAsync(request.BookingId, request.HotelId);

            var booking = await GetBookingAsync(request.BookingId);
            return _mapper.Map<BookingResponse>(booking);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException($"Hotel with ID {hotelId} doesn't exist.");
        }

        private async Task EnsureBookingBelongsToHotelAsync(Guid bookingId, Guid hotelId)
        {
            var belongs = await _hotelOwnershipValidator.IsBookingBelongsToHotelAsync(bookingId, hotelId);
            if (!belongs)
                throw new NotFoundException($"Booking with ID {bookingId} does not belong to hotel {hotelId}.");
        }

        private async Task<Booking> GetBookingAsync(Guid bookingId)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
            if (booking == null)
                throw new NotFoundException($"Booking with ID {bookingId} was not found.");

            return booking;
        }
    }
}