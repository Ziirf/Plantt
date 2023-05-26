using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IPlantService
    {
        Task<IEnumerable<PlantEntity>?> GetPlantPageAsync(int amount, int page, string? search = null);
        Task<PlantEntity?> GetPlantByIdAsync(int id);
    }
}
