namespace TravelEase.Application.DiscountManagement.DTOs.Requests
{
    public record DiscountQueryRequest
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}