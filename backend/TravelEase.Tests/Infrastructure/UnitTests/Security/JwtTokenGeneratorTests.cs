using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using TravelEase.Infrastructure.Common.Security;

namespace TravelEase.Tests.Infrastructure.UnitTests.Security
{
    public class JwtTokenGeneratorTests
    {
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IConfiguration _configuration;
        private readonly Fixture _fixture;

        public JwtTokenGeneratorTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
            {"Authentication:SecretForKey", "supersecretkey_supersecretkey!123"},
            {"Authentication:Issuer", "TestIssuer"},
            {"Authentication:Audience", "TestAudience"}
        };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _jwtTokenGenerator = new JwtTokenGenerator(_configuration);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GenerateToken_ShouldReturnValidJwtToken_WhenCalledWithClaims()
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _fixture.Create<string>()),
            new Claim(ClaimTypes.Email, _fixture.Create<string>())
        };

            var token = await _jwtTokenGenerator.GenerateToken(claims);

            token.Should().NotBeNullOrEmpty();

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            handler.CanReadToken(token).Should().BeTrue();

            var jwtToken = handler.ReadJwtToken(token);
            jwtToken.Issuer.Should().Be("TestIssuer");
            jwtToken.Audiences.Should().Contain("TestAudience");

            foreach (var claim in claims)
            {
                jwtToken.Claims.Should().Contain(c => c.Type == claim.Type && c.Value == claim.Value);
            }

            jwtToken.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddHours(1), precision: TimeSpan.FromMinutes(1));
        }
    }
}