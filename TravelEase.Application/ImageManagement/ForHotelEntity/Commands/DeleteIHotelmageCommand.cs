using MediatR;

namespace TravelEase.Application.ImageManagement.ForHotelEntity.Commands
{
    public record DeleteIHotelmageCommand : IRequest
    {
        public Guid HotelId { get; set; }
        public Guid ImageId { get; set; }
    }
}