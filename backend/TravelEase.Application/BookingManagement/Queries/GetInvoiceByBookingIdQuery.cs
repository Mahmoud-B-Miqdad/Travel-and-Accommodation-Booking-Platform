using MediatR;

namespace TravelEase.Application.BookingManagement.Queries
{
    public record GetInvoiceByBookingIdQuery : IRequest<InvoiceResponse>
    {
        public Guid HotelId { get; init; }
        public Guid BookingId { get; init; }
        public string GuestEmail { get; init; }
        public string GuestName { get; init; }
        public bool GeneratePdf { get; init; } = false;
        public bool SendByEmail { get; init; } = false;
    }
}