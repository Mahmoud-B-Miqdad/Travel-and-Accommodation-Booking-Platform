using AutoMapper;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.Reviews;

namespace TravelEase.Application.ReviewsManagement.Mapping
{
    public class ReviewsProfile : Profile
    {
        public ReviewsProfile()
        {
            CreateMap<Review, ReviewResponse>();
        }
    }
}