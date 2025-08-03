namespace TravelEase.Application.RoomManagement.DTOs.Responses
{
    public record RoomResponse
    {
        public Guid Id { get; init; }
        public Guid RoomTypeId { get; init; }
        public int AdultsCapacity { get; init; }
        public int ChildrenCapacity { get; init; }
        public string View { get; init; }
        public float Rating { get; init; }
    }
}