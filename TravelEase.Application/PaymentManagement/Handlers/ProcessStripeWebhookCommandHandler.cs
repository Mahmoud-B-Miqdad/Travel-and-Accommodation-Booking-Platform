using MediatR;
using Stripe;
using TravelEase.Application.PaymentManagement.Commands;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Enums;

namespace TravelEase.Application.PaymentManagement.Handlers
{
    public class ProcessStripeWebhookCommandHandler : IRequestHandler<ProcessStripeWebhookCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProcessStripeWebhookCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProcessStripeWebhookCommand request, CancellationToken cancellationToken)
        {
            var stripeEvent = request.StripeEvent;

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var intent = (PaymentIntent)stripeEvent.Data.Object;
                var bookingId = Guid.Parse(intent.Metadata["BookingId"]);

                var payment = await _unitOfWork.Payments.GetByBookingIdAsync(bookingId);
                if (payment is not null)
                {
                    payment.Status = PaymentStatus.Completed;
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            else if (stripeEvent.Type == "payment_intent.payment_failed")
            {
                var intent = (PaymentIntent)stripeEvent.Data.Object;
                var bookingId = Guid.Parse(intent.Metadata["BookingId"]);

                var payment = await _unitOfWork.Payments.GetByBookingIdAsync(bookingId);
                if (payment is not null)
                {
                    payment.Status = PaymentStatus.Cancelled;
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            else if (stripeEvent.Type == "charge.refunded")
            {
                var charge = (Charge)stripeEvent.Data.Object;

                var paymentIntentId = charge.PaymentIntentId;

                var payment = await _unitOfWork.Payments.GetByPaymentIntentIdAsync(paymentIntentId);

                if (payment is not null)
                {
                    payment.Status = PaymentStatus.Refunded;
                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }
    }
}