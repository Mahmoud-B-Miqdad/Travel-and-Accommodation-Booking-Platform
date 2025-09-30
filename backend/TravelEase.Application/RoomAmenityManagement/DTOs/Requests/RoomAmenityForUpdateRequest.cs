namespace TravelEase.Application.RoomAmenityManagement.DTOs.Requests
{
    public record RoomAmenityForUpdateRequest
    {
        public string Name { get; init; }
        public string Description { get; init; }
    }
}