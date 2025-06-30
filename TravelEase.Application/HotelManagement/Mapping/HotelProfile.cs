using AutoMapper;
using TravelEase.Application.HotelManagement.Commands;
using TravelEase.Application.HotelManagement.DTOs.Requests;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.Hotels;

namespace TravelEase.Application.HotelManagement.Mapping
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<Hotel, HotelWithoutRoomsResponse>();
            CreateMap<HotelForCreationRequest, CreateHotelCommand>();
            CreateMap<CreateHotelCommand, Hotel>();
            CreateMap<CreateHotelCommand, HotelWithoutRoomsResponse>();
            CreateMap<HotelForUpdateRequest, UpdateHotelCommand>();
            CreateMap<UpdateHotelCommand, Hotel>();
        }
    }
}