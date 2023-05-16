using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plantt.API.Constants;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/")]
    public class RoomController : ControllerBase
    {
        public RoomController()
        {

        }

        [Authorize(Policy = AuthorizePolicies.Registered)]
        [HttpGet()]
        public async Task<IActionResult> GetAccountRooms()
        {
            return Ok();
        }
    }
}
