using MediatR;
using TravelEase.Application.RoomManagement.DTOs.Responses;

namespace TravelEase.Application.RoomManagement.Queries
{
    internal class GetRoomByIdQuery : IRequest<RoomResponse?>
    {
        public Guid RoomId { get; set; }
    }
}