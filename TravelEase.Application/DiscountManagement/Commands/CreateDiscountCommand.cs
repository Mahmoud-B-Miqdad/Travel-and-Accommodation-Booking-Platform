using MediatR;
using TravelEase.Application.DiscountManagement.DTOs.Responses;

namespace TravelEase.Application.DiscountManagement.Commands
{
    public record CreateDiscountCommand : IRequest<DiscountResponse?>
    {
        public Guid RoomTypeId { get; init; }
        public float DiscountPercentage { get; init; }
        public DateTime FromDate { get; init; }
        public DateTime ToDate { get; init; }
    }
}