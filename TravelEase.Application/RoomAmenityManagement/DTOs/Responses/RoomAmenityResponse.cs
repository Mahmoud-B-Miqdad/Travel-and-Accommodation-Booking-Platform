namespace TravelEase.Application.RoomAmenityManagement.DTOs.Responses
{
    public record RoomAmenityResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
    }
}