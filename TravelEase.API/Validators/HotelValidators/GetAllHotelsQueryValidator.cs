using FluentValidation;
using TravelEase.Application.HotelManagement.Queries;

namespace TravelEase.API.Validators.HotelValidators
{
    public class GetAllHotelsQueryValidator : AbstractValidator<GetAllHotelsQuery>
    {
        public GetAllHotelsQueryValidator()
        {
            RuleFor(roomAmenity => roomAmenity.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(roomAmenity => roomAmenity.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0.");
        }
    }
}