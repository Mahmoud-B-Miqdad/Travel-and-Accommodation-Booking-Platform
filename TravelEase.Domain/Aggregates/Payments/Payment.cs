using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Enums;

namespace TravelEase.Domain.Aggregates.Payments
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public double Amount { get; set; }

        public string PaymentIntentId { get; set; }
    }
}