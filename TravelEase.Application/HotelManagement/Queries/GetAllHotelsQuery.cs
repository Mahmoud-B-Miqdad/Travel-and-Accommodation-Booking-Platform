using MediatR;
using TravelEase.Application.HotelManagement.DTOs;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.HotelManagement.Queries
{
    public class GetAllHotelsQuery : IRequest<PaginatedList<HotelWithoutRoomsDto>>
    {
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}