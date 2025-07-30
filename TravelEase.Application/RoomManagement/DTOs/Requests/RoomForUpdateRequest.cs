namespace TravelEase.Application.RoomManagement.DTOs.Requests
{
    public record RoomForUpdateRequest
    {
        public Guid RoomTypeId { get; set; }
        public int AdultsCapacity { get; set; }
        public int ChildrenCapacity { get; set; }
        public string View { get; set; }
    }
}