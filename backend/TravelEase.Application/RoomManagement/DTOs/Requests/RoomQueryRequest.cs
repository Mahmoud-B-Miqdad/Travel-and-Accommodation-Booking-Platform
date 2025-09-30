namespace TravelEase.Application.RoomManagement.DTOs.Requests
{
    public class RoomQueryRequest
    {
        public string? SearchQuery { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}