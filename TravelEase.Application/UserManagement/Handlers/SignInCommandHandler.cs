using MediatR;
using System.Security.Claims;
using TravelEase.Application.UserManagement.Commands;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Application.UserManagement.Handlers
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;

        public SignInCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user is null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var isValid = _passwordHasher.VerifyPassword(
                request.Password, user.PasswordHash, Convert.FromBase64String(user.Salt));

            if (!isValid)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Role, user.Role.ToString())
        };

            return await _tokenGenerator.GenerateToken(claims);
        }
    }
}