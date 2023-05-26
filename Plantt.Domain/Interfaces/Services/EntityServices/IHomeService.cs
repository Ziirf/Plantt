using Plantt.Domain.DTOs.Home.Request;
using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IHomeService
    {
        IEnumerable<HomeEntity> GetAccountHomes(int accountId);
        Task<HomeEntity?> GetAccountHomeByIdAsync(int accountId, int homeId);
        Task<HomeEntity> CreateHomeAsync(UpdateHomeRequest request, int accountId);
        Task<HomeEntity> UpdateHomeAsync(UpdateHomeRequest request, int id);
        Task DeleteHomeAsync(int homeId);
        Task<bool> ValidateOwnerAsync(int homeId, int accountId);
    }
}
