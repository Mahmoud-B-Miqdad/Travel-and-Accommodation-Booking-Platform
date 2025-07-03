namespace TravelEase.Application.RoomManagement.DTOs.Responses
{
    public record RoomResponse
    {
        public Guid Id { get; set; }
        public Guid RoomTypeId { get; set; }
        public int AdultsCapacity { get; set; }
        public int ChildrenCapacity { get; set; }
        public string View { get; set; }
        public float Rating { get; set; }
    }
}