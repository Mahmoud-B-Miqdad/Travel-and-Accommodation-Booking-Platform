using MediatR;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;

namespace TravelEase.Application.RoomAmenityManagement.Query
{
    public record GetRoomAmenityByIdQuery : IRequest<RoomAmenityResponse?>
    {
        public Guid Id { get; set; }
    }
}