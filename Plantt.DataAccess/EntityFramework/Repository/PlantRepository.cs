using Microsoft.EntityFrameworkCore;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public sealed class PlantRepository : GenericRepository<PlantEntity>, IPlantRepository
    {
        private readonly PlanttDBContext _context;

        public PlantRepository(PlanttDBContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<PlantEntity?> GetByIdAsync(int id)
        {
            return await _context.Plants
                .Include(plant => plant.PlantWatering)
                .FirstOrDefaultAsync(plant => plant.Id == id);
        }

        public async Task<IEnumerable<PlantEntity>> GetPlantPageAsync(int amount, int page)
        {
            return await _context.Plants
                .Include(plant => plant.PlantWatering)
                .OrderBy(plant => plant.Id)
                .Skip(amount * (page - 1))
                .Take(amount).AsNoTracking()
                .ToListAsync();
        }
    }
}
