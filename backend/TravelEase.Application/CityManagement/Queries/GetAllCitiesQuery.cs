using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;
using MediatR;

namespace TravelEase.Application.CityManagement.Queries
{
    public record GetAllCitiesQuery : IRequest<PaginatedList<CityResponse>>
    {
        public bool IncludeHotels { get; init; } = false;
        public string? SearchQuery { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}