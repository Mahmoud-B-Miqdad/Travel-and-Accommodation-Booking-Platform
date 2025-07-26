using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using Stripe;
using TravelEase.Application.PaymentManagement.Commands;
using TravelEase.Application.PaymentManagement.Handlers;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Payments;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.UnitTests.PaymentManagement.Handlers
{
    public class CreatePaymentIntentCommandHandlerTests
    {
        private readonly Mock<IPaymentService> _paymentServiceMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IPaymentRepository> _paymentRepoMock = new();
        private readonly Mock<IBookingRepository> _bookingRepoMock = new();
        private readonly CreatePaymentIntentCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public CreatePaymentIntentCommandHandlerTests()
        {
            _fixture.Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWorkMock.Setup(u => u.Payments).Returns(_paymentRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Bookings).Returns(_bookingRepoMock.Object);

            _handler = new CreatePaymentIntentCommandHandler(
                _paymentServiceMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreatePaymentIntentAndPersistPayment_WhenValid()
        {
            var command = _fixture.Create<CreatePaymentIntentCommand>();
            var createdPayment = _fixture.Create<Payment>();
            var booking = _fixture.Create<Booking>();

            var paymentIntent = new PaymentIntent
            {
                Id = "pi_test_123",
                ClientSecret = "secret_test_123"
            };

            _bookingRepoMock.Setup(r => r.GetByIdAsync(command.BookingId)).ReturnsAsync(booking);
            _bookingRepoMock.Setup(r => r.IsBookingAccessibleToUserAsync(command.BookingId, command.GuestEmail))
                .ReturnsAsync(true);

            _paymentServiceMock.Setup(s =>
            s.CreatePaymentIntentAsync(command.BookingId, command.Amount, command.Method, "usd"))
            .ReturnsAsync(paymentIntent);


            _paymentRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Payment>())).ReturnsAsync(createdPayment);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().Be(paymentIntent.ClientSecret);
            _paymentRepoMock.Verify(r => r.AddAsync(It.Is<Payment>(p =>
                p.BookingId == command.BookingId &&
                p.Amount == command.Amount &&
                p.Method == command.Method &&
                p.Status == PaymentStatus.Pending &&
                p.PaymentIntentId == paymentIntent.Id
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenBookingDoesNotExist()
        {
            var command = _fixture.Create<CreatePaymentIntentCommand>();
            _bookingRepoMock.Setup(r => r.GetByIdAsync(command.BookingId)).ReturnsAsync((Booking?)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Booking with ID {command.BookingId} was not found.");

            _paymentRepoMock.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserCannotAccessBooking()
        {
            var command = _fixture.Create<CreatePaymentIntentCommand>();
            var booking = _fixture.Create<Booking>();

            _bookingRepoMock.Setup(r => r.GetByIdAsync(command.BookingId)).ReturnsAsync(booking);
            _bookingRepoMock.Setup(r => r.IsBookingAccessibleToUserAsync(command.BookingId, command.GuestEmail))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("You are not authorized to access this booking.");

            _paymentRepoMock.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Never);
        }
    }
}