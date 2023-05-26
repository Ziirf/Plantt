using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IHubRepository : IRepository<HubEntity>
    {
        IEnumerable<HubEntity> GetHubsFromAccount(int accountId);
        Task<HubEntity?> GetHubByIdentityAsync(string identity);
        Task<bool> IsValidOwnerAsync(int hubId, int accountId);
        bool IsValidOwner(int hubId, int accountId);
    }
}
