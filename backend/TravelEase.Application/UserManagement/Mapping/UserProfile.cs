using AutoMapper;
using TravelEase.Application.UserManagement.Commands;
using TravelEase.Application.UserManagement.DTOs.Requests;
using TravelEase.Domain.Aggregates.Users;

namespace TravelEase.Application.UserManagement.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserForCreationRequest, CreateUserCommand>();
            CreateMap<CreateUserCommand, User>();
            CreateMap<SignInRequest, SignInCommand>();
        }
    }
}