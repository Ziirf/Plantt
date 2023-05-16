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
            return await _context.Hubs.FirstOrDefaultAsync(hub => hub.Identity == identity);
        }

        public IEnumerable<HubEntity> GetHubsFromAccount(int accountId)
        {
            return _context.Hubs.Include(hub => hub.Home).Where(hub => hub.Home!.AccountId == accountId).AsNoTracking();
        }
    }
}
