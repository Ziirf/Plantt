using Plantt.Domain.Entities;
using Plantt.Domain.Enums;

namespace Plantt.Applcation.Services
{
    public interface ITokenAuthenticationService
    {
        string GenerateAccessToken(Guid guid, string role = "Free");
        Task<RefreshTokenEntity> GenerateRefreshTokenAsync(AccountEntity account);
        Task<RefreshTokenEntity> GenerateRefreshTokenAsync(TokenFamilyEntity tokenFamily);
        Task<RefreshTokenEntity?> GetRefreshToken(string refreshToken);
        Task MarkRefreshTokenAsUsedAsync(RefreshTokenEntity refreshToken);
        Task RevokeTokenFamilyAsync(TokenFamilyEntity tokenFamily, TokenFamilyRevokeReason reason);
    }
}