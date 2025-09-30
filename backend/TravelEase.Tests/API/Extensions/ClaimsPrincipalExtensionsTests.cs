using FluentAssertions;
using System.Security.Claims;
using TravelEase.API.Common.Extensions;

namespace TravelEase.Tests.API.Extensions
{
    public class ClaimsPrincipalExtensionsTests
    {
        [Fact]
        public void GetEmailOrThrow_ShouldReturnEmail_WhenEmailExists()
        {
            var claims = new[] { new Claim(ClaimTypes.Email, "test@example.com") };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            var email = principal.GetEmailOrThrow();

            email.Should().Be("test@example.com");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void GetEmailOrThrow_ShouldThrow_WhenEmailMissingOrEmpty(string emailValue)
        {
            var claims = emailValue != null ? new[] { new Claim(ClaimTypes.Email, emailValue) }
            : new Claim[] { };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            Action act = () => principal.GetEmailOrThrow();

            act.Should().Throw<UnauthorizedAccessException>().WithMessage("Unauthorized access.");
        }

        [Fact]
        public void GetFullNameOrEmpty_ShouldReturnName_WhenNameExists()
        {
            var claims = new[] { new Claim(ClaimTypes.Name, "John Doe") };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            var name = principal.GetFullNameOrEmpty();

            name.Should().Be("John Doe");
        }

        [Fact]
        public void GetFullNameOrEmpty_ShouldReturnEmpty_WhenNameMissing()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity());

            var name = principal.GetFullNameOrEmpty();

            name.Should().BeEmpty();
        }
    }
}