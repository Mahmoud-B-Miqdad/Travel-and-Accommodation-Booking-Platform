using TravelEase.Domain.Enums;

namespace TravelEase.Application.PaymentManagement.DTOs.Requests
{
    public record CreatePaymentIntentRequest
    {
        public Guid BookingId { get; init; }
        public double Amount { get; init; }
        public PaymentMethod Method { get; init; }
    }
}