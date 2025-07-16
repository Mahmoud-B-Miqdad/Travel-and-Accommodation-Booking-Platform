using AutoMapper;
using MediatR;
using TravelEase.Application.BookingManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.CommonModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.BookingManagement.Handlers
{
    public class GetInvoiceByBookingIdQueryHandler 
        : IRequestHandler<GetInvoiceByBookingIdQuery, InvoiceResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInvoiceByBookingIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InvoiceResponse> Handle
            (GetInvoiceByBookingIdQuery request, CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);
            await EnsureBookingExists(request.BookingId);

            var invoice = await _unitOfWork.Bookings.GetInvoiceByBookingIdAsync(request.BookingId);
            ThrowIfInvoiceNotFound(invoice);

            await EnsureUserCanAccessBooking(request.BookingId, request.GuestEmail);

            return _mapper.Map<InvoiceResponse>(invoice);
        }
        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException($"Hotel with ID {hotelId} doesn't exist.");
        }

        private async Task EnsureBookingExists(Guid bookingId)
        {
            if (!await _unitOfWork.Bookings.ExistsAsync(bookingId))
                throw new NotFoundException("Booking doesn't exist.");
        }

        private static void ThrowIfInvoiceNotFound(Invoice? invoice)
        {
            if (invoice is null)
                throw new NotFoundException($"Invoice not found.");
        }

        private async Task EnsureUserCanAccessBooking(Guid bookingId, string guestEmail)
        {
            var isAccessible = await _unitOfWork.Bookings.IsBookingAccessibleToUserAsync(bookingId, guestEmail);
            if (!isAccessible)
                throw new UnauthorizedAccessException("You are not authorized to cancel this booking.");
        }
    }
}