namespace TravelEase.Application.RoomManagement.DTOs.Requests
{
    public record GetHotelAvailableRoomsRequest
    {
        public DateTime CheckInDate { get; init; }
        public DateTime CheckOutDate { get; init; }
    }
}