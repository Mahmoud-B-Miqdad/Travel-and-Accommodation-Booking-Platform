using MediatR;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.RoomTypeManagement.Queries
{
    public record GetAllRoomTypesByHotelIdQuery : IRequest<PaginatedList<RoomTypeResponse>>
    {
        public Guid HotelId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}