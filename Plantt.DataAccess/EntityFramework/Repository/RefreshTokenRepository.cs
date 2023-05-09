using Microsoft.EntityFrameworkCore;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public class RefreshTokenRepository : GenericRepository<RefreshTokenEntity>, IRefreshTokenRepository
    {
        private readonly PlanttDbContext _context;

        public RefreshTokenRepository(PlanttDbContext context) : base(context)
        {
            _context = context;
        }

        public override IEnumerable<RefreshTokenEntity> Getall()
        {
            return _context.RefreshTokens.Include(token => token.TokenFamily).AsNoTracking();
        }

        public override RefreshTokenEntity? GetById(int id)
        {
            return _context.RefreshTokens
                .Include(token => token.TokenFamily)
                .FirstOrDefault(token => token.Id == id);
        }

        public override async Task<RefreshTokenEntity?> GetByIdAsync(int id)
        {
            return await _context.RefreshTokens
                .Include(token => token.TokenFamily)
                .FirstOrDefaultAsync(token => token.Id == id);
        }

        public async Task<RefreshTokenEntity?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens
                .Include(token => token.TokenFamily)
                .FirstOrDefaultAsync(token => token.Token == refreshToken);
        }
    }
}
