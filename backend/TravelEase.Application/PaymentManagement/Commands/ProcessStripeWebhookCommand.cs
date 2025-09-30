using MediatR;
using Stripe;

namespace TravelEase.Application.PaymentManagement.Commands
{
    public record ProcessStripeWebhookCommand : IRequest
    {
        public Event StripeEvent { get; init; }
    }
}