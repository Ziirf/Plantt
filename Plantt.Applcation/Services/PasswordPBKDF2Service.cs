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

        /// <summary>
        /// Creates a PasswordDTO object with the hashed password, salt, and iterations based on the provided password.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>A PasswordDTO object containing the hashed password, salt, and iterations.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the password parameter is null or empty.</exception>
        public PasswordDTO CreatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

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

        /// <summary>
        /// Verifies if the provided password matches the hashed password stored in the PasswordDTO object.
        /// </summary>
        /// <param name="password">The password to be verified.</param>
        /// <param name="hashedPassword">The PasswordDTO object containing the hashed password, salt, and iterations.</param>
        /// <returns>True if the provided password matches the hashed password; otherwise, false.</returns>
        public bool VerifyPassword(string password, PasswordDTO hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || hashedPassword.HashedPassword is null)
            {
                return false;
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
