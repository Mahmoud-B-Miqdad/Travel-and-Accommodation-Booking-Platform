namespace TravelEase.Application.RoomAmenity.DTOs.Responses
{
    public record RoomAmenityResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}