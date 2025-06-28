using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.Cities;
using AutoMapper;

namespace TravelEase.Application.CityManagement.Mapping
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityResponse>();
            CreateMap<CityResponse, CityWithoutHotelsResponse>();
        }
    }
}