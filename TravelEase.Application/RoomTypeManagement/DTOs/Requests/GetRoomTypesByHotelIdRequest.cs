namespace TravelEase.Application.RoomTypeManagement.DTOs.Requests
{
    public record GetRoomTypesByHotelIdRequest
    {
        public bool IncludeAmenities { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}