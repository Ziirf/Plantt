using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IHomeRepository : IRepository<HomeEntity>
    {
        IEnumerable<HomeEntity> GetAccountHome(int accountId);
        Task<HomeEntity?> GetAccountHomeByIdAsync(int accountId, int homeId);
    }
}
