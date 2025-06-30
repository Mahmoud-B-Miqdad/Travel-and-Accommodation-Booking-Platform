namespace TravelEase.Application.RoomAmenityManagement.DTOs.Requests
{
    public record RoomAmenityForCreationRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}