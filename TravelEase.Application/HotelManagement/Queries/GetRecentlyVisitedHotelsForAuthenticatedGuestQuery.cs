using MediatR;
using TravelEase.Application.HotelManagement.DTOs.Responses;

namespace TravelEase.Application.HotelManagement.Queries
{
    public record GetRecentlyVisitedHotelsForAuthenticatedGuestQuery : IRequest<List<HotelWithoutRoomsResponse>>
    {
        public string GuestEmail { get; init; }
        public int Count { get; init; } = 5;
    }
}