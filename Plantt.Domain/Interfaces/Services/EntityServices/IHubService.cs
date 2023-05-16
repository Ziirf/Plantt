using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IHubService
    {
        Task<IEnumerable<HubEntity>> GetHubsFromAccount(Guid accountGuid);
        Task<HubEntity> RegistreHubAsync(int homeId, string name);
        Task<bool> VerifyHub(string identity, string secret);
    }
}
