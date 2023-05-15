using Plantt.Domain.Entities;
using Plantt.Domain.Enums;

namespace Plantt.Applcation.Services
{
    public interface ITokenAuthenticationService
    {
        string GenerateAccessToken(string subject, AccountRoles role = AccountRoles.Registred);
        string GenerateAccessToken(string subject, TimeSpan expireIn, string role);
        string GenerateAccessToken(string subject, TimeSpan expireIn, AccountRoles role = AccountRoles.Registred);
        Task<RefreshTokenEntity> GenerateRefreshTokenAsync(AccountEntity account);
        Task<RefreshTokenEntity> GenerateRefreshTokenAsync(TokenFamilyEntity tokenFamily);
        Task<RefreshTokenEntity?> GetRefreshTokenAsync(string refreshToken);
        Task MarkRefreshTokenAsUsedAsync(RefreshTokenEntity refreshToken);
        Task RevokeTokenFamilyAsync(TokenFamilyEntity tokenFamily, TokenFamilyRevokeReason reason);
        Task<bool> ValidateRefreshTokenAsync(RefreshTokenEntity refreshToken);
    }
}