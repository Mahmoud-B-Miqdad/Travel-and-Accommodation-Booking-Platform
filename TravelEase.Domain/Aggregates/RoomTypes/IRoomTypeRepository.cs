using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Enums;

namespace TravelEase.Domain.Aggregates.RoomTypes
{
    public interface IRoomTypeRepository : ICrudRepository<RoomType>
    {
        Task<PaginatedList<RoomType>> GetAllByHotelIdAsync
            (Guid hotelId, bool includeAmenities,int pageNumber, int pageSize);
        Task<bool> ExistsByHotelAndCategoryAsync(Guid hotelId, RoomCategory category);
        Task<bool> HasActiveBookingsAsync(Guid roomTypeId);
        Task<bool> HasRoomsAsync(Guid roomTypeId);
    }
}