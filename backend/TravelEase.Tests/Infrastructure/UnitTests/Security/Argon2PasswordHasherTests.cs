using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.Text;
using TravelEase.Infrastructure.Common.Security;

namespace TravelEase.Tests.Infrastructure.UnitTests.Security
{
    public class Argon2PasswordHasherTests
    {
        private readonly Argon2PasswordHasher _hasher;

        public Argon2PasswordHasherTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "PasswordHasher:SaltSize", "16" },
                    { "PasswordHasher:TimeCost", "4" },
                    { "PasswordHasher:Secret", "test-secret" },
                    { "PasswordHasher:HashLength", "32" },
                })
                .Build();

            _hasher = new Argon2PasswordHasher(configuration);
        }

        [Fact]
        public void GenerateSalt_ShouldReturnCorrectLength()
        {
            var salt = _hasher.GenerateSalt();
            salt.Should().HaveCount(16);
        }

        [Fact]
        public void GenerateHashedPassword_ShouldReturnConsistentResult()
        {
            var salt = _hasher.GenerateSalt();
            var hash1 = _hasher.GenerateHashedPassword("Test123!", salt);
            var hash2 = _hasher.GenerateHashedPassword("Test123!", salt);

            hash1.Should().Be(hash2);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
        {
            var salt = _hasher.GenerateSalt();
            var hash = _hasher.GenerateHashedPassword("ValidPassword123", salt);

            var result = _hasher.VerifyPassword("ValidPassword123", hash!, salt);
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
        {
            var salt = _hasher.GenerateSalt();
            var hash = _hasher.GenerateHashedPassword("ValidPassword123", salt);

            var result = _hasher.VerifyPassword("WrongPassword", hash!, salt);
            result.Should().BeFalse();
        }

        [Fact]
        public void GenerateHashedPassword_ShouldThrowException_ForShortSalt()
        {
            var shortSalt = Encoding.UTF8.GetBytes("123");

            var act = () => _hasher.GenerateHashedPassword("AnyPassword", shortSalt);

            act.Should().Throw<InvalidOperationException>();
        }
    }
}