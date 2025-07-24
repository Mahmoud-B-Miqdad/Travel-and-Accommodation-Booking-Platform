using Stripe;
using PaymentMethod = TravelEase.Domain.Enums.PaymentMethod;

namespace TravelEase.Domain.Common.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentIntent> CreatePaymentIntentAsync
            (Guid bookingId, double amount, PaymentMethod method,  string currency = "usd");
    }
}