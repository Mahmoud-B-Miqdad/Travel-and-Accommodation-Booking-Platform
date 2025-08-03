using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;

namespace TravelEase.Application.RoomTypeManagement.DTOs.Responses
{
    public record RoomTypeResponse
    {
        public Guid Id { get; init; }
        public Guid HotelId { get; init; }
        public string Category { get; init; }
        public float PricePerNight { get; init; }
        public List<RoomAmenityResponse> Amenities { get; init; }
    }
}