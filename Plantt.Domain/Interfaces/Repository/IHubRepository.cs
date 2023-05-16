using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IHubRepository : IRepository<HubEntity>
    {
        Task<HubEntity?> GetHubByIdentityAsync(string identity);
        IEnumerable<HubEntity> GetHubsFromAccount(int accountId);
    }
}
