using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Domain.Aggregates.Rooms
{
    public interface IRoomRepository : ICrudRepository<Room>
    {
        Task<PaginatedList<Room>> GetAllByHotelIdAsync(Guid hotelId, string? searchQuery,
            int pageNumber, int pageSize);
        Task<Room?> GetRoomWithTypeAndDiscountsAsync(Guid roomId);
        Task<List<Room>> GetHotelAvailableRoomsAsync(
            Guid hotelId, DateTime checkInDate, DateTime checkOutDate);
        public IQueryable<Room> GetAvailableRoomsWithCapacity
            (int adults, int children, DateTime checkInDate, DateTime checkOutDate);
    }
}