namespace TravelEase.Application.ReviewsManagement.DTOs.Responses
{
    public record ReviewResponse
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public float Rating { get; set; }
    }
}