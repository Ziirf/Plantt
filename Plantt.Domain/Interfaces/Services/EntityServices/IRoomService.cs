using Plantt.Domain.DTOs.Room.Request;
using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IRoomService
    {
        /// <summary>
        /// Retrieves a room entity asynchronously by its ID.
        /// </summary>
        /// <param name="roomId">The ID of the room to retrieve.</param>
        /// <returns>
        /// The <see cref="RoomEntity"/> object representing the room,
        /// or null if no room with the specified ID is found.
        /// </returns>
        Task<RoomEntity?> GetRoomByIdAsync(int roomId);

        /// <summary>
        /// Retrieves all rooms associated with an account.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>An enumerable collection of <see cref="RoomEntity"/> objects representing the rooms associated with the account.</returns>
        IEnumerable<RoomEntity> GetAccountsRooms(int accountId);

        /// <summary>
        /// Creates a new room asynchronously with the provided room information.
        /// </summary>
        /// <param name="roomRequest">The <see cref="UpdateRoomRequest"/> object containing the new room information.</param>
        /// <returns>The newly created <see cref="RoomEntity"/> object.</returns>
        Task<RoomEntity> CreateRoomAsync(UpdateRoomRequest roomRequest);

        /// <summary>
        /// Updates a room asynchronously with the provided room information.
        /// </summary>
        /// <param name="roomRequest">The <see cref="UpdateRoomRequest"/> object containing the updated room information.</param>
        /// <param name="room">The <see cref="RoomEntity"/> object representing the room to update.</param>
        /// <returns>The updated <see cref="RoomEntity"/> object.</returns>
        Task<RoomEntity> UpdateRoomAsync(UpdateRoomRequest roomRequest, RoomEntity room);

        /// <summary>
        /// Deletes a room asynchronously.
        /// </summary>
        /// <param name="roomEntity">The <see cref="RoomEntity"/> object representing the room to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteRoomAsync(RoomEntity roomEntity);

        /// <summary>
        /// Validates if an account is the owner of a room asynchronously.
        /// </summary>
        /// <param name="roomId">The ID of the room to validate.</param>
        /// <param name="accountId">The ID of the account to validate as the owner.</param>
        /// <returns>
        /// True if the account is the owner of the room, false otherwise.
        /// </returns>
        Task<bool> ValidateOwnerAsync(int roomId, int accountId);
    }
}
