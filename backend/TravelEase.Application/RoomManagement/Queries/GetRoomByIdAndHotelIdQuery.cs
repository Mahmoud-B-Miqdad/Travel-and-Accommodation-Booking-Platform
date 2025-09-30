using MediatR;
using TravelEase.Application.RoomManagement.DTOs.Responses;

namespace TravelEase.Application.RoomManagement.Queries
{
    public class GetRoomByIdAndHotelIdQuery : IRequest<RoomResponse?>
    {
        public Guid HotelId { get; init; }
        public Guid RoomId { get; init; }
    }
}