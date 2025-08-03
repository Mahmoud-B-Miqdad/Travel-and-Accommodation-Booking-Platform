namespace TravelEase.Application.RoomTypeManagement.DTOs.Requests
{
    public record GetRoomTypesByHotelIdRequest
    {
        public bool IncludeAmenities { get; init; } = false;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}