using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Infrastructure.Persistence.Context;

namespace TravelEase.Infrastructure.Persistence.Services
{
    public class OwnershipValidator : IOwnershipValidator
    {
        private readonly TravelEaseDbContext _context;

        public OwnershipValidator(TravelEaseDbContext context)
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

        public async Task<bool> IsRoomTypeBelongsToHotelAsync(Guid roomTypeId, Guid hotelId)
        {
            return await _context.RoomTypes
                .AnyAsync(rt => rt.Id == roomTypeId && rt.HotelId == hotelId);
        }

        public async Task<bool> IsDiscountBelongsToRoomTypeAsync(Guid discountId, Guid roomTypeId)
        {
            return await _context.Discounts
                .AnyAsync(d => d.Id == discountId && d.RoomTypeId == roomTypeId);
        }
    }
}