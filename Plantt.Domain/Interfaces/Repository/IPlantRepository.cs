using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IPlantRepository : IRepository<PlantEntity>
    {
        /// <summary>
        /// Retrieves a page of plants asynchronously based on the specified amount and page number, with optional search filtering.
        /// </summary>
        /// <param name="amount">The number of plants to retrieve per page.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="search">Optional. The search string to filter plants by Latin name or common name.</param>
        /// <returns>An enumerable collection of <see cref="PlantEntity"/> objects representing the plants on the specified page.</returns>
        Task<IEnumerable<PlantEntity>> GetPlantPageAsync(int amount, int page, string? search = null);
    }
}
