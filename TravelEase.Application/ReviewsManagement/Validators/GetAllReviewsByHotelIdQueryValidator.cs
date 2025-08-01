using FluentValidation;
using TravelEase.Application.ReviewsManagement.Queries;

namespace TravelEase.Application.ReviewsManagement.Validators
{
    public class GetAllReviewsByHotelIdQueryValidator : AbstractValidator<GetAllReviewsByHotelIdQuery>
    {
        public GetAllReviewsByHotelIdQueryValidator()
        {
            RuleFor(review => review.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(review => review.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0.")
                .LessThan(21)
                .WithMessage("Page Size can't be greater than 20");
        }
    }
}