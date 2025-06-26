using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.Repositories;

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
    }
}