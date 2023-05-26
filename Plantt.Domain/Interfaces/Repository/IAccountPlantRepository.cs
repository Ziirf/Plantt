using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IAccountPlantRepository : IRepository<AccountPlantEntity>
    {
        IEnumerable<AccountPlantEntity> GetAllFromAccount(int accountId);
        IEnumerable<AccountPlantEntity> GetAllFromRoom(int roomId);
        Task<AccountPlantEntity?> GetPlantWithDataPointsAsync(int id, int dataAgeInDays = 7);
        Task<bool> IsValidOwnerAsync(int plantId, int accountId);
        bool IsValidOwner(int plantId, int accountId);
    }
}
