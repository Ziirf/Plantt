using Microsoft.EntityFrameworkCore;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public class HomeRepository : GenericRepository<HomeEntity>, IHomeRepository
    {
        private readonly PlanttDBContext _context;

        public HomeRepository(PlanttDBContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<HomeEntity> GetAccountHomes(int accountId)
        {
            return _context.Homes
                .Where(home => home.AccountId == accountId)
                .Include(home => home.Rooms)
                .AsNoTracking();
        }

        public override async Task<HomeEntity?> GetByIdAsync(int id)
        {
            return await _context.Homes
                .Include(home => home.Rooms)
                .OrderBy(home => home.Id)
                .FirstOrDefaultAsync(home => home.Id == id);
        }

        public bool IsValidOwner(int homeId, int accountId)
        {
            var result = _context.Homes
                .OrderBy(home => home.Id)
                .FirstOrDefault(home => home.Id == homeId && home!.AccountId == accountId);

            return result is not null;
        }

        public async Task<bool> IsValidOwnerAsync(int homeId, int accountId)
        {
            var result = await _context.Homes
                .OrderBy(home => home.Id)
                .FirstOrDefaultAsync(home => home.Id == homeId && home!.AccountId == accountId);

            return result is not null;
        }
    }
}
