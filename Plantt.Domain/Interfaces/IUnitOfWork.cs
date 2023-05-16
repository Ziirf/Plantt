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

        void Commit();
        Task CommitAsync();
    }
}
