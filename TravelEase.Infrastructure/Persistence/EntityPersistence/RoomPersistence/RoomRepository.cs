using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.CommonRepositories;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.RoomPersistence
{
    public class RoomRepository : GenericCrudRepository<Room>, IRoomRepository
    {
        private readonly TravelEaseDbContext _context;

        public RoomRepository(TravelEaseDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Room>> GetAllByHotelIdAsync(Guid hotelId, string? searchQuery, int pageNumber, int pageSize)
        {
            var query = from room in _context.Rooms
                        join roomType in _context.RoomTypes on room.RoomTypeId equals roomType.Id
                        where roomType.HotelId == hotelId
                        select room;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                query = query.Where(room => room.View.Contains(searchQuery));
            }

            return await PaginationHelper.PaginateAsync(query.AsNoTracking(), pageNumber, pageSize);
        }

        public async Task<Room?> GetRoomWithTypeAndDiscountsAsync(Guid roomId)
        {
            return await _context.Rooms
                .Include(r => r.RoomType)
                    .ThenInclude(rt => rt.Discounts)
                .SingleOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<bool> IsRoomBelongsToHotelAsync(Guid roomId, Guid hotelId)
        {
            return await _context.Rooms
                .Where(r => r.Id == roomId && r.RoomType.HotelId == hotelId)
                .AnyAsync();
        }

        public async Task<List<Room>> GetHotelAvailableRoomsAsync(
            Guid hotelId, DateTime checkInDate, DateTime checkOutDate)
        {
            var rooms = await (from roomType in _context.RoomTypes
                               where roomType.HotelId == hotelId
                               join room in _context.Rooms
                                   on roomType.Id equals room.RoomTypeId
                               select room).ToListAsync();

            var roomIds = rooms.Select(r => r.Id).ToList();

            var conflictingRoomIds = await _context.Bookings
                .Where(b =>
                    roomIds.Contains(b.RoomId) &&
                    b.CheckInDate < checkOutDate &&
                    b.CheckOutDate > checkInDate)
                .Select(b => b.RoomId)
                .Distinct()
                .ToListAsync();

            return rooms.Where(room => !conflictingRoomIds.Contains(room.Id)).ToList();
        }
    }
}