using System.Security.Claims;

namespace TravelEase.Domain.Common.Interfaces
{
    public interface ITokenGenerator
    {
        Task<string> GenerateToken(List<Claim> claims);
    }
}