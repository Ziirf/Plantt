using Plantt.Domain.DTOs.Home.Request;
using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IHomeService
    {
        /// <summary>
        /// Retrieves a home entity asynchronously by its ID.
        /// </summary>
        /// <param name="homeId">The ID of the home to retrieve.</param>
        /// <returns>
        /// The <see cref="HomeEntity"/> object representing the home, 
        /// or null if no home with the specified ID is found.
        /// </returns>
        Task<HomeEntity?> GetHomeByIdAsync(int homeId);

        /// <summary>
        /// Retrieves all home entities associated with an account.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>An enumerable collection of <see cref="HomeEntity"/> objects representing the homes associated with the account.</returns>
        IEnumerable<HomeEntity> GetAllAccountHomes(int accountId);

        /// <summary>
        /// Creates a new home entity asynchronously with the provided information and associates it with the specified account.
        /// </summary>
        /// <param name="request">The <see cref="UpdateHomeRequest"/> object containing the information to create the home.</param>
        /// <param name="accountId">The ID of the account to associate the home with.</param>
        /// <returns>The newly created <see cref="HomeEntity"/> object.</returns>
        Task<HomeEntity> CreateHomeAsync(UpdateHomeRequest request, int accountId);

        /// <summary>
        /// Updates a home entity asynchronously with the provided information.
        /// </summary>
        /// <param name="request">The <see cref="UpdateHomeRequest"/> object containing the updated information for the home.</param>
        /// <param name="id">The ID of the home to update.</param>
        /// <returns>The updated <see cref="HomeEntity"/> object.</returns>
        /// <exception cref="NoEntryFoundException">Thrown when no home with the specified ID is found.</exception>
        Task<HomeEntity> UpdateHomeAsync(UpdateHomeRequest request, int id);

        /// <summary>
        /// Deletes a home entity asynchronously by its ID.
        /// </summary>
        /// <param name="homeId">The ID of the home to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="NoEntryFoundException">Thrown when no home with the specified ID is found.</exception>
        Task DeleteHomeAsync(int homeId);

        /// <summary>
        /// Validates if an account is the owner of a home asynchronously.
        /// </summary>
        /// <param name="homeId">The ID of the home to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>
        /// True if the account is the owner of the home, false otherwise.
        /// </returns>
        Task<bool> ValidateOwnerAsync(int homeId, int accountId);
    }
}
