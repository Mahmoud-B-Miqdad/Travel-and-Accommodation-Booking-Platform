using MediatR;

namespace TravelEase.Application.RoomManagement.Commands
{
    public record UpdateRoomCommand : IRequest
    {
        public Guid HotelId { get; init; }
        public Guid RoomId { get; init; }
        public Guid RoomTypeId { get; init; }
        public int AdultsCapacity { get; init; }
        public int ChildrenCapacity { get; init; }
        public string View { get; init; }
    }
}