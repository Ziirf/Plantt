using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services
{
    public interface IHubControllerService
    {
        Task<IEnumerable<HubEntity>> GetHubsFromAccount(Guid accountGuid);
        Task<HubEntity> RegistreHubAsync(int homeId, string name);
        Task<bool> VerifyHub(string identity, string secret);
    }
}
