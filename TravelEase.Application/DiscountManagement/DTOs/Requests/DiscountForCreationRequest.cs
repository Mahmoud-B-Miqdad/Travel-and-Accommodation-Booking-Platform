namespace TravelEase.Application.DiscountManagement.DTOs.Requests
{
    public record DiscountForCreationRequest
    {
        public float DiscountPercentage { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}