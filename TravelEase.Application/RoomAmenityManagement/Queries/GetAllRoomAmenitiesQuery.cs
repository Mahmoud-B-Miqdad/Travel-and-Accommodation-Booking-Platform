using MediatR;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.RoomAmenityManagement.Queries
{
    public record GetAllRoomAmenitiesQuery : IRequest<PaginatedList<RoomAmenityResponse>>
    {
        public string? SearchQuery { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}