using MediatR;
using TravelEase.Application.CityManagement.DTOs.Responses;

namespace TravelEase.Application.CityManagement.Queries
{
    public record GetTrendingCitiesQuery : IRequest<List<CityWithoutHotelsResponse>>
    {
        public int Count { get; set; } = 5;
    }
}