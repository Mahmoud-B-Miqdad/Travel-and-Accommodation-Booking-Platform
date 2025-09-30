namespace TravelEase.Application.ReviewsManagement.DTOs.Requests
{
    public record ReviewQueryRequest
    {
        public string? SearchQuery { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}