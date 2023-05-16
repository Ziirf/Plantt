using Microsoft.EntityFrameworkCore;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public class HomeRepository : GenericRepository<HomeEntity>, IHomeRepository
    {
        private readonly PlanttDBContext _context;

        public HomeRepository(PlanttDBContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<HomeEntity> GetAccountHome(int accountId)
        {
            return _context.Homes
                .Where(home => home.AccountId == accountId)
                .Include(home => home.Rooms)
                .AsNoTracking();
        }

        public Task<HomeEntity?> GetAccountHomeByIdAsync(int accountId, int homeId)
        {
            return _context.Homes
                .Include(home => home.Rooms)
                .FirstOrDefaultAsync(home => home.AccountId == accountId && home.Id == homeId);
        }
    }
}
