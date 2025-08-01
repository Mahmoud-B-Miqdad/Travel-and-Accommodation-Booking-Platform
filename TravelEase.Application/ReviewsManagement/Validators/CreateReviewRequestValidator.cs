using FluentValidation;
using TravelEase.Application.ReviewsManagement.DTOs.Requests;

namespace TravelEase.Application.ReviewsManagement.Validators
{
    public class CreateReviewRequestValidator : AbstractValidator<ReviewForCreationRequest>
    {
        public CreateReviewRequestValidator()
        {
            RuleFor(review => review.BookingId)
                .NotEmpty().WithMessage("BookingId field shouldn't be empty");

            RuleFor(review => review.Comment)
                .NotEmpty()
                .WithMessage("Comment field shouldn't be empty");

            RuleFor(review => review.Rating)
                .InclusiveBetween(0, 5)
                .WithMessage("Rating must be between 0 and 5.");
        }
    }
}