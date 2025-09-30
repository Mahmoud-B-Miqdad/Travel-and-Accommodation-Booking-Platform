namespace TravelEase.Application.ReviewsManagement.DTOs.Requests
{
    public record ReviewForCreationRequest
    {
        public Guid BookingId { get; init; }
        public string Comment { get; init; }
        public float Rating { get; init; }
    }
}