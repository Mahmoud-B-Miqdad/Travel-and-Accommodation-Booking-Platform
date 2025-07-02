using AutoMapper;
using TravelEase.Application.ReviewsManagement.DTOs.Commands;
using TravelEase.Application.ReviewsManagement.DTOs.Requests;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Application.ReviewsManagement.Queries;
using TravelEase.Domain.Aggregates.Reviews;

namespace TravelEase.Application.ReviewsManagement.Mapping
{
    public class ReviewsProfile : Profile
    {
        public ReviewsProfile()
        {
            CreateMap<Review, ReviewResponse>();
            CreateMap<ReviewQueryRequest, GetAllReviewsByHotelIdQuery>();
            CreateMap<ReviewForCreationRequest, CreateReviewCommand>();
            CreateMap<CreateReviewCommand, Review>();
            CreateMap<CreateReviewCommand, ReviewResponse>();
        }
    }
}