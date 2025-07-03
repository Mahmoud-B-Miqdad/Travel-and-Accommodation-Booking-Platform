using FluentValidation;
using TravelEase.Application.RoomManagement.Queries;

namespace TravelEase.API.Validators.RoomValidators
{
    public class GetAllRoomsByHotelIdQueryValidator : AbstractValidator<GetAllRoomsByHotelIdQuery>
    {
        public GetAllRoomsByHotelIdQueryValidator()
        {
            RuleFor(booking => booking.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(booking => booking.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0.")
                .LessThan(21)
                .WithMessage("Page Size can't be greater than 20");
        }
    }
}