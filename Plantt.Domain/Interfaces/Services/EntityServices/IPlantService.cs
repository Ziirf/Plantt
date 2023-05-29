using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IPlantService
    {
        /// <summary>
        /// Retrieves a page of plant entities asynchronously based on the specified pagination parameters and optional search term.
        /// </summary>
        /// <param name="pageSize">The number of plants to include in each page.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="search">An optional search term to filter the plants.</param>
        /// <returns>
        /// An enumerable collection of <see cref="PlantEntity"/> objects representing the plants in the specified page,
        /// or null if there are no plants matching the search criteria or if the page is out of range.
        /// </returns>
        Task<IEnumerable<PlantEntity>?> GetPlantPageAsync(int amount, int page, string? search = null);

        /// <summary>
        /// Retrieves a plant entity asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the plant to retrieve.</param>
        /// <returns>
        /// The <see cref="PlantEntity"/> object representing the plant, 
        /// or null if no plant with the specified ID is found.
        /// </returns>
        Task<PlantEntity?> GetPlantByIdAsync(int id);
    }
}
