using TravelEase.Domain.Enums;

namespace TravelEase.Application.RoomTypeManagement.DTOs.Requests
{
    public record RoomTypeForCreationRequest
    {
        public RoomCategory Category { get; init; }
        public float PricePerNight { get; init; }
        public List<Guid> AmenityIds { get; init; } = new();
    }
}