using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.CommonRepositories;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.CityPersistence
{
    public class CityRepository : GenericCrudRepository<City>, ICityRepository
    {
        private readonly TravelEaseDbContext _context;

        public CityRepository(TravelEaseDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedList<City>> GetAllAsync(bool includeHotels, string? searchQuery, int pageNumber, int pageSize)
        {
            var query = _context.Cities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                query = query.Where(city =>
                    city.Name.Contains(searchQuery) ||
                    city.CountryName.Contains(searchQuery));
            }

            if (includeHotels)
            {
                query = query.Include(city => city.Hotels);
            }

            return await PaginationHelper.PaginateAsync(query.AsNoTracking(), pageNumber, pageSize);
        }

        public async Task<bool> IsExistsAsync(string cityName)
        {
            return await _context
                .Cities
                .AnyAsync
                (city => city.Name.Equals(cityName));
        }

        public async Task<bool> IsExistsAsync(Guid cityId)
        {
            return await _context
                .Cities
                .AnyAsync
                (city => city.Name.Equals(cityId));
        }
    }
}