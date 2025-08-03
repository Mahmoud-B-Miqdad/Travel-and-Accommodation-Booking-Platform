using MediatR;
using TravelEase.Domain.Enums;

namespace TravelEase.Application.PaymentManagement.Commands
{
    public record CreatePaymentIntentCommand : IRequest<string>
    {
        public Guid BookingId { get; init; }
        public double Amount { get; init; }
        public string GuestEmail { get; init; }
        public PaymentMethod Method { get; init; }
    }
}