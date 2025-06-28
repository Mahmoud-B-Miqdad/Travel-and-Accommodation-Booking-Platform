using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;
using MediatR;

namespace TravelEase.Application.CityManagement.Queries
{
    public record GetAllCitiesQuery : IRequest<PaginatedList<CityResponse>>
    {
        public bool IncludeHotels { get; set; } = false;
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}