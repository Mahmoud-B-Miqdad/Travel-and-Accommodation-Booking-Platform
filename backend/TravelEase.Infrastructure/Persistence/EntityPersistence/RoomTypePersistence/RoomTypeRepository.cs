using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.CommonRepositories;
using TravelEase.Domain.Enums;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.RoomTypePersistence
{
    public class RoomTypeRepository : GenericCrudRepository<RoomType>, IRoomTypeRepository
    {
        private readonly TravelEaseDbContext _context;

        public RoomTypeRepository(TravelEaseDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedList<RoomType>> GetAllByHotelIdAsync(Guid hotelId, bool includeAmenities, int pageNumber, int pageSize)
        {
            var query = _context.RoomTypes
                .Where(rt => rt.HotelId == hotelId)
                .AsQueryable();

            if (includeAmenities)
            {
                query = query.Include(rt => rt.Amenities);
            }

            return await PaginationHelper.PaginateAsync(query.AsNoTracking(), pageNumber, pageSize);
        }

        public async Task<bool> ExistsByHotelAndCategoryAsync(Guid hotelId, RoomCategory category)
        {
            return await _context.RoomTypes
                .AnyAsync(rt => rt.HotelId == hotelId && rt.Category == category);
        }

        public async Task<bool> HasRoomsAsync(Guid roomTypeId)
        {
            return await _context.Rooms
                .AsNoTracking()
                .AnyAsync(r => r.RoomTypeId == roomTypeId);
        }
    }
}