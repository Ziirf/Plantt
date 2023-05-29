using Microsoft.EntityFrameworkCore;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    internal class AccountPlantRepository : GenericRepository<AccountPlantEntity>, IAccountPlantRepository
    {
        private readonly PlanttDBContext _context;

        public AccountPlantRepository(PlanttDBContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<AccountPlantEntity?> GetByIdAsync(int id)
        {
            return await _context.AccountPlants
                    .Include(ap => ap.Plant)
                    .OrderBy(ap => ap.Id)
                    .FirstOrDefaultAsync(ap => ap.Id == id);
        }

        public async Task<AccountPlantEntity?> GetPlantWithDataPointsAsync(int id, int dataAgeInDays = 7)
        {
            return await _context.AccountPlants
                    .Include(ap => ap.Plant)
                    .Include(ap => ap.PlantData.Where(pd => pd.CreatedTS >= DateTime.UtcNow.AddDays(dataAgeInDays * -1)).OrderByDescending(pd => pd.CreatedTS))
                    .OrderBy(ap => ap.Id)
                    .FirstOrDefaultAsync(ap => ap.Id == id);
        }

        public IEnumerable<AccountPlantEntity> GetAllFromRoom(int roomId)
        {
            return _context.AccountPlants
                    .Include(ap => ap.Plant)
                    .Where(ap => ap.RoomId == roomId)
                    .AsNoTracking();
        }

        public IEnumerable<AccountPlantEntity> GetAllFromAccount(int accountId)
        {
            return _context.AccountPlants
                    .Include(ap => ap.Plant)
                    .Where(ap => ap.Room!.Home!.AccountId == accountId)
                    .AsNoTracking();
        }

        public async Task<bool> IsValidOwnerAsync(int plantId, int accountId)
        {
            var result = await _context.AccountPlants
                    .OrderBy(ap => ap.Id)
                    .FirstOrDefaultAsync(ap => ap.Id == plantId && ap.Room!.Home!.AccountId == accountId);

            return result is not null;
        }

        public bool IsValidOwner(int plantId, int accountId)
        {
            var result = _context.AccountPlants
                    .OrderBy(ap => ap.Id)
                    .FirstOrDefault(ap => ap.Id == plantId && ap.Room!.Home!.AccountId == accountId);

            return result is not null;
        }
    }
}
