using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IHubRepository : IRepository<HubEntity>
    {
        /// <summary>
        /// Retrieves a hub entity asynchronously by its identity.
        /// </summary>
        /// <param name="identity">The identity of the hub to retrieve.</param>
        /// <returns>
        /// The <see cref="HubEntity"/> object representing the hub,
        /// or null if no hub with the specified identity is found.
        /// </returns>
        Task<HubEntity?> GetHubByIdentityAsync(string identity);

        /// <summary>
        /// Retrieves the hubs associated with an account.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>An enumerable collection of <see cref="HubEntity"/> objects representing the hubs associated with the account.</returns>
        IEnumerable<HubEntity> GetHubsFromAccount(int accountId);

        /// <summary>
        /// Validates asynchronously if an account is the owner of a hub.
        /// </summary>
        /// <param name="hubId">The ID of the hub to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>True if the account is the owner of the hub, false otherwise.</returns>
        Task<bool> IsValidOwnerAsync(int hubId, int accountId);

        /// <summary>
        /// Validates if an account is the owner of a hub.
        /// </summary>
        /// <param name="hubId">The ID of the hub to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>True if the account is the owner of the hub, false otherwise.</returns>
        bool IsValidOwner(int hubId, int accountId);

        /// <summary>
        /// Validates if an sensor is the child of a hub.
        /// </summary>
        /// <param name="hubId">The ID of the hub to validate.</param>
        /// <param name="sensorIdArray">The id's of the sensors to validate</param>
        /// <returns>Tru if all sensors are child of the hub, false otherwise</returns>
        bool IsSensorsChildOfHubAsync(int hubId, params int[] sensorIdArray);
    }
}
