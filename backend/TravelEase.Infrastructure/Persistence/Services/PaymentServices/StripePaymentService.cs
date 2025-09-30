using Stripe;
using TravelEase.Domain.Common.Interfaces;
using PaymentMethod = TravelEase.Domain.Enums.PaymentMethod;

namespace TravelEase.Infrastructure.Persistence.Services.PaymentServices
{
    public class StripePaymentService : IPaymentService
    {
        public async Task<PaymentIntent> CreatePaymentIntentAsync
            (Guid bookingId, double amount, PaymentMethod method, string currency = "usd")
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = currency,
                Metadata = new Dictionary<string, string>
            {
                { "BookingId", bookingId.ToString() }
            },
                PaymentMethodTypes = MapPaymentMethodToStripe(method)
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);
            return paymentIntent;
        }

        private static List<string> MapPaymentMethodToStripe(PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.CreditCard => new List<string> { "card" },
                PaymentMethod.MobileWallet => new List<string> { "wechat_pay" },
                PaymentMethod.Cash or PaymentMethod.None => throw new NotSupportedException("This payment method is not supported by Stripe."),
                _ => throw new ArgumentOutOfRangeException(nameof(method), $"Unsupported payment method: {method}")
            };
        }
    }
}