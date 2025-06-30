using AutoMapper;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.Hotels;

namespace TravelEase.Application.HotelManagement.Mapping
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<Hotel, HotelWithoutRoomsResponse>();
        }
    }
}