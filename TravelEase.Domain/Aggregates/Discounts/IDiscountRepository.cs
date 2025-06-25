using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Domain.Aggregates.Discounts
{
    public interface IDiscountRepository : ICrudRepository<Discount>
    {
        Task<PaginatedList<Discount>> GetAllByRoomTypeIdAsync(Guid roomTypeId, int pageNumber, int pageSize);
    }

}