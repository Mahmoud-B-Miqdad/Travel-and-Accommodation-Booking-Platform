namespace TravelEase.Application.RoomManagement.DTOs.Requests
{
    public class RoomQueryRequest
    {
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}