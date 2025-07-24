using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Payments;
using TravelEase.Infrastructure.Persistence.CommonRepositories;
using TravelEase.Infrastructure.Persistence.Context;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.PaymentPersistence
{
    public class PaymentRepository : GenericCrudRepository<Payment>, IPaymentRepository
    {
        private readonly TravelEaseDbContext _context;

        public PaymentRepository(TravelEaseDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Payment>> GetPaymentsByUserIdAsync(Guid userId)
        {
            return await _context.Payments
                .Where(p => p.Booking.UserId == userId)
                .ToListAsync();
        }

        public async Task<Payment?> GetByBookingIdAsync(Guid bookingId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == bookingId);
        }
    }
}