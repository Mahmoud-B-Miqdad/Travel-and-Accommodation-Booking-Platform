using System.Security.Claims;

namespace TravelEase.API.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetEmailOrThrow(this ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
                throw new UnauthorizedAccessException("Unauthorized access.");

            return email;
        }

        public static string GetFullNameOrEmpty(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        }
    }
}