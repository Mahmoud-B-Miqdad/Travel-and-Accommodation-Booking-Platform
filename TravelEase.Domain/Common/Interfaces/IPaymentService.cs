using TravelEase.Domain.Enums;

namespace TravelEase.Domain.Common.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentIntentAsync
            (Guid bookingId, double amount, PaymentMethod method,  string currency = "usd");
    }
}