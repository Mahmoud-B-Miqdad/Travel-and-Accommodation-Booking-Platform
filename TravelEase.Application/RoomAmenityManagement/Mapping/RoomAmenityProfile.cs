using AutoMapper;
using TravelEase.Domain.Aggregates.RoomAmenities;

namespace TravelEase.Application.RoomAmenityManagement.Mapping
{
    public class RoomAmenityProfile : Profile
    {
        public RoomAmenityProfile()
        {
            CreateMap<RoomAmenity, RoomAmenityProfile>();
        }
    }
}