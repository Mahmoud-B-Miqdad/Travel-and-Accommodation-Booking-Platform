using FluentValidation;
using TravelEase.Application.BookingManagement.Queries;

namespace TravelEase.API.Validators.BookingValidators
{
    public class GetAllBookingsByHotelIdQueryValidator : AbstractValidator<GetAllBookingsByHotelIdQuery>
    {
        public GetAllBookingsByHotelIdQueryValidator()
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