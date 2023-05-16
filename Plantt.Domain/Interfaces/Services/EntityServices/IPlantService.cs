using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IPlantService
    {
        Task<PlantEntity?> GetPlantById(int id);
        Task<IEnumerable<PlantEntity>?> GetPlantPage(int amount, int page);
    }
}
