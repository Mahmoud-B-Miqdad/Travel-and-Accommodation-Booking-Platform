using MediatR;
using TravelEase.Application.BookingManagement.DTOs.Responses;

namespace TravelEase.Application.BookingManagement.Commands
{
    public record ReserveRoomCommand : IRequest<BookingResponse?>
    {
        public Guid HotelId { get; init; }
        public Guid RoomId { get; init; }
        public string GuestEmail { get; init; }
        public DateTime CheckInDate { get; init; }
        public DateTime CheckOutDate { get; init; }
        public DateTime BookingDate { get; init; } = DateTime.Now;
    }
}