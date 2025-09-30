using MediatR;
using TravelEase.Application.DiscountManagement.DTOs.Responses;

namespace TravelEase.Application.DiscountManagement.Queries
{
    public record GetDiscountByIdAndRoomTypeIdQuery : IRequest<DiscountResponse>
    {
        public Guid RoomTypeId { get; init; }
        public Guid DiscountId { get; init; }
    }
}