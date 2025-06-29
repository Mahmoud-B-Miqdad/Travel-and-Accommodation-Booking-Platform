using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.Cities;
using AutoMapper;
using TravelEase.Application.CityManagement.DTOs.Requests;
using TravelEase.Application.CityManagement.Commands;

namespace TravelEase.Application.CityManagement.Mapping
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityResponse>();
            CreateMap<CityResponse, CityWithoutHotelsResponse>();
            CreateMap<CityForCreationRequest, CreateCityCommand>();
            CreateMap<CreateCityCommand, City>();
            CreateMap<City, CityWithoutHotelsResponse>();
            CreateMap<CityForUpdateRequest, UpdateCityCommand>();
            CreateMap<UpdateCityCommand, City>();

        }
    }
}