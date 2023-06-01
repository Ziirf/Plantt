using Microsoft.Extensions.Options;
using Plantt.Domain.Config;
using Plantt.Domain.DTOs.Account;
using Plantt.Domain.Interfaces.Services;
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

        public PasswordDTO CreatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            // Generates the required components for the password.
            int iterations = GenerateIterationFromRange(_passwordSettings.MinIterations, _passwordSettings.MaxIterations);
            byte[] salt = GenerateSalt(_passwordSettings.SaltSize);
            byte[] hashedPassword = HashPassword(password, salt, iterations, _passwordSettings.PasswordHashSize);

            return new PasswordDTO()
            {
                HashedPassword = hashedPassword,
                Salt = salt,
                Iterations = iterations
            };
        }

        public bool VerifyPassword(string password, PasswordDTO hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || hashedPassword.HashedPassword is null)
            {
                return false;
            }

            byte[] recreatedPassword = HashPassword(password, hashedPassword.Salt, hashedPassword.Iterations, _passwordSettings.PasswordHashSize);

            // Check the two values against each other, in a way where an attacker can't read anything from the timing of it.
            return CryptographicOperations.FixedTimeEquals(hashedPassword.HashedPassword, recreatedPassword);
        }

        private static byte[] GenerateSalt(int saltSize)
        {
            // Create a byte array as salt
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
            // hash the password with Rfc2898 using the SHA512 algorytm.
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA512))
            {
                return deriveBytes.GetBytes(hashSize);
            }
        }
    }
}
