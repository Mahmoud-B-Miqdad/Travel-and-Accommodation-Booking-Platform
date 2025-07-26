using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Payments;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Domain.Enums;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.EntityPersistence.PaymentPersistence;

namespace TravelEase.Tests.Infrastructure.IntegrationTests.Repositories
{
    public class PaymentRepositoryTests
    {
        private TravelEaseDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TravelEaseDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TravelEaseDbContext(options);
        }

        [Fact]
        public async Task GetPaymentsByUserIdAsync_ShouldReturnPayments_WhenPaymentsExist()
        {
            using var context = CreateDbContext();

            var user = CreateValidUser();
            var booking = CreateValidBooking(user.Id);
            var payment = CreateValidPayment(booking.Id);

            await context.AddRangeAsync(user, booking, payment);
            await context.SaveChangesAsync();

            var repo = new PaymentRepository(context);

            var result = await repo.GetPaymentsByUserIdAsync(user.Id);

            result.Should().HaveCount(1);
            result[0].Id.Should().Be(payment.Id);
        }

        [Fact]
        public async Task GetByBookingIdAsync_ShouldReturnPayment_WhenExists()
        {
            using var context = CreateDbContext();

            var booking = CreateValidBooking(Guid.NewGuid());
            var payment = CreateValidPayment(booking.Id);

            await context.AddRangeAsync(booking, payment);
            await context.SaveChangesAsync();

            var repo = new PaymentRepository(context);

            var result = await repo.GetByBookingIdAsync(booking.Id);

            result.Should().NotBeNull();
            result.Id.Should().Be(payment.Id);
        }

        [Fact]
        public async Task GetByPaymentIntentIdAsync_ShouldReturnPayment_WhenExists()
        {
            using var context = CreateDbContext();

            var booking = CreateValidBooking(Guid.NewGuid());
            var payment = CreateValidPayment(booking.Id);

            await context.AddRangeAsync(booking, payment);
            await context.SaveChangesAsync();

            var repo = new PaymentRepository(context);

            var result = await repo.GetByPaymentIntentIdAsync(payment.PaymentIntentId);

            result.Should().NotBeNull();
            result.Id.Should().Be(payment.Id);
        }

        #region Helper Methods

        private User CreateValidUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashed",
                PhoneNumber = "1234567890",
                Salt = "salt"
            };
        }

        private Booking CreateValidBooking(Guid userId)
        {
            return new Booking
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoomId = Guid.NewGuid(),
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(3),
                BookingDate = DateTime.Today,
                Price = 300
            };
        }

        private Payment CreateValidPayment(Guid bookingId)
        {
            return new Payment
            {
                Id = Guid.NewGuid(),
                BookingId = bookingId,
                Amount = 300,
                Status = PaymentStatus.Completed,
                Method = PaymentMethod.CreditCard,
                PaymentIntentId = Guid.NewGuid().ToString()
            };
        }
        #endregion
    }
}