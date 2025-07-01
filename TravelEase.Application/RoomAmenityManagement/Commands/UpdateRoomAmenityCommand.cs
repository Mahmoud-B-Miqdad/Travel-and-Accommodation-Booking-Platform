using MediatR;

namespace TravelEase.Application.RoomAmenityManagement.Commands
{
    public record UpdateRoomAmenityCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}