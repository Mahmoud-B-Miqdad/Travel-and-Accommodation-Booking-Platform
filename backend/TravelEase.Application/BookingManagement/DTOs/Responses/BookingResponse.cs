namespace TravelEase.Application.BookingManagement.DTOs.Responses
{
    public record BookingResponse
    {
        public Guid Id { get; init; }
        public Guid RoomId { get; init; }
        public Guid UserId { get; init; }
        public DateTime CheckInDate { get; init; }
        public DateTime CheckOutDate { get; init; }
        public DateTime BookingDate { get; init; }
        public double Price { get; init; }
    }
}