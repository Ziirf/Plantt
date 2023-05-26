using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IAccountRepository : IRepository<AccountEntity>
    {
        Task<AccountEntity?> GetByUsernameAsync(string username);
        Task<AccountEntity?> GetByGuidAsync(Guid guid);
        bool DoesUsernameExist(string username);
    }
}
