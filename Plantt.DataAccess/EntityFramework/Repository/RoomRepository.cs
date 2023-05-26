using Microsoft.EntityFrameworkCore;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public class RoomRepository : GenericRepository<RoomEntity>, IRoomRepository
    {
        private readonly PlanttDBContext _context;

        public RoomRepository(PlanttDBContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<RoomEntity> GetAllRooms(int accountId)
        {
            return _context.Rooms
                .Include(room => room.Plants)
                .ThenInclude(myPlant => myPlant.Plant)
                .ThenInclude(plant => plant!.PlantWatering)
                .Include(room => room.Home)
                .Where(room => room.Home!.AccountId == accountId)
                .AsNoTracking();
        }

        public async Task<RoomEntity?> GetRoomByIdAsync(int roomId)
        {
            return await _context.Rooms
                .Include(room => room.Plants)
                .ThenInclude(myPlant => myPlant.Plant)
                .ThenInclude(plant => plant!.PlantWatering)
                .Include(room => room.Home)
                .OrderBy(room => room.Id)
                .FirstOrDefaultAsync(room => room.Id == roomId);
        }


        public async Task<bool> IsValidOwnerAsync(int roomId, int accountId)
        {
            var result = await _context.Rooms
                .OrderBy(room => room.Id)
                .FirstOrDefaultAsync(room => room.Id == roomId && room.Home!.AccountId == accountId);

            return result is not null;
        }

        public bool IsValidOwner(int roomId, int accountId)
        {
            var result = _context.Rooms
                .OrderBy(room => room.Id)
                .FirstOrDefault(room => room.Id == roomId && room.Home!.AccountId == accountId);

            return result is not null;
        }
    }
}
