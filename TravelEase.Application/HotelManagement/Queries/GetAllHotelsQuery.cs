using MediatR;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.HotelManagement.Queries
{
    public class GetAllHotelsQuery : IRequest<PaginatedList<HotelWithoutRoomsResponse>>
    {
        public string? SearchQuery { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}