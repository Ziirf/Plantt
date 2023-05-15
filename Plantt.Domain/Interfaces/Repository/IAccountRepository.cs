using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IAccountRepository : IRepository<AccountEntity>
    {
        Task<AccountEntity?> GetByGuidAsync(Guid guid);
        Task<AccountEntity?> GetByUsernameAsync(string username);
        bool DoesUsernameExist(string username);
    }
}
