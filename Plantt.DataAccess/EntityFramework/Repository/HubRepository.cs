using Microsoft.EntityFrameworkCore;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public class HubRepository : GenericRepository<HubEntity>, IHubRepository
    {
        private readonly PlanttDBContext _context;

        public HubRepository(PlanttDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<HubEntity?> GetHubByIdentityAsync(string identity)
        {
            return await _context.Hubs
                .OrderBy(hub => hub.Id)
                .FirstOrDefaultAsync(hub => hub.Identity == identity);
        }

        public IEnumerable<HubEntity> GetHubsFromAccount(int accountId)
        {
            return _context.Hubs
                .Include(hub => hub.Home)
                .Where(hub => hub.Home!.AccountId == accountId)
                .AsNoTracking();
        }

        public async Task<bool> IsValidOwnerAsync(int hubId, int accountId)
        {
            var result = await _context.Hubs
                .OrderBy(hub => hub.Id)
                .FirstOrDefaultAsync(hub => hub.Id == hubId && hub.Home!.AccountId == accountId);

            return result is not null;
        }

        public bool IsValidOwner(int hubId, int accountId)
        {
            var result = _context.Hubs
                .OrderBy(hub => hub.Id)
                .FirstOrDefault(hub => hub.Id == hubId && hub.Home!.AccountId == accountId);

            return result is not null;
        }

        public bool IsSensorsChildOfHubAsync(int hubId, params int[] sensorIdArray)
        {
            var sensorIds = _context.Hubs
                .Where(hub => hub.Id == hubId)
                .SelectMany(hub => hub.Sensors.Select(sensor => sensor.Id))
                .ToList();

            return sensorIdArray.All(id => sensorIds.Contains(id));
        }
    }
}
