using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Domain.Aggregates.Cities
{
    public interface ICityRepository : ICrudRepository<City>
    {
        Task<PaginatedList<City>> GetAllAsync
            (bool includeHotels, string? searchQuery, int pageNumber, int pageSize);
        Task<bool> ExistsAsync(string cityName);
        Task<bool> ExistsAsync(Guid cityId);
        Task<List<City>> GetTrendingCitiesAsync(int count);
    }
}