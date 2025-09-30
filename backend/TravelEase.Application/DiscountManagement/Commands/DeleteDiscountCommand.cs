using MediatR;

namespace TravelEase.Application.DiscountManagement.Commands
{
    public record DeleteDiscountCommand : IRequest
    {
        public Guid RoomTypeId { get; init; }
        public Guid DiscountId { get; init; }
    }
}