using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Isopoh.Cryptography.Argon2;
using System.Text;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Infrastructure.Common.Security
{
    public class Argon2PasswordHasher : IPasswordHasher
    {
        private readonly int _saltSize;
        private readonly IConfiguration _configuration;

        public Argon2PasswordHasher(IConfiguration configuration)
        {
            _configuration = configuration;
            _saltSize = int.Parse(_configuration["PasswordHasher:SaltSize"]);
        }

        public byte[] GenerateSalt()
        {
            var salt = new byte[_saltSize];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        public string? GenerateHashedPassword(string password, byte[] salt)
        {
            if (salt.Length < 8)
                throw new InvalidOperationException("Short Salt Value, Enter 8 Bytes Or More ");

            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var config = new Argon2Config
            {
                Type = Argon2Type.DataIndependentAddressing,
                Version = Argon2Version.Nineteen,
                TimeCost = int.Parse(_configuration["PasswordHasher:TimeCost"]),
                Threads = Environment.ProcessorCount,
                Password = passwordBytes,
                Salt = salt,
                Secret = Encoding.UTF8.GetBytes(_configuration["PasswordHasher:Secret"]),
                HashLength = int.Parse(_configuration["PasswordHasher:HashLength"])
            };

            var argon2 = new Argon2(config);
            using var hashA = argon2.Hash();
            var hashString = config.EncodeString(hashA.Buffer);

            return hashString;
        }
    

        public bool VerifyPassword(string userPassword, string hashedPassword, byte[] salt)
        {
                return hashedPassword.Equals(GenerateHashedPassword(userPassword, salt));
        }
    }
}