using MediatR;
using TravelEase.Application.DiscountManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.DiscountManagement.Queries
{
    public record GetAllDiscountsByRoomTypeQuery : IRequest<PaginatedList<DiscountResponse>>
    {
        public Guid RoomTypeId { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}