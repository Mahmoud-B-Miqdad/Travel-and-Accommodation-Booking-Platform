using MediatR;

namespace TravelEase.Application.RoomManagement.Commands
{
    public record UpdateRoomCommand : IRequest
    {
        public Guid HotelId { get; set; }
        public Guid RoomId { get; set; }
        public Guid RoomTypeId { get; set; }
        public int AdultsCapacity { get; set; }
        public int ChildrenCapacity { get; set; }
        public string View { get; set; }
    }
}