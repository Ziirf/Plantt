using Microsoft.EntityFrameworkCore;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public class SensorRepository : GenericRepository<SensorEntity>, ISensorRepository
    {
        private readonly PlanttDBContext _context;

        public SensorRepository(PlanttDBContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SensorEntity> GetAllFromAccount(int accountId)
        {
            return _context.Sensor
                .Include(sensor => sensor.AccountPlant)
                .ThenInclude(accountPlant => accountPlant!.Plant)
                .Where(sensor => sensor!.Hub!.Home!.AccountId == accountId)
                .AsNoTracking();
        }

        public IEnumerable<SensorEntity> GetAllFromHub(int hubId)
        {
            return _context.Sensor
                .Include(sensor => sensor.AccountPlant)
                .ThenInclude(accountPlant => accountPlant!.Plant)
                .Where(sensor => sensor.HubId == hubId)
                .AsNoTracking();
        }

        public async Task<bool> IsValidOwnerAsync(int sensorId, int accountId)
        {
            var result = await _context.Sensor
                .OrderBy(sensor => sensor.Id)
                .FirstOrDefaultAsync(sensor => sensor.Id == sensorId && sensor.Hub!.Home!.AccountId == accountId);

            return result is not null;
        }
    }
}
