using MediatR;
using TravelEase.Application.HotelManagement.DTOs.Responses;

namespace TravelEase.Application.HotelManagement.Queries
{
    public record GetRecentlyVisitedHotelsForGuestQuery : IRequest<List<HotelWithoutRoomsResponse>>
    {
        public Guid GuestId { get; set; }
        public int Count { get; set; } = 5;
    }
}