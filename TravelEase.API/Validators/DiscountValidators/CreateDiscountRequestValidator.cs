using FluentValidation;
using TravelEase.Application.DiscountManagement.DTOs.Requests;

namespace TravelEase.API.Validators.DiscountValidators
{
    public class DiscountForCreationRequestValidator : AbstractValidator<DiscountForCreationRequest>
    {
        public DiscountForCreationRequestValidator()
        {
            RuleFor(discount => discount.DiscountPercentage)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage("Discount percentage must be between 0 and 100.");

            RuleFor(discount => discount.FromDate)
                .GreaterThanOrEqualTo(discount => discount.FromDate.AddDays(1))
                .WithMessage("Check-out date must be at least one day after the check-in date");

            RuleFor(discount => discount.ToDate)
                .GreaterThanOrEqualTo(discount => discount.ToDate.AddDays(1))
                .WithMessage("Check-out date must be at least one day after the check-in date");
        }
    }
}