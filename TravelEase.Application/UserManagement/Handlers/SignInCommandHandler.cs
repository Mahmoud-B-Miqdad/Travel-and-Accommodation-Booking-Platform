using MediatR;
using System.Security.Claims;
using TravelEase.Application.UserManagement.Commands;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Application.UserManagement.Handlers
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;

        public SignInCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await GetUserOrThrowAsync(request.Email);
            ValidatePassword(request.Password, user.PasswordHash, user.Salt);

            var claims = GenerateClaims(user);
            return await _tokenGenerator.GenerateToken(claims);
        }

        private async Task<User> GetUserOrThrowAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials.");
            return user;
        }

        private void ValidatePassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var isValid = _passwordHasher.VerifyPassword(password, storedHash, saltBytes);

            if (!isValid)
                throw new UnauthorizedAccessException("Invalid credentials.");
        }

        private static List<Claim> GenerateClaims(Domain.Aggregates.Users.User user) =>
            new()
            {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role.ToString())
            };
    }
}