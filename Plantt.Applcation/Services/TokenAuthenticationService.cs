using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Plantt.DataAccess.EntityFramework;
using Plantt.Domain.Config;
using Plantt.Domain.Entities;
using Plantt.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Plantt.Applcation.Services
{
    public class TokenAuthenticationService : ITokenAuthenticationService
    {
        private readonly JsonWebTokenSettings _jwtsettings;
        private readonly RefreshTokenSettings _refreshTokenSettins;
        private readonly PlanttDbContext _planttDbContext;

        public TokenAuthenticationService(
            IOptions<JsonWebTokenSettings> jwtsettings,
            IOptions<RefreshTokenSettings> refreshTokenSettins,
            PlanttDbContext planttDbContext)
        {
            _jwtsettings = jwtsettings.Value;
            _refreshTokenSettins = refreshTokenSettins.Value;
            _planttDbContext = planttDbContext;
        }

        public string GenerateAccessToken(Guid guid)
        {
            byte[] key = _jwtsettings.SecretKeyBytes;
            DateTime utcNow = DateTime.UtcNow;
            DateTime expireTime = utcNow.AddMinutes(_jwtsettings.MinutesToLive);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, guid.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iss, _jwtsettings.Issuer),
                    new Claim("role", "Free")
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

        public async Task<RefreshTokenEntity> GenerateRefreshTokenAsync(AccountEntity account)
        {
            var tokenFamily = CreateTokenFamilyEntity(account);

            var refreshToken = await GenerateRefreshTokenAsync(account, tokenFamily);

            return refreshToken;
        }

        public async Task<RefreshTokenEntity> GenerateRefreshTokenAsync(AccountEntity account, TokenFamilyEntity tokenFamily)
        {
            DateTime utcNow = DateTime.UtcNow;

            if (tokenFamily.Account.Id != account.Id)
            {
                throw new InvalidOperationException("Account does not match with tokenfamily");
            }

            RefreshTokenEntity refreshToken = new RefreshTokenEntity()
            {
                Token = Base64UrlEncoder.Encode(GenerateRanomByteArray(_refreshTokenSettins.RefreshTokenLength)),
                TokenFamily = tokenFamily,
                IssuedTS = utcNow,
                ExpirationTS = utcNow.AddDays(_refreshTokenSettins.DaysToLive),
                Used = false
            };

            await _planttDbContext.RefreshTokens.AddAsync(refreshToken);
            await _planttDbContext.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<RefreshTokenEntity?> GetRefreshToken(string refreshToken, int accountId)
        {
            RefreshTokenEntity? storedToken = await _planttDbContext.RefreshTokens
                .Include(token => token.TokenFamily)
                .ThenInclude(tokenFamily => tokenFamily.Account)
                .FirstOrDefaultAsync(token => token.TokenFamily.FK_Account_Id == accountId && token.Token == refreshToken);

            return storedToken;
        }

        public async Task<RefreshTokenEntity?> GetRefreshToken(string refreshToken, Guid accountId)
        {
            RefreshTokenEntity? storedToken = await _planttDbContext.RefreshTokens
                .Include(token => token.TokenFamily)
                .ThenInclude(tokenFamily => tokenFamily.Account)
                .FirstOrDefaultAsync(token => token.TokenFamily.Account.PublicId == accountId && token.Token == refreshToken);

            return storedToken;
        }

        public async Task MarkRefreshTokenAsUsedAsync(RefreshTokenEntity refreshToken)
        {
            refreshToken.Used = true;

            _planttDbContext.RefreshTokens.Update(refreshToken);
            await _planttDbContext.SaveChangesAsync();
        }

        public async Task RevokeTokenFamilyAsync(TokenFamilyEntity tokenFamily, TokenFamilyRevokeReason reason)
        {
            tokenFamily.RevokeTS = DateTime.UtcNow;
            tokenFamily.RevokeReason = reason;

            _planttDbContext.TokenFamilies.Update(tokenFamily);
            await _planttDbContext.SaveChangesAsync();
        }

        private TokenFamilyEntity CreateTokenFamilyEntity(AccountEntity account)
        {
            TokenFamilyEntity tokenFamily = new TokenFamilyEntity()
            {
                Account = account,
                Identifier = Base64UrlEncoder.Encode(GenerateRanomByteArray(_refreshTokenSettins.RefreshFamilyLength))
            };

            return tokenFamily;
        }

        private byte[] GenerateRanomByteArray(int byteLength)
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
