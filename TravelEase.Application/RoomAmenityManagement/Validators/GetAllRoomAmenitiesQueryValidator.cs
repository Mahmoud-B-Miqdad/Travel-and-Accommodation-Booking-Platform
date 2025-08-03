using FluentValidation;
using TravelEase.Application.RoomAmenityManagement.Queries;

namespace TravelEase.Application.RoomAmenityManagement.Validators
{
    public class GetAllRoomAmenitiesQueryValidator : AbstractValidator<GetAllRoomAmenitiesQuery>
    {
        public GetAllRoomAmenitiesQueryValidator()
        {
            RuleFor(roomAmenity => roomAmenity.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(roomAmenity => roomAmenity.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0.")
                .LessThan(21)
                .WithMessage("Page Size can't be greater than 20");
        }
    }
}