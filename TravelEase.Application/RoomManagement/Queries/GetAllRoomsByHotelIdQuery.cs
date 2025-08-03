using MediatR;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.RoomManagement.Queries
{
    public class GetAllRoomsByHotelIdQuery : IRequest<PaginatedList<RoomResponse>>
    {
        public Guid HotelId { get; init; }
        public string? SearchQuery { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}