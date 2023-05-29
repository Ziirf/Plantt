using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface ISensorRepository : IRepository<SensorEntity>
    {
        IEnumerable<SensorEntity> GetAllFromAccount(int accountId);

        /// <summary>
        /// Retrieves all sensors associated with a hub.
        /// </summary>
        /// <param name="hubId">The ID of the hub.</param>
        /// <returns>An enumerable collection of <see cref="SensorEntity"/> objects representing the sensors associated with the hub.</returns>
        IEnumerable<SensorEntity> GetAllFromHub(int hubId);

        /// <summary>
        /// Validates asynchronously if an account is the owner of a sensor.
        /// </summary>
        /// <param name="sensorId">The ID of the sensor to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>True if the account is the owner of the sensor, false otherwise.</returns>
        Task<bool> IsValidOwnerAsync(int sensorId, int accountId);
    }
}
