using Plantt.Domain.DTOs.AccountPlant.Request;
using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IAccountPlantService
    {
        Task<AccountPlantEntity?> GetPlantByIdAsync(int accountPlantId);
        IEnumerable<AccountPlantEntity> GetAccountPlantsFromAccount(int accountId);
        IEnumerable<AccountPlantEntity> GetAccountPlantsFromRoom(int roomId);
        Task<AccountPlantEntity> CreateAccountPlantAsync(UpdateAccountPlantRequest request);
        Task<AccountPlantEntity> UpdateAccountPlantAsync(UpdateAccountPlantRequest requst, int accountPlantId);
        Task<bool> DeleteAccountPlantAsync(int accountPlantId);
        Task<bool> ValidateOwnerAsync(int accountPlantId, int accountId);
    }
}
