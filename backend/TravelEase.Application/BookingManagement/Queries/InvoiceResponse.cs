using System.Text.Json.Serialization;

namespace TravelEase.Application.BookingManagement.Queries
{
    public record InvoiceResponse
    {
        public Guid BookingId { get; init; }
        public DateTime BookingDate { get; init; }
        public double Price { get; init; }
        public string HotelName { get; init; }
        public string OwnerName { get; init; }
    }
}