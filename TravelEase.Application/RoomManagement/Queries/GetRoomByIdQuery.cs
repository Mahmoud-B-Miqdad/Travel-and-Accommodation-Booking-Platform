using MediatR;
using TravelEase.Application.RoomManagement.DTOs.Responses;

namespace TravelEase.Application.RoomManagement.Queries
{
    public class GetRoomByIdQuery : IRequest<RoomResponse?>
    {
        public Guid Id { get; set; }
    }
}