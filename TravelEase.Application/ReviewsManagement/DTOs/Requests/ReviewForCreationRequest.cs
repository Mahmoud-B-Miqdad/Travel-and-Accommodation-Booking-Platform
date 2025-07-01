namespace TravelEase.Application.ReviewsManagement.DTOs.Requests
{
    public record ReviewForCreationRequest
    {
        public Guid BookingId { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }
    }
}