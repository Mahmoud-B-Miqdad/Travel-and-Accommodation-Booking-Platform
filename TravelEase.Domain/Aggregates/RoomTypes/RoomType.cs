using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Enums;

namespace TravelEase.Domain.Aggregates.RoomTypes
{
    public class RoomType
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public RoomCategory Category { get; set; }
        public float PricePerNight { get; set; }
        public List<RoomAmenity> Amenities { get; set; }
        public List<Discount> Discounts { get; set; }
    }
}