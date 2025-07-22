using MediatR;

namespace TravelEase.Application.ImageManagement.ForHotelEntity.Commands
{
    public record DeleteHotelImageCommand : IRequest
    {
        public Guid HotelId { get; set; }
        public Guid ImageId { get; set; }
    }
}