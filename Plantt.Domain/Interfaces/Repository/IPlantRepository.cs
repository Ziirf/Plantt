using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IPlantRepository : IRepository<PlantEntity>
    {
        Task<IEnumerable<PlantEntity>> GetPlantPageAsync(int amount, int page, string? search = null);
    }
}
