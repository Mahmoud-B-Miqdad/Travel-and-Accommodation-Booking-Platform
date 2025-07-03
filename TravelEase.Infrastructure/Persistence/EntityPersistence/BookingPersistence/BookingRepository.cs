using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.CommonRepositories;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.BookingPersistence
{
    public class BookingRepository : GenericCrudRepository<Booking>, IBookingRepository
    {
        private readonly TravelEaseDbContext _context;

        public BookingRepository(TravelEaseDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Booking>> GetAllByHotelIdAsync(Guid hotelId, int pageNumber, int pageSize)
        {
            var query = from booking in _context.Bookings
                        join room in _context.Rooms on booking.RoomId equals room.Id
                        join roomType in _context.RoomTypes on room.RoomTypeId equals roomType.Id
                        join hotel in _context.Hotels on roomType.HotelId equals hotel.Id
                        where roomType.HotelId == hotelId
                        select booking;

            return await PaginationHelper.PaginateAsync(query.AsNoTracking(), pageNumber, pageSize);
        }

        public async Task<bool> IsBookingAccessibleToUserAsync(Guid bookingId, string guestEmail)
        {
            var guestId = await _context.Users
                .Where(u => u.Email == guestEmail)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();

            if (guestId == Guid.Empty)
                return false;

            var booking = await _context.Bookings
                .Where(b => b.Id == bookingId)
                .Select(b => b.UserId)
                .SingleOrDefaultAsync();

            if (booking == Guid.Empty)
                return false;

            return guestId == booking;
        }

        public async Task<bool> ExistsConflictingBookingAsync(Guid roomId, DateTime checkInDate,
            DateTime checkOutDate)
        {
            return await _context.Bookings.AnyAsync(b =>
                b.RoomId == roomId &&
                b.CheckInDate < checkOutDate &&
                b.CheckOutDate > checkInDate
            );
        }
    }
}