using MediatR;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.RoomTypeManagement.Queries
{
    public record GetAllRoomTypesByHotelIdQuery : IRequest<PaginatedList<RoomTypeResponse>>
    {
        public Guid HotelId { get; init; }
        public bool IncludeAmenities { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}