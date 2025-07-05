using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Domain.Aggregates.RoomTypes
{
    public interface IRoomTypeRepository : ICrudRepository<RoomType>
    {
        Task<PaginatedList<RoomType>> GetAllByHotelIdAsync(Guid hotelId, bool includeAmenities,
            int pageNumber, int pageSize);
        Task<bool> CheckRoomTypeExistenceForHotelAsync(Guid hotelId, Guid roomTypeId);
    }
}