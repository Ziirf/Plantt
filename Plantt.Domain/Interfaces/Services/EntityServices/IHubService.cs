using Plantt.Domain.DTOs.Hub.Request;
using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IHubService
    {
        IEnumerable<HubEntity> GetHubsFromAccount(AccountEntity account);
        Task SaveDataAsync(DataRequest data);
        Task<HubEntity> RegistreHubAsync(int homeId, string name);
        Task<bool> VerifyHubAsync(string identity, string secret);
        long GetEpochTime(DateTime date);
        Task<bool> ValidateOwnerAsync(int hubId, int accountId);
    }
}
