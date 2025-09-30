namespace TravelEase.Application.BookingManagement.DTOs.Requests
{
    public record ReserveRoomRequest
    {
        public Guid RoomId { get; init; }
        public DateTime CheckInDate { get; init; }
        public DateTime CheckOutDate { get; init; }
    }
}