using MediatR;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;

namespace TravelEase.Application.ReviewsManagement.Commands
{
    public record CreateReviewCommand : IRequest<ReviewResponse?>
    {
        public Guid HotelId { get; init; }
        public Guid BookingId { get; init; }
        public string Comment { get; init; }
        public DateTime ReviewDate { get; init; } = DateTime.Today;
        public float Rating { get; init; }
        public string? GuestEmail { get; init; }
    }
}