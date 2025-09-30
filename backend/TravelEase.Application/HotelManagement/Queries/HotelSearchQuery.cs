using MediatR;
using TravelEase.Domain.Common.Models.HotelSearchModels;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.HotelManagement.Queries
{
    public record HotelSearchQuery : IRequest<PaginatedList<HotelSearchResult>>
    {
        public DateTime CheckInDate { get; init; }
        public DateTime CheckOutDate { get; init; }
        public string? CityName { get; init; }
        public float StarRate { get; init; } = 3;
        public int Adults { get; init; } = 2;
        public int Children { get; init; } = 1;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 5;
    }
}