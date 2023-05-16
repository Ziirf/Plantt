using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Plantt.API.Constants;
using Plantt.Domain.DTOs.Home;
using Plantt.Domain.DTOs.Home.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IHomeControllerService _homeService;
        private readonly IMapper _mapper;

        public HomeController(IHomeControllerService homeService, IMapper mapper)
        {
            _homeService = homeService;
            _mapper = mapper;
        }

        [Authorize(Policy = AuthorizePolicies.Registered)]
        [HttpGet()]
        public async Task<IActionResult> GetAccountHomes()
        {
            var accountSubject = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (accountSubject is null || !Guid.TryParse(accountSubject, out Guid accountGuid))
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "Invalid guid",
                    Detail = "Guid in token is invalid.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            IEnumerable<HomeEntity> homeEntities = await _homeService.GetAccountHomesAsync(accountGuid);

            return Ok(_mapper.Map<IEnumerable<HomeDTO>>(homeEntities));
        }

        [Authorize(Policy = AuthorizePolicies.Registered)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountHomeById(int id)
        {
            var accountSubject = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (accountSubject is null || !Guid.TryParse(accountSubject, out Guid accountGuid))
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "Invalid guid",
                    Detail = "Guid in token is invalid.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            HomeEntity? homeEntity = await _homeService.GetAccountHomeByIdAsync(accountGuid, id);

            if (homeEntity is null)
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "No home found",
                    Detail = "No home with that id was found under this account.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(_mapper.Map<HomeDTO>(homeEntity));
        }

        [Authorize(Policy = AuthorizePolicies.Registered)]
        [HttpPost()]
        public async Task<IActionResult> CreateHome([FromBody] CreateHomeRequest request)
        {
            var accountSubejct = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (accountSubejct is null || !Guid.TryParse(accountSubejct, out Guid accountGuid))
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "Invalid guid",
                    Detail = "Guid in token is invalid.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            HomeEntity homeEntity = await _homeService.CreateHomeAsync(request, accountGuid);

            return Ok(homeEntity);
        }

        [Authorize(Policy = AuthorizePolicies.Registered)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHome([FromBody] UpdateHomeRequest request, int id)
        {
            var accountSubject = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (accountSubject is null || !Guid.TryParse(accountSubject, out Guid accountGuid))
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "Invalid guid",
                    Detail = "Guid in token is invalid.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            HomeEntity homeEntity = await _homeService.UpdateHomeAsync(request, id, accountGuid);

            return Ok(homeEntity);
        }

        [Authorize(Policy = AuthorizePolicies.Registered)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHome(int id)
        {
            var accountSubject = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (accountSubject is null || !Guid.TryParse(accountSubject, out Guid accountGuid))
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "Invalid guid",
                    Detail = "Guid in token is invalid.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            await _homeService.DeleteHomeAsync(id, accountGuid);

            return NoContent();
        }
    }
}
