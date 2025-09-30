using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Domain.Aggregates.Payments
{
    public interface IPaymentRepository : ICrudRepository<Payment>
    {
        Task<List<Payment>> GetPaymentsByUserIdAsync(Guid userId);
        Task<Payment?> GetByBookingIdAsync(Guid bookingId);
        Task<Payment?> GetByPaymentIntentIdAsync(string paymentIntentId);
    }
}