using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Infrastructure.Common.Security
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<string> GenerateToken(string email, string password, string secretKey,
            string issuer, string audience)
        {
            var user = await ValidateCredentialsAsync(email, password);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User?> ValidateCredentialsAsync(string email, string password)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user is null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var isPasswordValid = _passwordHasher.VerifyPassword(password, user.PasswordHash, Convert.FromBase64String(user.Salt));
            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid credentials.");

            return user;
        }
    }
}