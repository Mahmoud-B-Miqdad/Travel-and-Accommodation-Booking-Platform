namespace TravelEase.Application.DiscountManagement.DTOs.Requests
{
    public record DiscountForCreationRequest
    {
        public float DiscountPercentage { get; init; }
        public DateTime FromDate { get; init; }
        public DateTime ToDate { get; init; }
    }
}