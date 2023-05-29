using Plantt.Domain.Interfaces.Repository;

namespace Plantt.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository AccountRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        ITokenFamilyRepository TokenFamilyRepository { get; }
        IPlantRepository PlantRepository { get; }
        IHubRepository HubRepository { get; }
        IHomeRepository HomeRepository { get; }
        IRoomRepository RoomRepository { get; }
        IAccountPlantRepository AccountPlantRepository { get; }
        ISensorRepository SensorRepository { get; }
        IPlantDataRepository PlantDataRepository { get; }

        /// <summary>
        /// Commits the changes made to the underlying database context.
        /// </summary>
        void Commit();

        /// <summary>
        /// Asynchronously commits the changes made to the database context.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>s
        Task CommitAsync();

        /// <summary>
        /// Disposes the resources used by the repository.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        void Dispose(bool disposing);
    }
}
