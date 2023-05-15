using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services
{
    public interface IPlantControllerService
    {
        Task<PlantEntity?> GetPlantById(int id);
        Task<IEnumerable<PlantEntity>?> GetPlantPage(int amount, int page);
    }
}
