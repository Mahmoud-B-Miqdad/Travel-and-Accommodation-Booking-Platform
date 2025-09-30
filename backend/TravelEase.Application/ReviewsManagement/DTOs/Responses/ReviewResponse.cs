namespace TravelEase.Application.ReviewsManagement.DTOs.Responses
{
    public record ReviewResponse
    {
        public Guid Id { get; init; }
        public string Comment { get; init; }
        public DateTime ReviewDate { get; init; }
        public float Rating { get; init; }
    }
}