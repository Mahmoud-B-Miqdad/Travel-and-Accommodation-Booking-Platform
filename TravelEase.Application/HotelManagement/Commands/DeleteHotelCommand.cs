using MediatR;

namespace TravelEase.Application.HotelManagement.Commands
{
    public record DeleteHotelCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}