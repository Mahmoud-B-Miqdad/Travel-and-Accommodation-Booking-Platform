using MediatR;
using TravelEase.Application.BookingManagement.DTOs.Responses;

namespace TravelEase.Application.BookingManagement.Commands
{
    public record ReserveRoomCommand : IRequest<BookingResponse?>
    {
        public Guid HotelId { get; set; }
        public Guid RoomId { get; set; }
        public string GuestEmail { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}