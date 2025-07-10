namespace TravelEase.Application.DiscountManagement.DTOs.Requests
{
    public record DiscountQueryRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}