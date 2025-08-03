using MediatR;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;
using TravelEase.Domain.Enums;

namespace TravelEase.Application.RoomTypeManagement.Commands
{
    public record CreateRoomTypeCommand : IRequest<RoomTypeResponse>
    {
        public Guid HotelId { get; init; }
        public RoomCategory Category { get; init; }
        public float PricePerNight { get; init; }
        public List<Guid> AmenityIds { get; init; } = new();
    }
}