using MediatR;
using TravelEase.Domain.Enums;

namespace TravelEase.Application.PaymentManagement.Commands
{
    public record CreatePaymentIntentCommand : IRequest<string>
    {
        public Guid BookingId { get; set; }
        public string Amount { get; set; }
        public string GuestEmail { get; set; }
        public PaymentMethod Method { get; set; }
    }
}