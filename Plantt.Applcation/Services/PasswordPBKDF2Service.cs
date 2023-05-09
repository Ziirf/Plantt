using Microsoft.Extensions.Options;
using Plantt.Domain.Config;
using Plantt.Domain.Interfaces.Services;
using Plantt.Domain.Models;
using System.Security.Cryptography;

namespace Plantt.Applcation.Services
{
    public class PasswordPBKDF2Service : IPasswordService
    {
        private readonly PasswordSettings _passwordSettings;

        public PasswordPBKDF2Service(IOptions<PasswordSettings> passwordSettings)
        {
            _passwordSettings = passwordSettings.Value;
        }

        public Password CreatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            int iterations = GenerateIterationFromRange(_passwordSettings.MinIterations, _passwordSettings.MaxIterations);
            byte[] salt = GenerateSalt(_passwordSettings.SaltSize);
            byte[] hashedPassword = HashPassword(password, salt, iterations, _passwordSettings.PasswordHashSize);

            return new Password()
            {
                HashedPassword = hashedPassword,
                Salt = salt,
                Iterations = iterations
            };
        }

        public bool VerifyPassword(string password, Password hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || hashedPassword.HashedPassword is null)
            {
                throw new ArgumentNullException("Either password or hashedPassword was null or empty");
            }

            byte[] recreatedPassword = HashPassword(password, hashedPassword.Salt, hashedPassword.Iterations, _passwordSettings.PasswordHashSize);

            return CryptographicOperations.FixedTimeEquals(hashedPassword.HashedPassword, recreatedPassword);
        }

        private static byte[] GenerateSalt(int saltSize)
        {
            byte[] salt = new byte[saltSize];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }
            return salt;
        }

        private static int GenerateIterationFromRange(int minValue, int maxValue)
        {
            var random = new Random();
            return random.Next(minValue, maxValue + 1);
        }

        private static byte[] HashPassword(string password, byte[] salt, int iterations, int hashSize)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA512))
            {
                return deriveBytes.GetBytes(hashSize);
            }
        }
    }
}
