using FluentValidation;
using TravelEase.Application.RoomManagement.Queries;

namespace TravelEase.API.Validators.RoomValidators
{
    public class GetHotelAvailableRoomsQueryValidator : AbstractValidator<GetHotelAvailableRoomsQuery>
    {
        public GetHotelAvailableRoomsQueryValidator()
        {
            RuleFor(x => x.HotelId)
                .NotEmpty().WithMessage("HotelId is required.");

            RuleFor(x => x.CheckInDate)
                .NotEmpty().WithMessage("Check-in date is required.")
                .Must(BeAValidDate).WithMessage("Check-in date must be a valid future date.");

            RuleFor(x => x.CheckOutDate)
                .NotEmpty().WithMessage("Check-out date is required.")
                .GreaterThan(x => x.CheckInDate)
                .WithMessage("Check-out date must be after check-in date.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return date > DateTime.UtcNow.Date;
        }
    }
}