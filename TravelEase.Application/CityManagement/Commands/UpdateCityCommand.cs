using MediatR;

namespace TravelEase.Application.CityManagement.Commands
{
    public record UpdateCityCommand : IRequest
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string CountryName { get; init; }
        public string CountryCode { get; init; }
        public string PostOffice { get; init; }
    }
}