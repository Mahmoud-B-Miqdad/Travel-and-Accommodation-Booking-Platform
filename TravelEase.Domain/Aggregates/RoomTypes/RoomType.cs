using TravelEase.Domain.Enums;

namespace TravelEase.Domain.Aggregates.RoomTypes
{
    public class RoomType
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public RoomCategory Category { get; set; }
        public float PricePerNight { get; set; }
    }
}