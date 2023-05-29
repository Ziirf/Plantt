using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IAccountRepository : IRepository<AccountEntity>
    {
        /// <summary>
        /// Retrieves an account entity asynchronously by username.
        /// </summary>
        /// <param name="username">The username of the account to retrieve.</param>
        /// <returns>
        /// The <see cref="AccountEntity"/> object representing the account,
        /// or null if 
        Task<AccountEntity?> GetByUsernameAsync(string username);

        /// <summary>
        /// Retrieves an account entity asynchronously by GUID.
        /// </summary>
        /// <param name="guid">The GUID of the account to retrieve.</param>
        /// <returns>
        /// The <see cref="AccountEntity"/> object representing the account,
        /// or null if no account with the specified GUID is found.
        /// </returns>
        Task<AccountEntity?> GetByGuidAsync(Guid guid);

        /// <summary>
        /// Checks if a username exists in the accounts.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>
        /// True if the username exists in the accounts, false otherwise.
        /// </returns>
        bool DoesUsernameExist(string username);
    }
}
