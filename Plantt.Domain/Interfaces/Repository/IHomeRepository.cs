using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IHomeRepository : IRepository<HomeEntity>
    {
        /// <summary>
        /// Retrieves the homes associated with an account.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>An enumerable collection of <see cref="HomeEntity"/> objects representing the homes associated with the account.</returns>
        IEnumerable<HomeEntity> GetAccountHomes(int accountId);

        /// <summary>
        /// Validates if an account is the owner of a home.
        /// </summary>
        /// <param name="homeId">The ID of the home to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>True if the account is the owner of the home, false otherwise.</returns>
        bool IsValidOwner(int homeId, int accountId);

        /// <summary>
        /// Validates asynchronously if an account is the owner of a home.
        /// </summary>
        /// <param name="homeId">The ID of the home to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>True if the account is the owner of the home, false otherwise.</returns>
        Task<bool> IsValidOwnerAsync(int homeId, int accountId);

    }
}
