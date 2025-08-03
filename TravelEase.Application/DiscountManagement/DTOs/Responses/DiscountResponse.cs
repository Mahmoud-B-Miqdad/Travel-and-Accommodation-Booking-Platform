namespace TravelEase.Application.DiscountManagement.DTOs.Responses
{
    public record DiscountResponse
    {
        public Guid Id { get; init; }
        public Guid RoomTypeId { get; init; }
        public float DiscountPercentage { get; init; }
        public DateTime FromDate { get; init; }
        public DateTime ToDate { get; init; }
    }
}