using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plantt.API.Constants;
using Plantt.Domain.DTOs.Room;
using Plantt.Domain.DTOs.Room.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Services.EntityServices;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RoomController : ControllerExtention
    {
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public RoomController(IRoomService roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        [HttpGet()]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoomDTO>))]
        public IActionResult GetAccountRooms()
        {
            AccountEntity account = GetAccountFromHttpContext();

            var roomEntities = _roomService.GetAccountsRooms(account.Id);

            return Ok(_mapper.Map<IEnumerable<RoomDTO>>(roomEntities));
        }

        [HttpGet("{roomId}")]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoomDTO))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountRoomsById(int roomId)
        {
            AccountEntity account = GetAccountFromHttpContext();

            if (await _roomService.ValidateOwnerAsync(roomId, account.Id) is false)
            {
                return Forbid();
            }

            RoomEntity? roomEntity = await _roomService.GetRoomByIdAsync(roomId);

            if (roomEntity is null)
            {
                return NotFound(new ProblemDetails()
                {
                    Title = "Room not found",
                    Detail = "Couldn't find a room with this id associated with this account.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(_mapper.Map<RoomDTO>(roomEntity));
        }


        [HttpPost()]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoomDTO))]
        public async Task<IActionResult> CreateRoom([FromBody] UpdateRoomRequest request)
        {
            var roomEntity = await _roomService.CreateRoomAsync(request);

            return Ok(_mapper.Map<RoomDTO>(roomEntity));
        }


        [HttpPut("{roomId}")]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoomDTO))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRoom([FromRoute] int roomId, [FromBody] UpdateRoomRequest request)
        {
            AccountEntity account = GetAccountFromHttpContext();

            if (await _roomService.ValidateOwnerAsync(roomId, account.Id) is false)
            {
                return Forbid();
            }

            var roomEntity = await _roomService.GetRoomByIdAsync(roomId);

            if (roomEntity is null)
            {
                return NotFound(new ProblemDetails()
                {
                    Title = "No Room Found",
                    Detail = "Couldn't find a room with given Id on this account.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            roomEntity = await _roomService.UpdateRoomAsync(request, roomEntity);

            return Ok(_mapper.Map<RoomDTO>(roomEntity));
        }

        [HttpDelete("{roomId}")]
        [Authorize(Policy = AuthorizePolicyConstant.Registered)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            AccountEntity account = GetAccountFromHttpContext();

            if (await _roomService.ValidateOwnerAsync(roomId, account.Id) is false)
            {
                return Forbid();
            }

            RoomEntity? roomEntity = await _roomService.GetRoomByIdAsync(roomId);

            if (roomEntity is null)
            {
                return NotFound(new ProblemDetails()
                {
                    Title = "No Room Found",
                    Detail = "Couldn't find a room with given Id on this account.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            await _roomService.DeleteRoomAsync(roomEntity);

            return NoContent();
        }
    }
}
