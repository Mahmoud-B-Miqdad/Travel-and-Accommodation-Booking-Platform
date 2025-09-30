namespace TravelEase.Application.RoomManagement.DTOs.Requests
{
    public class RoomForCreationRequest
    {
        public Guid RoomTypeId { get; init; }
        public int AdultsCapacity { get; init; }
        public int ChildrenCapacity { get; init; }
        public string View { get; init; }
        public float Rating { get; init; }
    }
}