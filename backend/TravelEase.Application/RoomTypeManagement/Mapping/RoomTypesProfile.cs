using AutoMapper;
using TravelEase.Application.RoomTypeManagement.Commands;
using TravelEase.Application.RoomTypeManagement.DTOs.Requests;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;
using TravelEase.Application.RoomTypeManagement.Queries;
using TravelEase.Domain.Aggregates.RoomTypes;

namespace TravelEase.Application.ReviewsManagement.Mapping
{
    public class RoomTypesProfile : Profile
    {
        public RoomTypesProfile()
        {
            CreateMap<RoomType, RoomTypeResponse>();
            CreateMap<GetRoomTypesByHotelIdRequest, GetAllRoomTypesByHotelIdQuery>();
            CreateMap<RoomTypeResponse, RoomTypeWithoutAmenitiesResponse>();
            CreateMap<RoomTypeForCreationRequest, CreateRoomTypeCommand>();
            CreateMap<CreateRoomTypeCommand, RoomType>();
            CreateMap<CreateRoomTypeCommand, RoomTypeResponse>();
        }
    }
}