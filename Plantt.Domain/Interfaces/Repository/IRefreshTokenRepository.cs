using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IRefreshTokenRepository : IRepository<RefreshTokenEntity>
    {
        /// <summary>
        /// Retrieves a refresh token entity asynchronously by the refresh token value.
        /// </summary>
        /// <param name="refreshToken">The refresh token to retrieve.</param>
        /// <returns>
        /// The <see cref="RefreshTokenEntity"/> object representing the refresh token,
        /// or null if no refresh token with the specified value is found.
        /// </returns>
        Task<RefreshTokenEntity?> GetByRefreshTokenAsync(string refreshToken);
    }
}
