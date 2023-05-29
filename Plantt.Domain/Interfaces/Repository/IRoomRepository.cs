using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IRoomRepository : IRepository<RoomEntity>
    {
        /// <summary>
        /// Retrieves all rooms associated with an account.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>An enumerable collection of <see cref="RoomEntity"/> objects representing the rooms associated with the account.</returns>
        IEnumerable<RoomEntity> GetAllRooms(int accountId);

        /// <summary>
        /// Validates asynchronously if an account is the owner of a room.
        /// </summary>
        /// <param name="roomId">The ID of the room to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>True if the account is the owner of the room, false otherwise.</returns>
        Task<bool> IsValidOwnerAsync(int roomId, int accountId);

        /// <summary>
        /// Validates if an account is the owner of a room.
        /// </summary>
        /// <param name="roomId">The ID of the room to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>True if the account is the owner of the room, false otherwise.</returns>
        bool IsValidOwner(int roomId, int accountId);
    }
}
