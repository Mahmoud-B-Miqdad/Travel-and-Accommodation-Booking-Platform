namespace TravelEase.Application.RoomAmenityManagement.DTOs.Requests
{
    public record RoomAmenityForCreationRequest
    {
        public string Name { get; init; }
        public string Description { get; init; }
    }
}