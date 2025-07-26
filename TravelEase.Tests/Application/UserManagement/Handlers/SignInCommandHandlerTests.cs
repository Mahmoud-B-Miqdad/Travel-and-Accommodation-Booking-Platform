using FluentAssertions;
using Moq;
using System.Security.Claims;
using TravelEase.Application.UserManagement.Commands;
using TravelEase.Application.UserManagement.Handlers;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Enums;

namespace TravelEase.Tests.Application.UserManagement.Handlers
{
    public class SignInCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<ITokenGenerator> _tokenGeneratorMock = new();
        private readonly SignInCommandHandler _handler;

        public SignInCommandHandlerTests()
        {
            _handler = new SignInCommandHandler(
                _unitOfWorkMock.Object,
                _passwordHasherMock.Object,
                _tokenGeneratorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
        {
            var command = new SignInCommand { Email = "notfound@example.com", Password = "pass" };
            _unitOfWorkMock.Setup(x => x.Users.GetByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Invalid Email or Password.");
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenPasswordIsInvalid()
        {
            var command = new SignInCommand { Email = "test@example.com", Password = "wrongpass" };
            var user = new User
            {
                Email = command.Email,
                Salt = Convert.ToBase64String(new byte[] { 1, 2, 3 }),
                PasswordHash = "hashed_pass"
            };

            _unitOfWorkMock.Setup(x => x.Users.GetByEmailAsync(command.Email))
                .ReturnsAsync(user);

            _passwordHasherMock.Setup(x =>
                x.VerifyPassword(command.Password, user.PasswordHash, It.IsAny<byte[]>()))
                .Returns(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Invalid Email or Password.");
        }

        [Fact]
        public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var command = new SignInCommand
            {
                Email = "user@example.com",
                Password = "correctpass"
            };

            var user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = command.Email,
                Role = UserRole.Guest,
                Salt = Convert.ToBase64String(new byte[] { 5, 6, 7 }),
                PasswordHash = "hashed_pass"
            };

            _unitOfWorkMock.Setup(x => x.Users.GetByEmailAsync(command.Email))
                .ReturnsAsync(user);

            _passwordHasherMock.Setup(x =>
                x.VerifyPassword(command.Password, user.PasswordHash, It.IsAny<byte[]>()))
                .Returns(true);

            _tokenGeneratorMock.Setup(x => x.GenerateToken(It.IsAny<List<Claim>>()))
                .ReturnsAsync("mock_token");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().Be("mock_token");

            _tokenGeneratorMock.Verify(x =>
                x.GenerateToken(It.Is<List<Claim>>(claims =>
                    claims.Exists(c => c.Type == ClaimTypes.Email && c.Value == user.Email) &&

                    claims.Exists(c => c.Type == ClaimTypes.Name && c.Value == 
                    $"{user.FirstName} {user.LastName}") &&

                    claims.Exists(c => c.Type == ClaimTypes.Role && c.Value == user.Role.ToString())
                )), Times.Once);
        }
    }
}