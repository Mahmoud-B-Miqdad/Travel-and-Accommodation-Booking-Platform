using FluentValidation;
using TravelEase.Application.RoomTypeManagement.DTOs.Requests;

namespace TravelEase.API.Validators.RoomTypeValidators
{
    public class CreateRoomTypeRequestValidator : AbstractValidator<RoomTypeForCreationRequest>
    {
        public CreateRoomTypeRequestValidator()
        {
            RuleFor(x => x.Category)
                .IsInEnum()
                .WithMessage("Invalid room category.");

            RuleFor(x => x.PricePerNight)
                .GreaterThan(0)
                .WithMessage("Price per night must be greater than zero.");

            RuleFor(x => x.AmenityIds)
                .NotNull()
                .WithMessage("Amenity IDs list must not be null.");
        }
    }
}