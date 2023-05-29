using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IAccountPlantRepository : IRepository<AccountPlantEntity>
    {
        /// <summary>
        /// Retrieves an account plant entity asynchronously by its ID, including associated plant and data points (within a specified data age range in days).
        /// </summary>
        /// <param name="id">The ID of the account plant to retrieve.</param>
        /// <param name="dataAgeInDays">The age of the plant data points to include, in days. Default is 7 days.</param>
        /// <returns>
        /// The <see cref="AccountPlantEntity"/> object representing the account plant,
        /// or null if no account plant with the specified ID is found.
        /// </returns>
        Task<AccountPlantEntity?> GetPlantWithDataPointsAsync(int id, int dataAgeInDays = 7);

        /// <summary>
        /// Retrieves all account plants associated with a room.
        /// </summary>
        /// <param name="roomId">The ID of the room.</param>
        /// <returns>An enumerable collection of <see cref="AccountPlantEntity"/> objects representing the account plants associated with the room.</returns>
        IEnumerable<AccountPlantEntity> GetAllFromRoom(int roomId);

        /// <summary>
        /// Retrieves all account plants associated with an account.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>An enumerable collection of <see cref="AccountPlantEntity"/> objects representing the account plants associated with the account.</returns>
        IEnumerable<AccountPlantEntity> GetAllFromAccount(int accountId);

        /// <summary>
        /// Validates if an account is the owner of a plant asynchronously.
        /// </summary>
        /// <param name="plantId">The ID of the plant to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>
        /// True if the account is the owner of the plant, false otherwise.
        /// </returns>
        Task<bool> IsValidOwnerAsync(int plantId, int accountId);

        /// <summary>
        /// Validates if an account is the owner of a plant.
        /// </summary>
        /// <param name="plantId">The ID of the plant to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>
        /// True if the account is the owner of the plant, false otherwise.
        /// </returns>
        bool IsValidOwner(int plantId, int accountId);
    }
}
