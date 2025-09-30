using MediatR;

namespace TravelEase.Application.RoomAmenityManagement.Commands
{
    public record UpdateRoomAmenityCommand : IRequest
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
    }
}