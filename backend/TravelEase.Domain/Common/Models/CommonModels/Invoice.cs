namespace TravelEase.Domain.Common.Models.CommonModels
{
    public record Invoice
    {
        public Guid BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public double Price { get; set; }
        public string HotelName { get; set; }
        public string OwnerName { get; set; }
    }
}