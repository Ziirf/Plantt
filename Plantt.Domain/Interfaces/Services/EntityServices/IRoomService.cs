using Plantt.Domain.DTOs.Room.Request;
using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IRoomService
    {
        IEnumerable<RoomEntity> GetAccountsRooms(int accountId);
        Task<RoomEntity?> GetRoomByIdAsync(int roomId);
        Task<RoomEntity> CreateRoomAsync(UpdateRoomRequest roomRequest);
        Task<RoomEntity> UpdateRoomAsync(UpdateRoomRequest roomRequest, RoomEntity room);
        Task DeleteRoomAsync(RoomEntity roomEntity);
        Task<bool> ValidateOwnerAsync(int roomId, int accountId);
    }
}
