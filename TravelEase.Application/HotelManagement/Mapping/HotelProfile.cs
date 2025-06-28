using AutoMapper;
using TravelEase.Application.HotelManagement.DTOs;
using TravelEase.Domain.Aggregates.Hotels;

namespace TravelEase.Application.HotelManagement.Mapping
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<Hotel, HotelWithoutRoomsDto>();
        }
    }
}