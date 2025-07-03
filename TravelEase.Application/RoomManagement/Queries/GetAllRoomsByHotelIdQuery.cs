using MediatR;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.RoomManagement.Queries
{
    internal class GetAllRoomsByHotelIdQuery : IRequest<PaginatedList<RoomResponse>>
    {
        public Guid HotelId { get; set; }
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}