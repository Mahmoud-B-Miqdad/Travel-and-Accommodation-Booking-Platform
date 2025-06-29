using MediatR;
using TravelEase.Application.CityManagement.DTOs.Responses;

namespace TravelEase.Application.CityManagement.Commands
{
    public record CreateCityCommand : IRequest<CityWithoutHotelsResponse?>
    {
        public string Name { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string PostOffice { get; set; }
    }
}