using MediatR;

namespace TravelEase.Application.BookingManagement.Commands
{
    public record DeleteBookingCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}