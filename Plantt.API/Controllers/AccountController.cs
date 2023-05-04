using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Plantt.Applcation.Services;
using Plantt.Domain.DTOs.Account;
using Plantt.Domain.DTOs.Reponse;
using Plantt.Domain.DTOs.Requests;
using Plantt.Domain.Entities;
using Plantt.Domain.Enums;
using Plantt.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ITokenAuthenticationService _tokenService;
        private readonly IAccountControllerService _accountService;
        private readonly IMapper _mapper;

        public AccountController(
            ITokenAuthenticationService authenticationService,
            IAccountControllerService accountService,
            IMapper mapper)
        {
            _tokenService = authenticationService;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateNewAccountEndpoint([FromBody] CreateAccountRequest request)
        {
            AccountEntity account = await _accountService.CreateNewAccountAsync(request);

            string accessToken = _tokenService.GenerateAccessToken(account.PublicId);
            RefreshTokenEntity refreshToken = await _tokenService.GenerateRefreshTokenAsync(account);

            var response = new TokenAuthenticationResponse()
            {
                Account = _mapper.Map<AccountDto>(account),
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginEndpoint([FromBody] LoginRequest request)
        {
            AccountEntity? account = await _accountService.GetAccountByUsernameAsync(request.Username);

            if (account is null)
            {
                return NotFound("Can't find account with that username.");
            }

            if (!_accountService.VerifyPassword(account, request.Password))
            {
                return Unauthorized("Password didn't match the account.");
            }

            //TODO: Make a threshold on how many attempts you can use, before the account is softlocked, and the owner is warned.

            string accessToken = _tokenService.GenerateAccessToken(account.PublicId);
            RefreshTokenEntity refreshToken = await _tokenService.GenerateRefreshTokenAsync(account);

            var response = new TokenAuthenticationResponse()
            {
                Account = _mapper.Map<AccountDto>(account),
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };

            return Ok(response);
        }

        [HttpGet("Logout/{token}")]
        public async Task<IActionResult> LogoutEndpoint([FromRoute] string token)
        {
            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Guid.TryParse(identity, out var publicId))
            {
                RefreshTokenEntity? refreshToken = await _tokenService.GetRefreshToken(token, publicId);

                if (refreshToken is not null)
                {
                    await _tokenService.MarkRefreshTokenAsUsedAsync(refreshToken);
                    await _tokenService.RevokeTokenFamilyAsync(refreshToken.TokenFamily, TokenFamilyRevokeReason.LoggedOut);

                    return Ok();
                }
            }

            return NotFound();
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshTokenEndpoint([FromBody] RefreshTokenRequest request)
        {
            AccountEntity? account = await _accountService.GetAccountByGuidAsync(request.AccountPublicId);

            if (account is null)
            {
                return NotFound("Couldn't find an account with this ID.");
            }

            RefreshTokenEntity? refreshToken = await _tokenService.GetRefreshToken(request.Token, account.Id);

            if (refreshToken is null)
            {
                return NotFound("Couldn't find refreshtoken in database");
            }

            if (refreshToken.TokenFamily.RevokeTS is not null)
            {
                return Unauthorized("This token family have been revoked");
            }

            if (refreshToken.Used is true)
            {
                await _tokenService.RevokeTokenFamilyAsync(refreshToken.TokenFamily, TokenFamilyRevokeReason.Compromised);
                return Unauthorized("This token have already been used");
            }

            if (DateTime.UtcNow.Ticks > refreshToken.ExpirationTS.Ticks)
            {
                await _tokenService.RevokeTokenFamilyAsync(refreshToken.TokenFamily, TokenFamilyRevokeReason.Expired);
                return Unauthorized("This token have expired");
            }

            await _tokenService.MarkRefreshTokenAsUsedAsync(refreshToken);

            string accessToken = _tokenService.GenerateAccessToken(account.PublicId);
            RefreshTokenEntity newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(account, refreshToken.TokenFamily);

            var response = new TokenAuthenticationResponse()
            {
                Account = _mapper.Map<AccountDto>(account),
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token
            };

            return Ok(response);
        }
    }
}
