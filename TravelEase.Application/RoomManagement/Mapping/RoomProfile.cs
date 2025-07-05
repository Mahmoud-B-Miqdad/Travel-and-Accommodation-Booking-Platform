using AutoMapper;
using TravelEase.Application.RoomManagement.Commands;
using TravelEase.Application.RoomManagement.DTOs.Requests;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using TravelEase.Application.RoomManagement.Queries;
using TravelEase.Domain.Aggregates.Rooms;

namespace TravelEase.Application.RoomManagement.Mapping
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomResponse>();
            CreateMap<RoomQueryRequest, GetAllRoomsByHotelIdQuery>();
            CreateMap<RoomForCreationRequest, CreateRoomCommand>();
            CreateMap<CreateRoomCommand, Room>();
            CreateMap<CreateRoomCommand, RoomResponse>();
        }
    }
}