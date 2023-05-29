using Plantt.Domain.DTOs.Account.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Enums;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IAccountService
    {
        /// <summary>
        /// Retrieves an account entity asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the account to retrieve.</param>
        /// <returns>
        /// The <see cref="AccountEntity"/> object representing the account, 
        /// or null if no account with the specified ID is found.
        /// </returns>
        Task<AccountEntity?> GetAccountByIdAsync(int id);

        /// <summary>
        /// Retrieves an account entity asynchronously by its username.
        /// </summary>
        /// <param name="username">The username of the account to retrieve.</param>
        /// <returns>
        /// The <see cref="AccountEntity"/> object representing the account,
        /// or null if no account with the specified username is found.
        /// </returns>
        Task<AccountEntity?> GetAccountByUsernameAsync(string username);

        /// <summary>
        /// Creates a new account asynchronously with the provided account information.
        /// </summary>
        /// <param name="accountRequest">The <see cref="CreateAccountRequest"/> object containing the account information.</param>
        /// <returns>The newly created <see cref="AccountEntity"/> object.</returns>
        Task<AccountEntity> CreateAccountAsync(CreateAccountRequest accountRequest);

        /// <summary>
        /// Changes the role of an account asynchronously. (Only used for testing)
        /// </summary>
        /// <param name="account">The <see cref="AccountEntity"/> object representing the account to update.</param>
        /// <param name="role">The new role to assign to the account.</param>
        /// <returns>The updated <see cref="AccountEntity"/> object.</returns>
        Task<AccountEntity?> ChangeAccountRoleAsync(AccountEntity account, AccountRoles role);

        /// <summary>
        /// Deletes an account asynchronously.
        /// </summary>
        /// <param name="account">The <see cref="AccountEntity"/> object representing the account to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAccount(AccountEntity account);

        /// <summary>
        /// Verifies if the provided password matches the stored password for an account.
        /// </summary>
        /// <param name="account">The <see cref="AccountEntity"/> object representing the account.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns>True if the provided password matches the stored password, false otherwise.</returns>
        bool VerifyPassword(AccountEntity account, string password);
    }
}