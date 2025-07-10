using AutoMapper;
using TravelEase.Application.DiscountManagement.DTOs.Requests;
using TravelEase.Application.DiscountManagement.DTOs.Responses;
using TravelEase.Application.DiscountManagement.Queries;
using TravelEase.Domain.Aggregates.Discounts;

namespace TravelEase.Application.DiscountManagement.Mapping
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<DiscountQueryRequest, GetAllDiscountsByRoomTypeQuery>();
            CreateMap<Discount, DiscountResponse>();
        }
    }
}