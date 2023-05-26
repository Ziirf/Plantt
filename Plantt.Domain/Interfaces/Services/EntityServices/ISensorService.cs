using Plantt.Domain.DTOs.Sensor.Request;
using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface ISensorService
    {
        IEnumerable<SensorEntity> GetHubsSensors(int hubId);
        Task<SensorEntity> RegisterSensorAsync(UpdateSensorRequest request);
        Task<SensorEntity> UpdateSensorAsync(UpdateSensorRequest request, int sensorId);
        Task DeleteSensorAsync(int sensorId);
        Task<bool> ValidateOwnerAsync(int sensorId, int accountId);
    }
}
