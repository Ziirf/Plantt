using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Plantt.API.Constants;
using Plantt.Applcation.Services;
using Plantt.Domain.Config;
using Plantt.Domain.DTOs.Hub;
using Plantt.Domain.DTOs.Hub.Request;
using Plantt.Domain.DTOs.Hub.Response;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Services.EntityServices;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HubController : ControllerExtention
    {
        private readonly IHubService _hubService;
        private readonly HubSettings _hubSettings;
        private readonly IMapper _mapper;
        private readonly ITokenAuthenticationService _tokenService;

        public HubController(
            IHubService hubService,
            IOptions<HubSettings> hubSettings,
            IMapper mapper,
            ITokenAuthenticationService tokenService)
        {
            _hubService = hubService;
            _hubSettings = hubSettings.Value;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        // TODO: DELETE, used for testing.
        [HttpPost("ping2")]
        [Authorize(Policy = AuthorizePolicyConstant.Hub)]
        public IActionResult Ping2([FromBody] object requestBody)
        {
            return Ok(requestBody);
        }

        [HttpPost("Data")]
        [Authorize(Policy = AuthorizePolicyConstant.Hub)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PostData([FromBody] DataRequest requestBody)
        {
            await _hubService.SaveDataAsync(requestBody);

            return NoContent();
        }


        [HttpPost("Register")]
        [Authorize(Policy = AuthorizePolicyConstant.Premium)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateHubResponse))]
        public async Task<IActionResult> RegisterHub([FromBody] RegisterHubRequest request)
        {
            HubEntity hubEntity = await _hubService.RegistreHubAsync(request.HomeId, request.Name);

            CreateHubResponse response = _mapper.Map<CreateHubResponse>(hubEntity);

            return Ok(response);
        }

        [HttpGet()]
        [Authorize(Policy = AuthorizePolicyConstant.Premium)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HubDTO))]
        public IActionResult GetHubs([FromQuery] bool includeSecret = false)
        {
            AccountEntity account = GetAccountFromHttpContext();

            IEnumerable<HubEntity> hubEntities = _hubService.GetHubsFromAccount(account);

            if (includeSecret is false)
            {
                foreach (var hubEntity in hubEntities)
                {
                    hubEntity.Secret = string.Empty;
                }
            }

            return Ok(_mapper.Map<IEnumerable<HubDTO>>(hubEntities));
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginHubResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginWithHub([FromBody] LoginHubRequest request)
        {
            bool loginVerified = await _hubService.VerifyHubAsync(request.Identity, request.Secret);

            if (loginVerified is false)
            {
                return Unauthorized(new ProblemDetails()
                {
                    Title = "No match",
                    Detail = "Token or secret wasn't correct.",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            TimeSpan expiresIn = _hubSettings.TokenTimeToLive.Time;

            string accessToken = _tokenService.GenerateAccessToken(request.Identity, expiresIn, "Hub");

            long epochTime = _hubService.GetEpochTime(DateTime.UtcNow + expiresIn);

            var response = new LoginHubResponse()
            {
                Expire = epochTime,
                AccessToken = accessToken
            };

            return Ok(response);
        }
    }
}
