using Plantt.Domain.DTOs.Sensor.Request;
using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface ISensorService
    {
        /// <summary>
        /// Retrieves all sensors associated with a hub.
        /// </summary>
        /// <param name="hubId">The ID of the hub.</param>
        /// <returns>An enumerable collection of <see cref="SensorEntity"/> objects representing the sensors associated with the hub.</returns>
        IEnumerable<SensorEntity> GetHubsSensors(int hubId);

        /// <summary>
        /// Registers a new sensor asynchronously with the provided sensor information.
        /// </summary>
        /// <param name="request">The <see cref="UpdateSensorRequest"/> object containing the sensor information.</param>
        /// <returns>The newly registered <see cref="SensorEntity"/> object.</returns>
        Task<SensorEntity> RegisterSensorAsync(UpdateSensorRequest request);

        /// <summary>
        /// Updates a sensor asynchronously with the provided sensor information.
        /// </summary>
        /// <param name="request">The <see cref="UpdateSensorRequest"/> object containing the updated sensor information.</param>
        /// <param name="sensorId">The ID of the sensor to update.</param>
        /// <returns>The updated <see cref="SensorEntity"/> object.</returns>
        /// <exception cref="NoEntryFoundException">Thrown when no sensor with the specified ID is found.</exception>
        Task<SensorEntity> UpdateSensorAsync(UpdateSensorRequest request, int sensorId);

        /// <summary>
        /// Deletes a sensor asynchronously.
        /// </summary>
        /// <param name="sensorId">The ID of the sensor to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="NoEntryFoundException">Thrown when no sensor with the specified ID is found.</exception>
        Task DeleteSensorAsync(int sensorId);

        /// <summary>
        /// Validates if an account is the owner of a sensor asynchronously.
        /// </summary>
        /// <param name="sensorId">The ID of the sensor to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>
        /// True if the account is the owner of the sensor, false otherwise.
        /// </returns>
        Task<bool> ValidateOwnerAsync(int sensorId, int accountId);
        IEnumerable<SensorEntity> GetAllFromAccount(int accountId);
    }
}
