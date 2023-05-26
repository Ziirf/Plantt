using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IRoomRepository : IRepository<RoomEntity>
    {
        IEnumerable<RoomEntity> GetAllRooms(int accountId);
        Task<RoomEntity?> GetRoomByIdAsync(int roomId);
        Task<bool> IsValidOwnerAsync(int roomId, int accountId);
        bool IsValidOwner(int roomId, int accountId);
    }
}
