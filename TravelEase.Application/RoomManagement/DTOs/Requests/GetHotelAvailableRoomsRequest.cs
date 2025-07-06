namespace TravelEase.Application.RoomManagement.DTOs.Requests
{
    public record GetHotelAvailableRoomsRequest
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}