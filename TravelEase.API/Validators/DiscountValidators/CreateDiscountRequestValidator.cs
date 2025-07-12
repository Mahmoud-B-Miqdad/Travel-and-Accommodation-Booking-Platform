using FluentValidation;
using TravelEase.Application.DiscountManagement.DTOs.Requests;

namespace TravelEase.API.Validators.DiscountValidators
{
    public class DiscountForCreationRequestValidator : AbstractValidator<DiscountForCreationRequest>
    {
        public DiscountForCreationRequestValidator()
        {
            RuleFor(x => x.DiscountPercentage)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage("Discount percentage must be between 0 and 100.");

            RuleFor(x => x.FromDate)
                .Must(BeValidDate).WithMessage("FromDate must be a valid date.")
                .LessThan(x => x.ToDate).WithMessage("FromDate must be earlier than ToDate.");

            RuleFor(x => x.ToDate)
                .Must(BeValidDate).WithMessage("ToDate must be a valid date.")
                .GreaterThan(x => x.FromDate).WithMessage("ToDate must be later than FromDate.");
        }

        private bool BeValidDate(DateTime date)
        {
            return date != default;
        }
    }
}