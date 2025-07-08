using AutoMapper;
using MediatR;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Application.BookingManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.BookingManagement.Handlers
{
    public class GetBookingByIdAndHotelIdQueryHandler : IRequestHandler<GetBookingByIdAndHotelIdQuery, BookingResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHotelOwnershipValidator _hotelOwnershipValidator;

        private readonly IMapper _mapper;
        public GetBookingByIdAndHotelIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHotelOwnershipValidator hotelOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelOwnershipValidator = hotelOwnershipValidator;
        }

        public async Task<BookingResponse?> Handle(GetBookingByIdAndHotelIdQuery request, CancellationToken cancellationToken)
        {
            var hotelExists = await _unitOfWork.Hotels.ExistsAsync(request.HotelId);
            if (!hotelExists)
                throw new NotFoundException($"Hotel with ID {request.HotelId} doesn't exist.");

            var belongsToHotel = await _hotelOwnershipValidator
                .IsBookingBelongsToHotelAsync(request.BookingId, request.HotelId);
            if (!belongsToHotel)
                throw new NotFoundException($"Booking with ID {request.BookingId} does not belong to hotel {request.HotelId}.");

            var booking = await _unitOfWork.Bookings.GetByIdAsync(request.BookingId);
            if (booking == null)
                throw new NotFoundException($"Booking with Id {request.BookingId} was not found.");

            return _mapper.Map<BookingResponse>(booking);
        }
    }
}