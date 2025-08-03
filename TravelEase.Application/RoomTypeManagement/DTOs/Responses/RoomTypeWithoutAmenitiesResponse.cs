namespace TravelEase.Application.RoomTypeManagement.DTOs.Responses
{
    public record RoomTypeWithoutAmenitiesResponse
    {
        public Guid Id { get; init; }
        public Guid HotelId { get; init; }
        public string Category { get; init; }
        public float PricePerNight { get; init; }
    }
}