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

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerExtention
    {
        private readonly ITokenAuthenticationService _tokenService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(
            ITokenAuthenticationService authenticationService,
            IAccountService accountService,
            IMapper mapper)
        {
            _tokenService = authenticationService;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenAuthenticationResponse))]
        public async Task<IActionResult> CreateNewAccountEndpoint([FromBody] CreateAccountRequest request)
        {
            // Create account

            AccountEntity account = await _accountService.CreateAccountAsync(request);

            // Generate access token and refresh token for credentials

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenAuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginEndpoint([FromBody] LoginRequest request)
        {
            // Verify login

            AccountEntity? account = await _accountService.GetAccountByUsernameAsync(request.Username);

            if (account is null || _accountService.VerifyPassword(account, request.Password) is not true)
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "Invalid Login",
                    Detail = "Username and password didn't match.",
                    Status = StatusCodes.Status403Forbidden
                });
            }

            // Generate access token and refresh token for credentials

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LogoutEndpoint([FromRoute] string token)
        {
            // Find the token in the database.

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

            // Mark it as invalid so it can't be missused.

            await _tokenService.MarkRefreshTokenAsUsedAsync(refreshToken);
            await _tokenService.RevokeTokenFamilyAsync(refreshToken.TokenFamily, TokenFamilyRevokeReason.LoggedOut);

            return NoContent();
        }

        [HttpPatch("Upgrade/{role}")]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountDTO))]
        public async Task<IActionResult> UpgradeEndpoint([FromRoute] AccountRoles role)
        {
            // Get account, upgrade it, and return the newe account. - it wont take effect before they refresh their access token.

            AccountEntity account = GetAccountFromHttpContext();

            await _accountService.ChangeAccountRoleAsync(account, role);

            return Ok(_mapper.Map<AccountDTO>(account));
        }

        [HttpDelete()]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAccount([FromRoute] AccountRoles role)
        {
            // Get account, Delete it, and return 204 statuscode.

            AccountEntity account = GetAccountFromHttpContext();

            await _accountService.DeleteAccount(account);

            return NoContent();
        }

        [HttpGet("Refresh/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenAuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RefreshTokenEndpoint([FromRoute] string token)
        {
            // Get the refresh token from the database for validation.

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

            // Check if the token is valid, checking if token is used, expired or have been revoked.

            bool isTokenValid = await _tokenService.ValidateRefreshTokenAsync(refreshToken);

            if (isTokenValid is false)
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "Invalid Refreshtoken",
                    Detail = "Refreshtoken was invalid. Log back in to get a new one.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            AccountEntity? account = await _accountService.GetAccountByIdAsync(refreshToken.TokenFamily.AccountId);

            if (account is null)
            {
                return NotFound(new ProblemDetails()
                {
                    Title = "Account not found",
                    Detail = "Unable to find an account linked to this token.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            // Generating new access token, and refresh token.
            string accessToken = _tokenService.GenerateAccessToken(account.PublicId.ToString(), account.Role);
            RefreshTokenEntity newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(refreshToken.TokenFamily);

            // Mark the token as used, in case of reuse, this would compromise the security of the account.
            await _tokenService.MarkRefreshTokenAsUsedAsync(refreshToken);

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
