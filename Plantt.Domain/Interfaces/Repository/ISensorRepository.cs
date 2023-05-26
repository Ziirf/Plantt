using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface ISensorRepository : IRepository<SensorEntity>
    {
        IEnumerable<SensorEntity> GetAllFromHub(int hubId);
        Task<bool> IsValidOwnerAsync(int sensorId, int accountId);
    }
}
