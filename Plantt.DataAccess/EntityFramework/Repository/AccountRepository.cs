using Microsoft.EntityFrameworkCore;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public class AccountRepository : GenericRepository<AccountEntity>, IAccountRepository
    {
        private readonly PlanttDBContext _context;

        public AccountRepository(PlanttDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AccountEntity?> GetByUsernameAsync(string username)
        {
            return await _context.Accounts
                .OrderBy(account => account.Id)
                .FirstOrDefaultAsync(account => account.Username == username);
        }

        public async Task<AccountEntity?> GetByGuidAsync(Guid guid)
        {
            return await _context.Accounts
                .OrderBy(account => account.Id)
                .FirstOrDefaultAsync(account => account.PublicId == guid);
        }

        public bool DoesUsernameExist(string username)
        {
            return _context.Accounts.Any(account => account.Username == username);
        }
    }
}
