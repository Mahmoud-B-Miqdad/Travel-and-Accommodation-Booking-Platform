using MediatR;

namespace TravelEase.Application.BookingManagement.Queries
{
    public record GetBookingInvoiceQuery : IRequest<InvoiceResponse>
    {
        public Guid BookingId { get; set; }
    }
}