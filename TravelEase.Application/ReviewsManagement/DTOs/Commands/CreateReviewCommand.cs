using MediatR;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;

namespace TravelEase.Application.ReviewsManagement.DTOs.Commands
{
    public class CreateReviewCommand : IRequest<ReviewResponse?>
    {
        public Guid BookingId { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Today;
        public float Rating { get; set; }
    }
}