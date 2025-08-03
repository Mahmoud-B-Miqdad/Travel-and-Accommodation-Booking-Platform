namespace TravelEase.Application.RoomManagement.DTOs.Requests
{
    public record RoomForUpdateRequest
    {
        public Guid RoomTypeId { get; init; }
        public int AdultsCapacity { get; init; }
        public int ChildrenCapacity { get; init; }
        public string View { get; init; }
    }
}