using MediatR;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;

namespace TravelEase.Application.ReviewsManagement.Queries
{
    public class GetReviewByIdAndHotelIdQuery : IRequest<ReviewResponse>
    {
        public Guid HotelId { get; init; }
        public Guid ReviewId { get; init; }
    }
}