using MediatR;

namespace TravelEase.Application.ImageManagement.ForCityEntity.Commands
{
    public record DeleteCityImageCommand : IRequest
    {
        public Guid CityId { get; set; }
        public Guid ImageId { get; set; }
    }
}