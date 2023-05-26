using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plantt.API.Constants;
using Plantt.Domain.DTOs.AccountPlant;
using Plantt.Domain.DTOs.AccountPlant.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Services.EntityServices;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/My/Plant")]
    public class AccountPlantController : ControllerExtention
    {
        private readonly IAccountPlantService _accountPlantService;
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public AccountPlantController(IAccountPlantService accountPlantService, IRoomService roomService, IMapper mapper)
        {
            _accountPlantService = accountPlantService;
            _roomService = roomService;
            _mapper = mapper;
        }

        [HttpGet()]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AccountPlantDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPlantsFromHomeId([FromQuery] bool detailed = false)
        {
            AccountEntity account = GetAccountFromHttpContext();

            IEnumerable<AccountPlantEntity> accountPlantEntitiesList = _accountPlantService.GetAccountPlantsFromAccount(account.Id);

            if (detailed)
            {
                return Ok(_mapper.Map<IEnumerable<AccountPlantDTO>>(accountPlantEntitiesList));
            }

            return Ok(_mapper.Map<IEnumerable<AccountPlantMinimumDTO>>(accountPlantEntitiesList));
        }

        [HttpGet("room/{roomId}")]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AccountPlantDTO>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPlantsFromRoomId([FromRoute] int roomId, [FromQuery] bool detailed = false)
        {
            AccountEntity account = GetAccountFromHttpContext();

            if (await _roomService.ValidateOwnerAsync(roomId, account.Id) is false)
            {
                return Forbid();
            }

            IEnumerable<AccountPlantEntity> accountPlantEntitiesList = _accountPlantService.GetAccountPlantsFromRoom(roomId);

            if (detailed)
            {
                return Ok(_mapper.Map<IEnumerable<AccountPlantDTO>>(accountPlantEntitiesList));
            }

            return Ok(_mapper.Map<IEnumerable<AccountPlantMinimumDTO>>(accountPlantEntitiesList));
        }

        [HttpGet("{plantId}")]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountPlantDTO))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPlantById([FromRoute] int plantId)
        {
            AccountEntity account = GetAccountFromHttpContext();

            if (await _accountPlantService.ValidateOwnerAsync(plantId, account.Id) is false)
            {
                return Forbid();
            }

            AccountPlantEntity? accountPlantEntityList = await _accountPlantService.GetPlantByIdAsync(plantId);

            return Ok(_mapper.Map<AccountPlantDTO>(accountPlantEntityList));
        }

        [HttpPost()]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountPlantDTO))]
        public async Task<IActionResult> AddPlant([FromBody] UpdateAccountPlantRequest request)
        {
            AccountPlantEntity accountPlantEntity = await _accountPlantService.CreateAccountPlantAsync(request);

            return Ok(_mapper.Map<AccountPlantMinimumDTO>(accountPlantEntity));
        }

        [HttpPut("{plantId}")]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountPlantDTO))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdatePlant([FromRoute] int plantId, [FromBody] UpdateAccountPlantRequest request)
        {
            AccountEntity account = GetAccountFromHttpContext();

            if (await _accountPlantService.ValidateOwnerAsync(plantId, account.Id) is false)
            {
                return Forbid();
            }

            AccountPlantEntity accountPlantEntity = await _accountPlantService.UpdateAccountPlantAsync(request, plantId);

            return Ok(_mapper.Map<AccountPlantMinimumDTO>(accountPlantEntity));
        }

        [HttpDelete("{plantId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        public async Task<IActionResult> DeletePlant([FromRoute] int plantId)
        {
            AccountEntity account = GetAccountFromHttpContext();

            if (await _accountPlantService.ValidateOwnerAsync(plantId, account.Id) is false)
            {
                return Forbid();
            }

            await _accountPlantService.DeleteAccountPlantAsync(plantId);

            return NoContent();
        }
    }
}
