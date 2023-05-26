﻿using Plantt.Domain.DTOs.Account;

namespace Plantt.Domain.Interfaces.Services
{
    public interface IPasswordService
    {
        /// <summary>
        /// Creates a PasswordDTO object with the hashed password, salt, and iterations based on the provided password.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>A PasswordDTO object containing the hashed password, salt, and iterations.</returns>
        PasswordDTO CreatePassword(string password);

        /// <summary>
        /// Verifies if the provided password matches the hashed password stored in the PasswordDTO object.
        /// </summary>
        /// <param name="password">The password to be verified.</param>
        /// <param name="hashedPassword">The PasswordDTO object containing the hashed password, salt, and iterations.</param>
        /// <returns>True if the provided password matches the hashed password; otherwise, false.</returns>
        bool VerifyPassword(string password, PasswordDTO hashedPassword);
    }
}