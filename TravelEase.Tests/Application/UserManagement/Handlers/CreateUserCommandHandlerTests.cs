using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.UserManagement.Commands;
using TravelEase.Application.UserManagement.Handlers;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.UserManagement.Handlers
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandHandlerTests()
        {
            _handler = new CreateUserCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _passwordHasherMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenEmailAlreadyExists()
        {
            // Arrange
            var command = new CreateUserCommand { Email = "test@example.com" };
            _unitOfWorkMock.Setup(x => x.Users.GetByEmailAsync(command.Email))
                .ReturnsAsync(new User());

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage("User with email 'test@example.com' already exists.");
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenPasswordHashIsNull()
        {
            var command = new CreateUserCommand
            {
                Email = "new@example.com",
                Password = "P@ssword"
            };

            _unitOfWorkMock.Setup(x => x.Users.GetByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            var user = new User();
            _mapperMock.Setup(x => x.Map<User>(command))
                .Returns(user);

            _passwordHasherMock.Setup(x => x.GenerateSalt())
                .Returns(new byte[] { 1, 2, 3 });

            _passwordHasherMock.Setup(x =>
                    x.GenerateHashedPassword(command.Password, It.IsAny<byte[]>()))
                .Returns<string?>(null); // simulate hashing failure

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Can't hash user password.");
        }

        [Fact]
        public async Task Handle_ShouldCreateUserSuccessfully_WhenInputIsValid()
        {
            var command = new CreateUserCommand
            {
                Email = "new@example.com",
                Password = "StrongP@ss"
            };

            _unitOfWorkMock.Setup(x => x.Users.GetByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            var user = new User();
            _mapperMock.Setup(x => x.Map<User>(command))
                .Returns(user);

            byte[] salt = new byte[] { 1, 2, 3 };
            string hashedPassword = "hashed_password";

            _passwordHasherMock.Setup(x => x.GenerateSalt())
                .Returns(salt);

            _passwordHasherMock.Setup(x => x.GenerateHashedPassword(command.Password, salt))
                .Returns(hashedPassword);

            await _handler.Handle(command, CancellationToken.None);

            user.Salt.Should().Be(Convert.ToBase64String(salt));
            user.PasswordHash.Should().Be(hashedPassword);
            user.Role.Should().Be(UserRole.Guest);

            _unitOfWorkMock.Verify(x => x.Users.AddAsync(user), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}