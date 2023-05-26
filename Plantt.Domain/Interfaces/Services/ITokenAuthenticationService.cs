using Plantt.Domain.Entities;
using Plantt.Domain.Enums;

namespace Plantt.Applcation.Services
{
    public interface ITokenAuthenticationService
    {

        /// <summary>
        /// Generates an access token with the specified subject and role using the default time.
        /// </summary>
        /// <param name="subject">The subject associated with the access token.</param>
        /// <param name="role">The role associated with the access token.</param>
        /// <returns>A string representation of the generated access token.</returns>
        string GenerateAccessToken(string subject, AccountRoles role = AccountRoles.Registred);

        /// <summary>
        /// Generates an access token with the specified subject, expiration time, and role.
        /// </summary>
        /// <param name="subject">The subject associated with the access token.</param>
        /// <param name="expireIn">The duration for which the access token is valid.</param>
        /// <param name="role">The role associated with the access token.</param>
        /// <returns>A string representation of the generated access token.</returns>
        string GenerateAccessToken(string subject, TimeSpan expireIn, AccountRoles role = AccountRoles.Registred);

        /// <summary>
        /// Generates an access token with the specified subject, expiration time, and role.
        /// </summary>
        /// <param name="subject">The subject associated with the access token.</param>
        /// <param name="expireIn">The duration for which the access token is valid.</param>
        /// <param name="role">The role associated with the access token.</param>
        /// <returns>A string representation of the generated access token.</returns>
        string GenerateAccessToken(string subject, TimeSpan expireIn, string role);

        /// <summary>
        /// Generates a refresh token asynchronously for the specified token family.
        /// </summary>
        /// <param name="account">The account associated with the refresh token.</param>
        /// <returns>The generated RefreshTokenEntity as a task.</returns>
        Task<RefreshTokenEntity> GenerateRefreshTokenAsync(AccountEntity account);
        /// <summary>
        /// Generates a refresh token asynchronously for the specified token family.
        /// </summary>
        /// <param name="tokenFamily">The token family associated with the refresh token.</param>
        /// <returns>The generated RefreshTokenEntity as a task.</returns>
        Task<RefreshTokenEntity> GenerateRefreshTokenAsync(TokenFamilyEntity tokenFamily);

        /// <summary>
        /// Generates a refresh token asynchronously for the specified account.
        /// </summary>
        /// <param name="account">The account associated with the refresh token.</param>
        /// <returns>The generated RefreshTokenEntity as a task.</returns>
        Task<RefreshTokenEntity?> GetRefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Marks the specified refresh token as used asynchronously.
        /// </summary>
        /// <param name="refreshToken">The refresh token to mark as used.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task MarkRefreshTokenAsUsedAsync(RefreshTokenEntity refreshToken);

        /// <summary>
        /// Revokes the specified token family asynchronously with the specified reason.
        /// </summary>
        /// <param name="tokenFamily">The token family to revoke.</param>
        /// <param name="reason">The reason for revoking the token family.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RevokeTokenFamilyAsync(TokenFamilyEntity tokenFamily, TokenFamilyRevokeReason reason);

        /// <summary>
        /// Validates the specified refresh token asynchronously.
        /// </summary>
        /// <param name="refreshToken">The refresh token to validate.</param>
        /// <returns>
        ///     A bool that represent if the Refresh token is valid(true) or not(false), as a task.
        /// </returns>
        Task<bool> ValidateRefreshTokenAsync(RefreshTokenEntity refreshToken);
    }
}