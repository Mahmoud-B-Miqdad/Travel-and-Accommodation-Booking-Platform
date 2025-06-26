using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.Repositories;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.HotelPersistence
{
    public class HotelRepository : GenericCrudRepository<Hotel>, IHotelRepository
    {
        private readonly TravelEaseDbContext _context;

        public HotelRepository(TravelEaseDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Hotel>> GetAllAsync(string? searchQuery, int pageNumber, int pageSize)
        {
            var query = _context.Hotels.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                query = query.Where(hotel =>
                    hotel.Name.Contains(searchQuery) ||
                    hotel.Description.Contains(searchQuery) ||
                    hotel.StreetAddress.Contains(searchQuery));
            }

            return await PaginationHelper.PaginateAsync(query.AsNoTracking(), pageNumber, pageSize);
        }
    }
}