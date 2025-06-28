using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.CommonRepositories;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.RoomAmenityPersistence
{
    public class RoomAmenityRepository : GenericCrudRepository<RoomAmenity>, IRoomAmenityRepository
    {
        private readonly TravelEaseDbContext _context;

        public RoomAmenityRepository(TravelEaseDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedList<RoomAmenity>> GetAllAsync(string? searchQuery, int pageNumber, int pageSize)
        {
            var query = _context.RoomAmenities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                query = query.Where(r =>
                    r.Name.Contains(searchQuery) ||
                    r.Description.Contains(searchQuery));
            }

            return await PaginationHelper.PaginateAsync(query.AsNoTracking(), pageNumber, pageSize);
        }
    }
}