namespace TravelEase.Application.RoomTypeManagement.DTOs.Responses
{
    public record RoomTypeResponse
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public string Category { get; set; }
        public float PricePerNight { get; set; }
    }
}