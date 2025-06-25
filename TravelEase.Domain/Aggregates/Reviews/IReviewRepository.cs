using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Domain.Aggregates.Reviews
{
    public interface IReviewRepository : ICrudRepository<Review>
    {
        Task<PaginatedList<Review>> GetAllByHotelIdAsync(Guid hotelId, string? searchQuery, int pageNumber, int pageSize);
    }
}