using Plantt.Domain.Entities;
using Plantt.Domain.Enums;

namespace Plantt.Applcation.Services
{
    public interface ITokenAuthenticationService
    {
        string GenerateAccessToken(Guid guid);
        Task<RefreshTokenEntity> GenerateRefreshTokenAsync(AccountEntity account);
        Task<RefreshTokenEntity> GenerateRefreshTokenAsync(AccountEntity account, TokenFamilyEntity tokenFamily);
        Task<RefreshTokenEntity?> GetRefreshToken(string refreshToken, int accountId);
        Task<RefreshTokenEntity?> GetRefreshToken(string refreshToken, Guid accountId);
        Task MarkRefreshTokenAsUsedAsync(RefreshTokenEntity refreshToken);
        Task RevokeTokenFamilyAsync(TokenFamilyEntity tokenFamily, TokenFamilyRevokeReason reason);
    }
}