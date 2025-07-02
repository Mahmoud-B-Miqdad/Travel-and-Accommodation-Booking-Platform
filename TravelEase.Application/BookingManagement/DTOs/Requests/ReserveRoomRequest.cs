namespace TravelEase.Application.BookingManagement.DTOs.Requests
{
    public record ReserveRoomRequest
    {
        public Guid RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}