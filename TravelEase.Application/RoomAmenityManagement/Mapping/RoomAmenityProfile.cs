using AutoMapper;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.RoomAmenities;

namespace TravelEase.Application.RoomAmenityManagement.Mapping
{
    public class RoomAmenityProfile : Profile
    {
        public RoomAmenityProfile()
        {
            CreateMap<RoomAmenity, RoomAmenityResponse>();
        }
    }
}