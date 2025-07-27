using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.EntityPersistence.UserPersistence;

namespace TravelEase.Tests.Infrastructure.IntegrationTests.Repositories
{
    public class UserRepositoryTests
    {
        private TravelEaseDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TravelEaseDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TravelEaseDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnUsersWithBookings_WhenIncludeBookingsIsTrue()
        {
            using var context = CreateDbContext();

            var user = CreateValidUser();
            var booking = CreateValidBooking(user.Id);

            await context.AddRangeAsync(user, booking);
            await context.SaveChangesAsync();

            var repo = new UserRepository(context);

            var result = await repo.GetAllAsync(includeBookings: true, pageNumber: 1, pageSize: 10);

            result.Items.Should().ContainSingle(u => u.Id == user.Id);
            result.Items.First().Bookings.Should().ContainSingle(b => b.Id == booking.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnUsersWithoutBookings_WhenIncludeBookingsIsFalse()
        {
            using var context = CreateDbContext();

            var user = CreateValidUser();
            var booking = CreateValidBooking(user.Id);

            await context.AddRangeAsync(user, booking);
            await context.SaveChangesAsync();

            var repo = new UserRepository(context);

            var result = await repo.GetAllAsync(includeBookings: false, pageNumber: 1, pageSize: 10);

            result.Items.Should().ContainSingle(u => u.Id == user.Id);
            result.Items.First().Bookings.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnUser_WhenEmailExists()
        {
            using var context = CreateDbContext();

            var user = CreateValidUser();

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var repo = new UserRepository(context);

            var result = await repo.GetByEmailAsync(user.Email);

            result.Should().NotBeNull();
            result.Id.Should().Be(user.Id);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
        {
            using var context = CreateDbContext();

            var repo = new UserRepository(context);

            var result = await repo.GetByEmailAsync("notfound@example.com");

            result.Should().BeNull();
        }

        #region Helpers

        private User CreateValidUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashed",
                PhoneNumber = "123456789",
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
                CheckOutDate = DateTime.Today.AddDays(2),
                BookingDate = DateTime.Today,
                Price = 100
            };
        }

        #endregion
    }
}