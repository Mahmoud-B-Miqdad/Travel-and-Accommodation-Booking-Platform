using MediatR;
using TravelEase.Application.DiscountManagement.DTOs.Responses;

namespace TravelEase.Application.DiscountManagement.Queries
{
    public record GetDiscountByIdAndRoomTypeIdQuery : IRequest<DiscountResponse>
    {
        public Guid RoomTypeId { get; set; }
        public Guid DiscountId { get; set; }
    }
}