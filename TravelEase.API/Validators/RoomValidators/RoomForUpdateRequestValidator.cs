using FluentValidation;
using TravelEase.Application.RoomManagement.DTOs.Requests;

namespace TravelEase.API.Validators.RoomValidators
{
    public class RoomForUpdateRequestValidator : AbstractValidator<RoomForUpdateRequest>
    {
        public RoomForUpdateRequestValidator()
        {
            RuleFor(x => x.RoomTypeId)
                .NotEmpty().WithMessage("Room type is required.");

            RuleFor(x => x.AdultsCapacity)
                .GreaterThanOrEqualTo(0).WithMessage("Adults capacity must be 0 or greater.");

            RuleFor(x => x.ChildrenCapacity)
                .GreaterThanOrEqualTo(0).WithMessage("Children capacity must be 0 or greater.");

            RuleFor(x => x.View)
                .NotEmpty().WithMessage("Room view is required.")
                .MinimumLength(2).WithMessage("Room view must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Room view must not exceed 100 characters.");
        }
    }
}