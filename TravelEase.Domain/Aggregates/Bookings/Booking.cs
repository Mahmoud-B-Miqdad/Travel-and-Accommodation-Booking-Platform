namespace TravelEase.Domain.Aggregates.Bookings
{
    public class Booking
    {
        public Guid Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime BookingDate { get; set; }
        public double Price { get; set; }
    }
}