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

        public string GenerateAccessToken(string subject, AccountRoles role = AccountRoles.Registred)
        {
            var defaultTime = _jwtsettings.TimeToLive.Time;

            return GenerateAccessToken(subject, defaultTime, role.ToString());
        }

        public string GenerateAccessToken(string subject, TimeSpan expireIn, AccountRoles role = AccountRoles.Registred)
        {
            return GenerateAccessToken(subject, expireIn, role.ToString());
        }

        public string GenerateAccessToken(string subject, TimeSpan expireIn, string role)
        {
            // Getting the key from appsetting.json
            byte[] key = _jwtsettings.SecretKeyBytes;

            // Get the timestamps in utc
            DateTime utcNow = DateTime.UtcNow;
            DateTime expireTime = utcNow + expireIn;

            // Creates the JWT object
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

            // Sign the token.
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Gets the token string that will be used as access token.
            return tokenHandler.WriteToken(token);
        }

        public async Task<RefreshTokenEntity> GenerateRefreshTokenAsync(AccountEntity account)
        {
            var tokenFamily = CreateTokenFamilyEntity(account);

            var refreshToken = await GenerateRefreshTokenAsync(tokenFamily);

            return refreshToken;
        }

        public async Task<RefreshTokenEntity> GenerateRefreshTokenAsync(TokenFamilyEntity tokenFamily)
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeSpan refreshTokenTTL = _refreshTokenSettins.TimeToLive.Time;

            // Generate a byte array for the token, length is assigned through the appsetting.json
            byte[] tokenByteArray = GenerateRandomByteArray(_refreshTokenSettins.RefreshTokenLength);

            // Generate a refresh token.
            var refreshToken = new RefreshTokenEntity()
            {
                Token = Base64UrlEncoder.Encode(tokenByteArray),
                TokenFamily = tokenFamily,
                IssuedTS = utcNow,
                ExpirationTS = utcNow.Add(refreshTokenTTL),
                Used = false
            };

            // Add the token to the database.
            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.CommitAsync();

            // returns it to be sent to the user
            return refreshToken;
        }

        public async Task<RefreshTokenEntity?> GetRefreshTokenAsync(string refreshToken)
        {
            // Gets the refresh token from the database.
            return await _unitOfWork.RefreshTokenRepository.GetByRefreshTokenAsync(refreshToken);
        }

        public async Task MarkRefreshTokenAsUsedAsync(RefreshTokenEntity refreshToken)
        {
            // Mark a token as used and save it on the database.
            refreshToken.Used = true;
            _unitOfWork.RefreshTokenRepository.Update(refreshToken);
            await _unitOfWork.CommitAsync();
        }

        public async Task RevokeTokenFamilyAsync(TokenFamilyEntity tokenFamily, TokenFamilyRevokeReason reason)
        {
            // Set a token as revoked, and save it to the database.
            tokenFamily.RevokeTS = DateTime.UtcNow;
            tokenFamily.RevokeReason = reason;

            _unitOfWork.TokenFamilyRepository.Update(tokenFamily);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> ValidateRefreshTokenAsync(RefreshTokenEntity refreshToken)
        {
            // Is token family marked as revoked?
            if (refreshToken.TokenFamily.RevokeTS is not null)
            {
                return false;
            }

            // Is refresh token used?
            if (refreshToken.Used is true)
            {
                // Revoke whole family, this is family is compromised.
                await RevokeTokenFamilyAsync(refreshToken.TokenFamily, TokenFamilyRevokeReason.Compromised);
                return false;
            }

            // Have token expired?
            if (DateTime.UtcNow.Ticks > refreshToken.ExpirationTS.Ticks)
            {
                // Mark whole family as expired.
                await RevokeTokenFamilyAsync(refreshToken.TokenFamily, TokenFamilyRevokeReason.Expired);
                return false;
            }

            return true;
        }

        private TokenFamilyEntity CreateTokenFamilyEntity(AccountEntity account)
        {
            // Get the length from appsetting, and generate a idenfier of that length
            var tokenIdentifier = GenerateRandomByteArray(_refreshTokenSettins.RefreshFamilyLength);

            // Generate a refresh token family
            var tokenFamily = new TokenFamilyEntity()
            {
                Account = account,
                Identifier = Base64UrlEncoder.Encode(tokenIdentifier)
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
