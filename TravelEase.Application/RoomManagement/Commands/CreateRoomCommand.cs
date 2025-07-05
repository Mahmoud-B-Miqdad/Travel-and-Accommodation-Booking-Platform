using MediatR;
using TravelEase.Application.RoomManagement.DTOs.Responses;

namespace TravelEase.Application.RoomManagement.Commands
{
    public record CreateRoomCommand : IRequest<RoomResponse?>
    {
        public Guid HotelId { get; set; }
        public Guid RoomTypeId { get; set; }
        public int AdultsCapacity { get; set; }
        public int ChildrenCapacity { get; set; }
        public string View { get; set; }
        public float Rating { get; set; }
    }
}