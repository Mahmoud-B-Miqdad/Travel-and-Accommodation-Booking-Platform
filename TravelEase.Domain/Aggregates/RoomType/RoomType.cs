using TravelEase.Domain.Enums;

namespace TravelEase.Domain.Aggregates.RoomType
{
    public class RoomType
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public RoomCategory Category { get; set; }
        public float PricePerNight { get; set; }
    }
}