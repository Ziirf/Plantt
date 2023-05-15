

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plantt.API.Constants;
using Plantt.Applcation.Services;
using Plantt.Domain.DTOs.Hub;
using Plantt.Domain.DTOs.Hub.Request;
using Plantt.Domain.DTOs.Hub.Response;
using Plantt.Domain.Entities;
using Plantt.Domain.Enums;
using Plantt.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HubController : ControllerBase
    {
        private readonly IHubControllerService _hubService;
        private readonly ILogger<HubController> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenAuthenticationService _tokenService;
        private readonly IAccountControllerService _accountService;

        public HubController(
            IHubControllerService hubService,
            ILogger<HubController> logger,
            IMapper mapper,
            ITokenAuthenticationService tokenService,
            IAccountControllerService accountService)
        {
            _hubService = hubService;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _accountService = accountService;
        }

        [HttpPost("ping")]
        public IActionResult Ping([FromRoute] int id, [FromBody] object requestBody)
        {
            _logger.LogInformation("The body is {body}", requestBody.ToString());
            return Ok(requestBody);
        }

        [HttpPost("test/{id}")]
        public IActionResult PostHub([FromRoute] int id, [FromBody] dynamic requestBody)
        {
            return Ok(requestBody);
        }

        [Authorize]
        [Authorize(Policy = AuthorizePolicies.Premium)]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterHub([FromBody] CreateHubRequest request)
        {
            // TODO: Check that account is owner of that home, from the User property.

            var hubEntity = await _hubService.RegistreHubAsync(request.HomeId, request.Name);

            var response = _mapper.Map<CreateHubResponse>(hubEntity);

            return Ok(response);
        }

        [Authorize]
        [Authorize(Policy = AuthorizePolicies.Premium)]
        [HttpGet()]
        public async Task<IActionResult> GetHubs([FromQuery] bool includeSecret = false)
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

            IEnumerable<HubEntity> hubEntities = await _hubService.GetHubsFromAccount(accountGuid);

            if (includeSecret)
            {
                return Ok(_mapper.Map< IEnumerable<HubWithSecretDTO>>(hubEntities));
            }
            
            return Ok(_mapper.Map< IEnumerable<HubDTO>>(hubEntities));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginWithHub([FromBody] LoginHubRequest request)
        {
            bool loginVerified = await _hubService.VerifyHub(request.Identity, request.Secret);

            if (loginVerified is false)
            {
                return Unauthorized(new ProblemDetails()
                {
                    Title = "No match",
                    Detail = "Token or secret wasn't correct.",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            TimeSpan expiresIn = TimeSpan.FromHours(1);
            var accessToken = _tokenService.GenerateAccessToken(request.Identity, expiresIn, "Token");

            return Ok(new LoginHubResponse() { ExpireTs = DateTime.UtcNow + expiresIn, accessToken = accessToken});
        }
    }
}
