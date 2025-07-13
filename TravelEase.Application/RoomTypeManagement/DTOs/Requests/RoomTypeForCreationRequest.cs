using TravelEase.Domain.Enums;

namespace TravelEase.Application.RoomTypeManagement.DTOs.Requests
{
    public record RoomTypeForCreationRequest
    {
        public RoomCategory Category { get; set; }
        public float PricePerNight { get; set; }

        public List<Guid> AmenityIds { get; set; } = new();
    }
}