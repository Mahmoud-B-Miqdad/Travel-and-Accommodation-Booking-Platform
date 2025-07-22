using FluentValidation;
using TravelEase.Application.ImageManagement.ForCityEntity.Queries;

namespace TravelEase.API.Validators.ImageValidators
{
    public class GetAllCityImagesQueryValidator : AbstractValidator<GetAllCityImagesQuery>
    {
        public GetAllCityImagesQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.");
        }
    }
}