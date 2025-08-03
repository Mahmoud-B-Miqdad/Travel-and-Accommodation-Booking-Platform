using MediatR;

namespace TravelEase.Application.RoomAmenityManagement.Commands
{
    public record DeleteRoomAmenityCommand : IRequest
    {
        public Guid Id { get; init; }
    }
}