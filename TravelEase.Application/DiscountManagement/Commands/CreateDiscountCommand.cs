using MediatR;
using TravelEase.Application.DiscountManagement.DTOs.Responses;

namespace TravelEase.Application.DiscountManagement.Commands
{
    public record CreateDiscountCommand : IRequest<DiscountResponse?>
    {
        public Guid RoomTypeId { get; set; }
        public float DiscountPercentage { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}