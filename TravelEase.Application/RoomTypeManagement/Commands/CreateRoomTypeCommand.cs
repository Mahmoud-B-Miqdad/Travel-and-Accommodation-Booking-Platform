using MediatR;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;
using TravelEase.Domain.Enums;

namespace TravelEase.Application.RoomTypeManagement.Commands
{
    public record CreateRoomTypeCommand : IRequest<RoomTypeResponse>
    {
        public Guid HotelId { get; set; }
        public RoomCategory Category { get; set; }
        public float PricePerNight { get; set; }
        public List<Guid> AmenityIds { get; set; } = new();
    }
}