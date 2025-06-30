namespace TravelEase.Application.RoomAmenityManagement.DTOs.Responses
{
    public record RoomAmenityResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}