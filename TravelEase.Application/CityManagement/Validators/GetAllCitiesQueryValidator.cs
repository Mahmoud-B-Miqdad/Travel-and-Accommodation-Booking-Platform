using FluentValidation;
using TravelEase.Application.CityManagement.Queries;

namespace TravelEase.Application.CityManagement.Validators
{
    public class GetAllCitiesQueryValidator : AbstractValidator<GetAllCitiesQuery>
    {
        public GetAllCitiesQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.");
        }
    }
}