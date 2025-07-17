using System.Text.Json.Serialization;

namespace TravelEase.Application.BookingManagement.Queries
{
    public record InvoiceResponse
    {
        public Guid Id { get; set; }
        public DateTime BookingDate { get; set; }
        public double Price { get; set; }
        public string HotelName { get; set; }
        public string OwnerName { get; set; }
        [JsonIgnore]
        public byte[]? PdfBytes { get; set; }
    }
}