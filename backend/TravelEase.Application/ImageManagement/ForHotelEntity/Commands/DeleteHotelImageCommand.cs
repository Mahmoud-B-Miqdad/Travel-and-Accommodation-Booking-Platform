using MediatR;

namespace TravelEase.Application.ImageManagement.ForHotelEntity.Commands
{
    public record DeleteHotelImageCommand : IRequest
    {
        public Guid HotelId { get; init; }
        public Guid ImageId { get; init; }
    }
}