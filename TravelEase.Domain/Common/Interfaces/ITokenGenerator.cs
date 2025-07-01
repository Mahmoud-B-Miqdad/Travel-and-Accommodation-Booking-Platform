using TravelEase.Domain.Aggregates.Users;

namespace TravelEase.Domain.Common.Interfaces
{
    public interface ITokenGenerator
    {
        public Task<string> GenerateToken(
            string email,
            string password,
            string secretKey,
            string issuer,
            string audience);
    }
}