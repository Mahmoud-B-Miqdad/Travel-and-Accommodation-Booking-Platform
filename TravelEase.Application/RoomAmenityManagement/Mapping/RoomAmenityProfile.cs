using AutoMapper;
using TravelEase.Application.RoomAmenityManagement.Commands;
using TravelEase.Application.RoomAmenityManagement.DTOs.Requests;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.RoomAmenities;

namespace TravelEase.Application.RoomAmenityManagement.Mapping
{
    public class RoomAmenityProfile : Profile
    {
        public RoomAmenityProfile()
        {
            CreateMap<RoomAmenity, RoomAmenityResponse>();
            CreateMap<RoomAmenityForCreationRequest, CreateRoomAmenityCommand>();
            CreateMap<CreateRoomAmenityCommand, RoomAmenity>();
            CreateMap<CreateRoomAmenityCommand, RoomAmenityResponse>();
            CreateMap<RoomAmenityForUpdateRequest, UpdateRoomAmenityCommand>();
            CreateMap<UpdateRoomAmenityCommand, RoomAmenity>();
        }
    }
}