using MediatR;

namespace TravelEase.Application.BookingManagement.Queries
{
    public record GetInvoiceByBookingIdQuery : IRequest<InvoiceResponse>
    {
        public Guid HotelId { get; set; }
        public Guid BookingId { get; set; }
        public string GuestEmail { get; set; }
    }
}