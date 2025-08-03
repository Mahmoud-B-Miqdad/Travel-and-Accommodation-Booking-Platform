using MediatR;
using TravelEase.Application.RoomManagement.DTOs.Responses;

namespace TravelEase.Application.RoomManagement.Queries
{
    public class GetHotelAvailableRoomsQuery : IRequest<List<RoomResponse>>
    {
        public Guid HotelId { get; init; }
        public DateTime CheckInDate { get; init; }
        public DateTime CheckOutDate { get; init; }
    }
}