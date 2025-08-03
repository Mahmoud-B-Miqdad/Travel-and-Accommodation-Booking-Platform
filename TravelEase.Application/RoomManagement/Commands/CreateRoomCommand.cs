using MediatR;
using TravelEase.Application.RoomManagement.DTOs.Responses;

namespace TravelEase.Application.RoomManagement.Commands
{
    public record CreateRoomCommand : IRequest<RoomResponse?>
    {
        public Guid HotelId { get; init; }
        public Guid RoomTypeId { get; init; }
        public int AdultsCapacity { get; init; }
        public int ChildrenCapacity { get; init; }
        public string View { get; init; }
        public float Rating { get; init; }
    }
}