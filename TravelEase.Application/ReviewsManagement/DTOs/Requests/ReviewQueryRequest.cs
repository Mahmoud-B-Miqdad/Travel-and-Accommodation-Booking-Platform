namespace TravelEase.Application.ReviewsManagement.DTOs.Requests
{
    public record ReviewQueryRequest
    {
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}