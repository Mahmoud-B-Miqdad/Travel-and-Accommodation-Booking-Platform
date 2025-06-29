using MediatR;
using TravelEase.Application.CityManagement.DTOs.Responses;

namespace TravelEase.Application.CityManagement.Queries
{
    public record GetCityByIdQuery : IRequest<CityResponse?>
    {
        public Guid Id { get; set; }
    }
}