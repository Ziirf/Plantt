using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            ITokenAuthenticationService authenticationService,
            IAccountControllerService accountService,
            IMapper mapper,
            ILogger<AccountController> logger)
        {
            _tokenService = authenticationService;
            _accountService = accountService;
            _mapper = mapper;
            _logger = logger;
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
            RefreshTokenEntity? refreshToken = await _tokenService.GetRefreshToken(token);

            if (refreshToken is not null)
            {
                await _tokenService.MarkRefreshTokenAsUsedAsync(refreshToken);
                await _tokenService.RevokeTokenFamilyAsync(refreshToken.TokenFamily, TokenFamilyRevokeReason.LoggedOut);

                return NoContent();
            }

            return NotFound();
        }

        [HttpGet("Refresh/{token}")]
        public async Task<IActionResult> RefreshTokenEndpoint([FromRoute] string token)
        {

            RefreshTokenEntity? refreshToken = await _tokenService.GetRefreshToken(token);

            if (refreshToken is null)
            {
                return NotFound("Token couldn't be found.");
            }

            if (refreshToken.TokenFamily.RevokeTS is not null)
            {
                return Unauthorized("Token has been revoked.");
            }

            if (refreshToken.Used is true)
            {
                await _tokenService.RevokeTokenFamilyAsync(refreshToken.TokenFamily, TokenFamilyRevokeReason.Compromised);
                return Unauthorized("Token have previously been used, and is now marked as compromised.");
            }

            if (DateTime.UtcNow.Ticks > refreshToken.ExpirationTS.Ticks)
            {
                await _tokenService.RevokeTokenFamilyAsync(refreshToken.TokenFamily, TokenFamilyRevokeReason.Expired);
                return Unauthorized("Token have expired.");
            }

            await _tokenService.MarkRefreshTokenAsUsedAsync(refreshToken);

            var account = await _accountService.GetAccountByIdAsync(refreshToken.TokenFamily.FK_Account_Id);


            if (account is null)
            {
                return NotFound("Was unable to find an account linked to this token.");
            }

            string accessToken = _tokenService.GenerateAccessToken(account.PublicId);
            RefreshTokenEntity newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(refreshToken.TokenFamily);

            var response = new TokenAuthenticationResponse()
            {
                Account = _mapper.Map<AccountDto>(account),
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("validate")]
        public IActionResult Validate()
        {
            return Ok();
        }
    }
}
