using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.Repositories;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.ReviewPersistence
{
    public class ReviewRepository : GenericCrudRepository<Review>, IReviewRepository
    {
        private readonly TravelEaseDbContext _context;

        public ReviewRepository(TravelEaseDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Review>> GetAllByHotelIdAsync(Guid hotelId, string? searchQuery, int pageNumber, int pageSize)
        {
            var query = from booking in _context.Bookings
                        join room in _context.Rooms on booking.RoomId equals room.Id
                        join roomType in _context.RoomTypes on room.RoomTypeId equals roomType.Id
                        join hotel in _context.Hotels on roomType.HotelId equals hotel.Id
                        join review in _context.Reviews on booking.Id equals review.BookingId
                        where roomType.HotelId == hotelId
                        select review;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                query = query.Where(review => review.Comment.Contains(searchQuery));
            }

            return await PaginationHelper.PaginateAsync(query.AsNoTracking(), pageNumber, pageSize);
        }
    }
}