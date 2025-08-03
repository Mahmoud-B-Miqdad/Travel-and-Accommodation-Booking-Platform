using MediatR;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;

namespace TravelEase.Application.RoomAmenityManagement.Commands
{
    public record CreateRoomAmenityCommand : IRequest<RoomAmenityResponse?>
    {
        public string Name { get; init; }
        public string Description { get; init; }
    }
}