using Plantt.Domain.DTOs.Hub.Request;
using Plantt.Domain.DTOs.PlantData.Request;
using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IHubService
    {
        /// <summary>
        /// Saves the sensor data asynchronously.
        /// </summary>
        /// <param name="data">The <see cref="DataRequest"/> object containing the sensor data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="NoEntryFoundException">Thrown when no sensor or sensor plant is found.</exception>
        Task SaveDataAsync(SendDataRequest data);

        /// <summary>
        /// Registers a new hub asynchronously with the provided information.
        /// </summary>
        /// <param name="homeId">The ID of the home to associate the hub with.</param>
        /// <param name="name">The name of the hub.</param>
        /// <returns>The newly registered <see cref="HubEntity"/> object.</returns>
        Task<HubEntity> RegistreHubAsync(int homeId, string name);

        /// <summary>
        /// Retrieves all hubs associated with an account.
        /// </summary>
        /// <param name="account">The <see cref="AccountEntity"/> object representing the account.</param>
        /// <returns>An enumerable collection of <see cref="HubEntity"/> objects representing the hubs associated with the account.</returns>
        IEnumerable<HubEntity> GetHubsFromAccount(AccountEntity account);

        /// <summary>
        /// Verifies if the provided hub identity and secret are valid.
        /// </summary>
        /// <param name="identity">The identity of the hub.</param>
        /// <param name="secret">The secret of the hub.</param>
        /// <returns>True if the hub identity and secret are valid, false otherwise.</returns>
        Task<bool> VerifyHubAsync(string identity, string secret);

        /// <summary>
        /// Converts a <see cref="DateTime"/> value to its equivalent Unix epoch time representation.
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/> value to convert.</param>
        /// <returns>The Unix epoch time in seconds.</returns>
        long GetEpochTime(DateTime date);

        /// <summary>
        /// Validates if an account is the owner of a hub asynchronously.
        /// </summary>
        /// <param name="hubId">The ID of the hub to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>
        /// True if the account is the owner of the hub, false otherwise.
        /// </returns>
        Task<bool> ValidateOwnerAsync(int hubId, int accountId);
        Task SaveDataAsync(IEnumerable<SendDataRequest> data);
        bool IsSensorChildOfHub(int hubId, params int[] sensorId);
    }
}
