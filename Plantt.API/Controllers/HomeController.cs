using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plantt.API.Constants;
using Plantt.Domain.DTOs.Home;
using Plantt.Domain.DTOs.Home.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Services.EntityServices;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HomeController : ControllerExtention
    {
        private readonly IHomeService _homeService;
        private readonly IMapper _mapper;

        public HomeController(IHomeService homeService, IMapper mapper)
        {
            _homeService = homeService;
            _mapper = mapper;
        }

        [HttpGet()]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HomeDTO>))]
        public IActionResult GetAccountHomes()
        {
            AccountEntity account = GetAccountFromHttpContext();

            IEnumerable<HomeEntity> homeEntities = _homeService.GetAllAccountHomes(account.Id);

            return Ok(_mapper.Map<IEnumerable<HomeDTO>>(homeEntities));
        }

        [HttpGet("{homeId}")]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HomeDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountHomeById([FromRoute] int homeId)
        {
            AccountEntity account = GetAccountFromHttpContext();

            if (await _homeService.ValidateOwnerAsync(homeId, account.Id) is false)
            {
                return Forbid();
            }

            HomeEntity? homeEntity = await _homeService.GetHomeByIdAsync(homeId);

            if (homeEntity is null)
            {
                return NotFound(new ProblemDetails()
                {
                    Title = "No home found",
                    Detail = "No home with that id was found under this account.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(_mapper.Map<HomeDTO>(homeEntity));
        }

        [HttpPost()]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HomeDTO))]
        public async Task<IActionResult> CreateHome([FromBody] UpdateHomeRequest request)
        {
            AccountEntity account = GetAccountFromHttpContext();

            HomeEntity homeEntity = await _homeService.CreateHomeAsync(request, account.Id);

            return Ok(_mapper.Map<HomeDTO>(homeEntity));
        }

        [HttpPut("{homeId}")]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HomeDTO))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateHome([FromRoute] int homeId, [FromBody] UpdateHomeRequest request)
        {
            AccountEntity account = GetAccountFromHttpContext();

            if (await _homeService.ValidateOwnerAsync(homeId, account.Id) is false)
            {
                return Forbid();
            }

            HomeEntity homeEntity = await _homeService.UpdateHomeAsync(request, homeId);

            return Ok(_mapper.Map<HomeDTO>(homeEntity));
        }

        [HttpDelete("{homeId}")]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteHome([FromRoute] int homeId)
        {
            AccountEntity account = GetAccountFromHttpContext();

            if (await _homeService.ValidateOwnerAsync(homeId, account.Id) is false)
            {
                return Forbid();
            }

            await _homeService.DeleteHomeAsync(homeId);

            return NoContent();
        }
    }
}
