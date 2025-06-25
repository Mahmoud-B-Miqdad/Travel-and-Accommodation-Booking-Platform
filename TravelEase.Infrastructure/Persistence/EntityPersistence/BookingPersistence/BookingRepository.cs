using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Helpers;
using TravelEase.Infrastructure.Persistence.Repositories;

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
    }
}