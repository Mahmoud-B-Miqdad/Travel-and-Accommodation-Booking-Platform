using FluentValidation;
using TravelEase.Application.ImageManagement.ForHotelEntity.Queries;

namespace TravelEase.API.Validators.ImageValidators
{
    public class GetAllImagesQueryValidator : AbstractValidator<GetAllImagesQuery>
    {
        public GetAllImagesQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.");
        }
    }
}