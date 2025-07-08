using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Infrastructure.Persistence.Context;

namespace TravelEase.Infrastructure.Persistence.Services
{
    public class HotelOwnershipValidator : IHotelOwnershipValidator
    {
        private readonly TravelEaseDbContext _context;

        public HotelOwnershipValidator(TravelEaseDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsRoomBelongsToHotelAsync(Guid roomId, Guid hotelId)
        {
            return await _context.Rooms
                .Where(r => r.Id == roomId && r.RoomType.HotelId == hotelId)
                .AnyAsync();
        }

        public async Task<bool> IsBookingBelongsToHotelAsync(Guid bookingId, Guid hotelId)
        {
            return await _context.Bookings
                .Where(b => b.Id == bookingId && b.Room.RoomType.HotelId == hotelId)
                .AnyAsync();
        }

        public async Task<bool> IsReviewBelongsToHotelAsync(Guid reviewId, Guid hotelId)
        {
            return await _context.Reviews
                .Where(r => r.Id == reviewId && r.Booking.Room.RoomType.HotelId == hotelId)
                .AnyAsync();
        }
    }
}