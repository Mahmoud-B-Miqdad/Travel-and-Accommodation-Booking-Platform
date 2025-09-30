using MediatR;

namespace TravelEase.Application.CityManagement.Commands
{
    public record DeleteCityCommand : IRequest
    {
        public Guid Id { get; init; }
    }
}