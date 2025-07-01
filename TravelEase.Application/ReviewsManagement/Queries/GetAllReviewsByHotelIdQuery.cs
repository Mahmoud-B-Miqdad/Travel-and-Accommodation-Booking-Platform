using MediatR;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.ReviewsManagement.Queries
{
    public class GetAllReviewsByHotelIdQuery : IRequest<PaginatedList<ReviewResponse>>
    {
        public Guid HotelId { get; set; }
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}