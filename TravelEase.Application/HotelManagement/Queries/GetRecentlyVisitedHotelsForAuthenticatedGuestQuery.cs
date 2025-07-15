using MediatR;
using TravelEase.Application.HotelManagement.DTOs.Responses;

namespace TravelEase.Application.HotelManagement.Queries
{
    public record GetRecentlyVisitedHotelsForAuthenticatedGuestQuery : IRequest<List<HotelWithoutRoomsResponse>>
    {
        public string GuestEmail { get; set; }
        public int Count { get; set; } = 5;
    }
}