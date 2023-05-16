using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plantt.API.Constants;
using Plantt.Applcation.Services;
using Plantt.Domain.DTOs.Account;
using Plantt.Domain.DTOs.Account.Request;
using Plantt.Domain.DTOs.Account.Response;
using Plantt.Domain.DTOs.RefreshToken;
using Plantt.Domain.Entities;
using Plantt.Domain.Enums;
using Plantt.Domain.Interfaces.Services.EntityServices;
using System.Security.Claims;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ITokenAuthenticationService _tokenService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            ITokenAuthenticationService authenticationService,
            IAccountService accountService,
            IMapper mapper,
            ILogger<AccountController> logger)
        {
            _tokenService = authenticationService;
            _accountService = accountService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("Auth")]
        [Authorize(Policy = AuthorizePolicies.Registered)]
        public IActionResult Auth()
        {
            return Ok();
        }


        [HttpPost("Create")]
        public async Task<IActionResult> CreateNewAccountEndpoint([FromBody] CreateAccountRequest request)
        {
            AccountEntity account = await _accountService.CreateNewAccountAsync(request);

            string accessToken = _tokenService.GenerateAccessToken(account.PublicId.ToString(), account.Role);
            RefreshTokenEntity refreshToken = await _tokenService.GenerateRefreshTokenAsync(account);

            var response = new TokenAuthenticationResponse()
            {
                Account = _mapper.Map<AccountDTO>(account),
                AccessToken = accessToken,
                RefreshToken = _mapper.Map<RefreshTokenDTO>(refreshToken)
            };

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginEndpoint([FromBody] LoginRequest request)
        {
            AccountEntity? account = await _accountService.GetAccountByUsernameAsync(request.Username);

            if (account is null)
            {
                return NotFound(new ProblemDetails()
                {
                    Title = "Account not found.",
                    Detail = "Can't find account with that username.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            if (!_accountService.VerifyPassword(account, request.Password))
            {
                return Unauthorized(new ProblemDetails()
                {
                    Title = "Wrong Password",
                    Detail = "Password didn't match the account.",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            //TODO: Make a threshold on how many attempts you can use, before the account is softlocked, and the owner is warned.

            string accessToken = _tokenService.GenerateAccessToken(account.PublicId.ToString(), account.Role);
            RefreshTokenEntity refreshToken = await _tokenService.GenerateRefreshTokenAsync(account);

            var response = new TokenAuthenticationResponse()
            {
                Account = _mapper.Map<AccountDTO>(account),
                AccessToken = accessToken,
                RefreshToken = _mapper.Map<RefreshTokenDTO>(refreshToken)
            };

            return Ok(response);
        }

        [HttpGet("Logout/{token}")]
        public async Task<IActionResult> LogoutEndpoint([FromRoute] string token)
        {
            RefreshTokenEntity? refreshToken = await _tokenService.GetRefreshTokenAsync(token);

            if (refreshToken is not null)
            {
                await _tokenService.MarkRefreshTokenAsUsedAsync(refreshToken);
                await _tokenService.RevokeTokenFamilyAsync(refreshToken.TokenFamily, TokenFamilyRevokeReason.LoggedOut);

                return NoContent();
            }

            return NotFound(new ProblemDetails()
            {
                Title = "Refreshtoken not found.",
                Detail = "Was unable to find the refreshtoken in the system.",
                Status = StatusCodes.Status404NotFound
            });
        }

        [Authorize]
        [HttpPatch("Upgrade/{role}")]
        public async Task<IActionResult> UpgradeEndpoint([FromRoute] AccountRoles role)
        {
            var accountSubejct = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (accountSubejct is null || !Guid.TryParse(accountSubejct, out Guid accountGuid))
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "No public Id was found",
                    Detail = "Token or secret wasn't correct.",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            var entity = await _accountService.Upgrade(accountGuid, role);

            if (entity is null)
            {
                return NotFound(new ProblemDetails()
                {
                    Title = "Account not found.",
                    Detail = "Can't find account with that username.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(_mapper.Map<AccountDTO>(entity));
        }

        [HttpGet("Refresh/{token}")]
        public async Task<IActionResult> RefreshTokenEndpoint([FromRoute] string token)
        {

            RefreshTokenEntity? refreshToken = await _tokenService.GetRefreshTokenAsync(token);

            if (refreshToken is null)
            {
                return NotFound(new ProblemDetails()
                {
                    Title = "Refreshtoken not found.",
                    Detail = "Was unable to find the refreshtoken in the system.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            bool tokenValid = await _tokenService.ValidateRefreshTokenAsync(refreshToken);

            if (!tokenValid)
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "Invalid Refreshtoken",
                    Detail = "Refreshtoken was invalid. Log back in to get a new one.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            await _tokenService.MarkRefreshTokenAsUsedAsync(refreshToken);

            var account = await _accountService.GetAccountByIdAsync(refreshToken.TokenFamily.AccountId);

            if (account is null)
            {
                return NotFound(new ProblemDetails()
                {
                    Title = "Account not found",
                    Detail = "Unable to find an account linked to this token.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            string accessToken = _tokenService.GenerateAccessToken(account.PublicId.ToString(), account.Role);
            RefreshTokenEntity newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(refreshToken.TokenFamily);

            var response = new TokenAuthenticationResponse()
            {
                Account = _mapper.Map<AccountDTO>(account),
                AccessToken = accessToken,
                RefreshToken = _mapper.Map<RefreshTokenDTO>(newRefreshToken)
            };

            return Ok(response);
        }
    }
}
