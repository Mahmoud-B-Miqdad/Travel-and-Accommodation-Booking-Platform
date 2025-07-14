using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Enums;

namespace TravelEase.Domain.Common.Models.HotelModels
{
    public record HotelSearchRawResult
    {
        public Guid CityId { get; set; }
        public string CityName { get; set; }
        public Guid HotelId { get; set; }
        public Guid RoomId { get; set; }
        public float RoomPricePerNight { get; set; }
        public RoomCategory RoomTypeCategory { get; set; }
        public string HotelName { get; set; }
        public string HotelStreetAddress { get; set; }
        public int HotelFloorsNumber { get; set; }
        public float HotelRating { get; set; }
        public List<Discount> RoomDiscounts { get; set; } = new();
    }
}