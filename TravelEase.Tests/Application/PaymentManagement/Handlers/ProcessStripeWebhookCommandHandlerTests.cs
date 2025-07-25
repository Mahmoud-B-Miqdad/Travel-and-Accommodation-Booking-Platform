using AutoFixture.AutoMoq;
using AutoFixture;
using FluentAssertions;
using Moq;
using Stripe;
using TravelEase.Application.PaymentManagement.Commands;
using TravelEase.Application.PaymentManagement.Handlers;
using TravelEase.Domain.Aggregates.Payments;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Enums;

namespace TravelEase.Tests.Application.PaymentManagement.Handlers
{
    public class ProcessStripeWebhookCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IPaymentRepository> _paymentRepoMock = new();
        private readonly ProcessStripeWebhookCommandHandler _handler;
        private readonly Fixture _fixture;

        public ProcessStripeWebhookCommandHandlerTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
            _fixture.Behaviors.Remove(_fixture.Behaviors.OfType<ThrowingRecursionBehavior>().Single());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _paymentRepoMock = new Mock<IPaymentRepository>();
            _unitOfWorkMock.Setup(u => u.Payments).Returns(_paymentRepoMock.Object);

            _handler = new ProcessStripeWebhookCommandHandler(_unitOfWorkMock.Object);
        }

        [Theory]
        [InlineData("payment_intent.succeeded", PaymentStatus.Completed)]
        [InlineData("payment_intent.payment_failed", PaymentStatus.Cancelled)]
        public async Task Handle_ShouldUpdatePaymentStatus_ForPaymentIntentEvents
            (string eventType, PaymentStatus expectedStatus)
        {
            var bookingId = Guid.NewGuid();

            var metadata = new Dictionary<string, string>
            {
                { "BookingId", bookingId.ToString() }
            };

            var paymentIntent = new PaymentIntent
            {
                Metadata = metadata
            };

            var stripeEvent = new Event
            {
                Type = eventType,
                Data = new EventData { Object = paymentIntent }
            };

            var payment = _fixture.Create<Payment>();

            _paymentRepoMock.Setup(r => r.GetByBookingIdAsync(bookingId)).ReturnsAsync(payment);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);
            var command = new ProcessStripeWebhookCommand
            {
                StripeEvent = stripeEvent
            };

            await _handler.Handle(command, default);

            payment.Status.Should().Be(expectedStatus);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldUpdatePaymentStatus_ForChargeRefundedEvent()
        {
            var paymentIntentId = "pi_test_123";

            var charge = new Charge
            {
                PaymentIntentId = paymentIntentId
            };

            var stripeEvent = new Event
            {
                Type = "charge.refunded",
                Data = new EventData { Object = charge }
            };

            var payment = _fixture.Create<Payment>();

            _paymentRepoMock.Setup(r => r.GetByPaymentIntentIdAsync(paymentIntentId)).ReturnsAsync(payment);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);
            var command = new ProcessStripeWebhookCommand
            {
                StripeEvent = stripeEvent
            };

            await _handler.Handle(command, default);

            payment.Status.Should().Be(PaymentStatus.Refunded);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotThrow_WhenPaymentIsNull()
        {
            var bookingId = Guid.NewGuid();
            var metadata = new Dictionary<string, string>
            {
                { "BookingId", bookingId.ToString() }
            };

            var paymentIntent = new PaymentIntent
            {
                Metadata = metadata
            };

            var stripeEvent = new Event
            {
                Type = "payment_intent.succeeded",
                Data = new EventData { Object = paymentIntent }
            };

            _paymentRepoMock.Setup(r => r.GetByBookingIdAsync(bookingId)).ReturnsAsync((Payment?)null);

            var command = new ProcessStripeWebhookCommand
            {
                StripeEvent = stripeEvent
            };

            var act = async () => await _handler.Handle(command, default);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Handle_ShouldDoNothing_ForUnhandledEventTypes()
        {
            var stripeEvent = new Event
            {
                Type = "unhandled.event",
                Data = new EventData { Object = null! }
            };

            var command = new ProcessStripeWebhookCommand
            {
                StripeEvent = stripeEvent
            };

            await _handler.Handle(command, default);

            _paymentRepoMock.Verify(r => r.GetByBookingIdAsync(It.IsAny<Guid>()), Times.Never);
            _paymentRepoMock.Verify(r => r.GetByPaymentIntentIdAsync(It.IsAny<string>()), Times.Never);
        }
    }
}