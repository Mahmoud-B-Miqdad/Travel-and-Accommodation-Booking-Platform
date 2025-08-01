using FluentValidation;
using TravelEase.Application.DiscountManagement.DTOs.Requests;

namespace TravelEase.Application.DiscountManagement.Validators
{
    public class GetAllDiscountsByRoomTypeIdQueryValidator : AbstractValidator<DiscountQueryRequest>
    {
        public GetAllDiscountsByRoomTypeIdQueryValidator()
        {
            RuleFor(discount => discount.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(discount => discount.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0.")
                .LessThan(21)
                .WithMessage("Page Size can't be greater than 20");
        }
    }
}