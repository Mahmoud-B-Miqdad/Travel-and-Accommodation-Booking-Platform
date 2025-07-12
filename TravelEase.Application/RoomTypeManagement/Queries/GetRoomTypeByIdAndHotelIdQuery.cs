using MediatR;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;

namespace TravelEase.Application.RoomTypeManagement.Queries
{
    public record GetRoomTypeByIdAndHotelIdQuery : IRequest<RoomTypeResponse>
    {
        public Guid HotelId { get; init; }
        public Guid RoomTypeId { get; init; }
    }
}