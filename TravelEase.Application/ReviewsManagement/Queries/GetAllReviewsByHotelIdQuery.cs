using MediatR;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.ReviewsManagement.Queries
{
    public class GetAllReviewsByHotelIdQuery : IRequest<PaginatedList<ReviewResponse>>
    {
        public Guid HotelId { get; init; }
        public string? SearchQuery { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}