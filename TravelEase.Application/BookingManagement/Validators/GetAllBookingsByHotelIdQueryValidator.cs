using FluentValidation;
using TravelEase.Application.BookingManagement.DTOs.Requests;

namespace TravelEase.Application.BookingManagement.Validators
{
    public class GetAllBookingsByHotelIdQueryValidator : AbstractValidator<BookingQueryRequest>
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