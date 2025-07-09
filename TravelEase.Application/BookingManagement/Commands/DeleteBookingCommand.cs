using MediatR;

namespace TravelEase.Application.BookingManagement.Commands
{
    public record DeleteBookingCommand : IRequest
    {
        public Guid HotelId { get; init; }
        public Guid BookingId { get; init; }
        public string GuestEmail { get; init; }
    }
}