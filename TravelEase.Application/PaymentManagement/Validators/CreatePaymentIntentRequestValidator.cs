using FluentValidation;
using TravelEase.Application.PaymentManagement.Requests;
using TravelEase.Domain.Enums;

namespace TravelEase.Application.PaymentManagement.Validators
{
    public class CreatePaymentIntentRequestValidator : AbstractValidator<CreatePaymentIntentRequest>
    {
        public CreatePaymentIntentRequestValidator()
        {
            RuleFor(x => x.BookingId)
                .NotEmpty()
                .WithMessage("Booking ID is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Method)
                .IsInEnum()
                .WithMessage("Invalid payment method.")
                .Must(method => method is not PaymentMethod.None and not PaymentMethod.Cash)
                .WithMessage("This payment method is not supported via Stripe.");
        }
    }
}