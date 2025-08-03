using MediatR;
using TravelEase.Application.CityManagement.DTOs.Responses;

namespace TravelEase.Application.CityManagement.Commands
{
    public record CreateCityCommand : IRequest<CityWithoutHotelsResponse?>
    {
        public string Name { get; init; }
        public string CountryName { get; init; }
        public string CountryCode { get; init; }
        public string PostOffice { get; init; }
    }
}