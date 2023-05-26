using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Plantt.Domain.Config;
using Plantt.Domain.Entities;
using Plantt.Domain.Enums;
using Plantt.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Plantt.Applcation.Services
{
    public class TokenAuthenticationService : ITokenAuthenticationService
    {
        private readonly JsonWebTokenSettings _jwtsettings;
        private readonly RefreshTokenSettings _refreshTokenSettins;
        private readonly ILogger<TokenAuthenticationService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public TokenAuthenticationService(
            ILogger<TokenAuthenticationService> logger,
            IOptions<JsonWebTokenSettings> jwtsettings,
            IOptions<RefreshTokenSettings> refreshTokenSettins,
            IUnitOfWork unitOfWork)
        {
            _jwtsettings = jwtsettings.Value;
            _refreshTokenSettins = refreshTokenSettins.Value;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Generates an access token with the specified subject and role using the default time.
        /// </summary>
        /// <param name="subject">The subject associated with the access token.</param>
        /// <param name="role">The role associated with the access token.</param>
        /// <returns>A string representation of the generated access token.</returns>
        public string GenerateAccessToken(string subject, AccountRoles role = AccountRoles.Registred)
        {
            var defaultTime = _jwtsettings.TimeToLive.Time;

            return GenerateAccessToken(subject, defaultTime, role.ToString());
        }

        /// <summary>
        /// Generates an access token with the specified subject, expiration time, and role.
        /// </summary>
        /// <param name="subject">The subject associated with the access token.</param>
        /// <param name="expireIn">The duration for which the access token is valid.</param>
        /// <param name="role">The role associated with the access token.</param>
        /// <returns>A string representation of the generated access token.</returns>
        public string GenerateAccessToken(string subject, TimeSpan expireIn, AccountRoles role = AccountRoles.Registred)
        {
            return GenerateAccessToken(subject, expireIn, role.ToString());
        }

        /// <summary>
        /// Generates an access token with the specified subject, expiration time, and role.
        /// </summary>
        /// <param name="subject">The subject associated with the access token.</param>
        /// <param name="expireIn">The duration for which the access token is valid.</param>
        /// <param name="role">The role associated with the access token.</param>
        /// <returns>A string representation of the generated access token.</returns>
        public string GenerateAccessToken(string subject, TimeSpan expireIn, string role)
        {
            byte[] key = _jwtsettings.SecretKeyBytes;
            DateTime utcNow = DateTime.UtcNow;
            DateTime expireTime = utcNow + expireIn;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, subject),
                    new Claim(JwtRegisteredClaimNames.Iss, _jwtsettings.Issuer),
                    new Claim("role", role)
                }),
                NotBefore = utcNow,
                Expires = expireTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


        /// <summary>
        /// Generates a refresh token asynchronously for the specified token family.
        /// </summary>
        /// <param name="account">The account associated with the refresh token.</param>
        /// <returns>The generated RefreshTokenEntity as a task.</returns>
        public async Task<RefreshTokenEntity> GenerateRefreshTokenAsync(AccountEntity account)
        {
            var tokenFamily = CreateTokenFamilyEntity(account);

            var refreshToken = await GenerateRefreshTokenAsync(tokenFamily);

            return refreshToken;
        }

        /// <summary>
        /// Generates a refresh token asynchronously for the specified token family.
        /// </summary>
        /// <param name="tokenFamily">The token family associated with the refresh token.</param>
        /// <returns>The generated RefreshTokenEntity as a task.</returns>
        public async Task<RefreshTokenEntity> GenerateRefreshTokenAsync(TokenFamilyEntity tokenFamily)
        {
            DateTime utcNow = DateTime.UtcNow;

            var refreshToken = new RefreshTokenEntity()
            {
                Token = Base64UrlEncoder.Encode(GenerateRandomByteArray(_refreshTokenSettins.RefreshTokenLength)),
                TokenFamily = tokenFamily,
                IssuedTS = utcNow,
                ExpirationTS = utcNow.Add(_refreshTokenSettins.TimeToLive.Time),
                Used = false
            };

            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.CommitAsync();

            return refreshToken;
        }

        /// <summary>
        /// Generates a refresh token asynchronously for the specified account.
        /// </summary>
        /// <param name="account">The account associated with the refresh token.</param>
        /// <returns>The generated RefreshTokenEntity as a task.</returns>
        public async Task<RefreshTokenEntity?> GetRefreshTokenAsync(string refreshToken)
        {
            return await _unitOfWork.RefreshTokenRepository.GetByRefreshTokenAsync(refreshToken);
        }

        /// <summary>
        /// Marks the specified refresh token as used asynchronously.
        /// </summary>
        /// <param name="refreshToken">The refresh token to mark as used.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task MarkRefreshTokenAsUsedAsync(RefreshTokenEntity refreshToken)
        {
            refreshToken.Used = true;
            _unitOfWork.RefreshTokenRepository.Update(refreshToken);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Revokes the specified token family asynchronously with the specified reason.
        /// </summary>
        /// <param name="tokenFamily">The token family to revoke.</param>
        /// <param name="reason">The reason for revoking the token family.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RevokeTokenFamilyAsync(TokenFamilyEntity tokenFamily, TokenFamilyRevokeReason reason)
        {
            tokenFamily.RevokeTS = DateTime.UtcNow;
            tokenFamily.RevokeReason = reason;

            _unitOfWork.TokenFamilyRepository.Update(tokenFamily);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Validates the specified refresh token asynchronously.
        /// </summary>
        /// <param name="refreshToken">The refresh token to validate.</param>
        /// <returns>
        ///     A bool that represent if the Refresh token is valid(true) or not(false), as a task.
        /// </returns>
        public async Task<bool> ValidateRefreshTokenAsync(RefreshTokenEntity refreshToken)
        {

            if (refreshToken.TokenFamily.RevokeTS is not null)
            {
                return false;
            }

            if (refreshToken.Used is true)
            {
                await RevokeTokenFamilyAsync(refreshToken.TokenFamily, TokenFamilyRevokeReason.Compromised);
                return false;
            }

            if (DateTime.UtcNow.Ticks > refreshToken.ExpirationTS.Ticks)
            {
                await RevokeTokenFamilyAsync(refreshToken.TokenFamily, TokenFamilyRevokeReason.Expired);
                return false;
            }

            return true;
        }

        private TokenFamilyEntity CreateTokenFamilyEntity(AccountEntity account)
        {
            var tokenFamily = new TokenFamilyEntity()
            {
                Account = account,
                Identifier = Base64UrlEncoder.Encode(GenerateRandomByteArray(_refreshTokenSettins.RefreshFamilyLength))
            };

            return tokenFamily;
        }

        private static byte[] GenerateRandomByteArray(int byteLength)
        {
            var bytes = new byte[byteLength];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
                return bytes;
            }
        }
    }
}
