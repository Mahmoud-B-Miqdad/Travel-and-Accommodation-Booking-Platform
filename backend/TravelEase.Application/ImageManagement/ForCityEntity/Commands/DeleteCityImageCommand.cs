using MediatR;

namespace TravelEase.Application.ImageManagement.ForCityEntity.Commands
{
    public record DeleteCityImageCommand : IRequest
    {
        public Guid CityId { get; init; }
        public Guid ImageId { get; init; }
    }
}