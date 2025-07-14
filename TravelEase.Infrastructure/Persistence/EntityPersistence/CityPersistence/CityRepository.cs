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
        public async Task<List<City>> GetTrendingCitiesAsync(int count)
        {
            var trendingCitiesIdsWithCount =
                await (from booking in _context.Bookings
                       join room in _context.Rooms on booking.RoomId equals room.Id
                       join roomType in _context.RoomTypes on room.RoomTypeId equals roomType.Id
                       join hotel in _context.Hotels on roomType.HotelId equals hotel.Id
                       join city in _context.Cities on hotel.CityId equals city.Id
                       group city by city.Id into groupedCities
                       orderby groupedCities.Count() descending
                       select new
                       {
                           CityId = groupedCities.Key,
                           Popularity = groupedCities.Count()
                       })
                .Take(count)
                .ToListAsync();

            var cityIds = trendingCitiesIdsWithCount.Select(x => x.CityId).ToList();

            var cities = await _context.Cities
                .Where(c => cityIds.Contains(c.Id))
                .AsNoTracking()
                .ToListAsync();

            var cityDict = cities.ToDictionary(c => c.Id);

            var orderedCities = cityIds
                .Where(id => cityDict.ContainsKey(id))
                .Select(id => cityDict[id])
                .ToList();

            return orderedCities;
        }
    }
}