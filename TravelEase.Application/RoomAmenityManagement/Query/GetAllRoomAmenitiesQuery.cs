using MediatR;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.RoomAmenityManagement.Query
{
    public record GetAllRoomAmenitiesQuery : IRequest<PaginatedList<RoomAmenityResponse>>
    {
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}