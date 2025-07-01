namespace TravelEase.Application.RoomAmenityManagement.DTOs.Requests
{
    public record RoomAmenityForUpdateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}