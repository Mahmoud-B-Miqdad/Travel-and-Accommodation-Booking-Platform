using MediatR;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;

namespace TravelEase.Application.ReviewsManagement.Queries
{
    public class GetReviewByIdQuery : IRequest<ReviewResponse>
    {
        public Guid Id { get; set; }
    }
}