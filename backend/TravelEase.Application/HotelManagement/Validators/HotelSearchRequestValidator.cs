using FluentValidation;
using TravelEase.Domain.Common.Models.HotelSearchModels;

namespace TravelEase.Application.HotelManagement.Validators
{
    public class HotelSearchRequestValidator : AbstractValidator<HotelSearchParameters>
    {
        public HotelSearchRequestValidator()
        {
            RuleFor(x => x.CheckInDate)
                .NotEmpty().WithMessage("Check-in date is required.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Check-in date cannot be in the past.");

            RuleFor(x => x.CheckOutDate)
                .NotEmpty().WithMessage("Check-out date is required.")
                .GreaterThan(x => x.CheckInDate).WithMessage("Check-out date must be after check-in date.");

            RuleFor(x => x.StarRate)
                .InclusiveBetween(0, 5).WithMessage("Star rate must be between 0 and 5.");

            RuleFor(x => x.Adults)
                .GreaterThan(0).WithMessage("At least one adult is required.");

            RuleFor(x => x.Children)
                .GreaterThanOrEqualTo(0).WithMessage("Children count cannot be negative.");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
        }
    }
}