using Plantt.Domain.DTOs.AccountPlant.Request;
using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IAccountPlantService
    {
        /// <summary>
        /// Retrieves an account plant entity asynchronously by its ID along with its associated data points.
        /// </summary>
        /// <param name="accountPlantId">The ID of the account plant to retrieve.</param>
        /// <returns>
        /// The <see cref="AccountPlantEntity"/> object representing the account plant, 
        /// or null if no account plant with the specified ID is found.
        /// </returns>
        Task<AccountPlantEntity?> GetPlantByIdAsync(int accountPlantId);

        /// <summary>
        /// Retrieves all account plant entities associated with a room.
        /// </summary>
        /// <param name="roomId">The ID of the room.</param>
        /// <returns>An enumerable collection of <see cref="AccountPlantEntity"/> objects representing the account plants associated with the room.</returns>
        IEnumerable<AccountPlantEntity> GetAccountPlantsFromRoom(int roomId);

        /// <summary>
        /// Retrieves all account plant entities associated with an account.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>An enumerable collection of <see cref="AccountPlantEntity"/> objects representing the account plants associated with the account.</returns>
        IEnumerable<AccountPlantEntity> GetAccountPlantsFromAccount(int accountId);

        /// <summary>
        /// Creates a new account plant entity asynchronously with the provided information.
        /// </summary>
        /// <param name="request">The <see cref="UpdateAccountPlantRequest"/> object containing the information to create the account plant.</param>
        /// <returns>The newly created <see cref="AccountPlantEntity"/> object.</returns>
        Task<AccountPlantEntity> CreateAccountPlantAsync(UpdateAccountPlantRequest request);

        /// <summary>
        /// Updates an account plant entity asynchronously with the provided information.
        /// </summary>
        /// <param name="request">The <see cref="UpdateAccountPlantRequest"/> object containing the updated information for the account plant.</param>
        /// <param name="accountPlantId">The ID of the account plant to update.</param>
        /// <returns>The updated <see cref="AccountPlantEntity"/> object.</returns>
        /// <exception cref="NoEntryFoundException">Thrown when no account plant with the specified ID is found.</exception>
        Task<AccountPlantEntity> UpdateAccountPlantAsync(UpdateAccountPlantRequest requst, int accountPlantId);

        /// <summary>
        /// Deletes an account plant entity asynchronously by its ID.
        /// </summary>
        /// <param name="accountPlantId">The ID of the account plant to delete.</param>
        /// <returns>True if the account plant is successfully deleted, false otherwise.</returns>
        Task<bool> DeleteAccountPlantAsync(int accountPlantId);

        /// <summary>
        /// Validates if an account is the owner of an account plant asynchronously.
        /// </summary>
        /// <param name="accountPlantId">The ID of the account plant to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>
        /// True if the account is the owner of the account plant, false otherwise.
        /// </returns>
        Task<bool> ValidateOwnerAsync(int accountPlantId, int accountId);
    }
}
