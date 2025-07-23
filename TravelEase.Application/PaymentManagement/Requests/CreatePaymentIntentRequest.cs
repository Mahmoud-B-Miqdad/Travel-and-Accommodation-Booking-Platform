using TravelEase.Domain.Enums;

namespace TravelEase.Application.PaymentManagement.Requests
{
    public record CreatePaymentIntentRequest
    {
        public Guid BookingId { get; set; }
        public double Amount { get; set; }
        public PaymentMethod Method { get; set; }
    }
}