using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Domain.Aggregates.RoomAmenities
{
    public interface IRoomAmenityRepository : ICrudRepository<RoomAmenity>
    {
        Task<PaginatedList<RoomAmenity>> GetAllAsync
            (string? searchQuery, int pageNumber, int pageSize);
        Task<List<RoomAmenity>> GetByIdsAsync(List<Guid> amenityIds);
    }
}