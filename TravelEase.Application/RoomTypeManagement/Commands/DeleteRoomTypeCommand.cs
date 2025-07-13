using MediatR;

namespace TravelEase.Application.RoomTypeManagement.Commands
{
    public record DeleteRoomTypeCommand : IRequest
    {
        public Guid HotelId { get; init; }
        public Guid RoomTypeId { get; init; }
    }
}