using Microsoft.EntityFrameworkCore;
using System;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.Repositories;

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
    }
}