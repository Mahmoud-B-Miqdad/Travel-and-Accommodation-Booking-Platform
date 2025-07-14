using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.HotelModels;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Domain.Aggregates.Hotels
{
    public interface IHotelRepository : ICrudRepository<Hotel>
    {
        Task<PaginatedList<Hotel>> GetAllAsync
            (string? searchQuery, int pageNumber, int pageSize);
        Task<PaginatedList<HotelSearchResult>> HotelSearchAsync
            (HotelSearchParameters searchParams);
    }
}