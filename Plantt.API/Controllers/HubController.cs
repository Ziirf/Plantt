using Microsoft.AspNetCore.Mvc;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HubController
    {
        [HttpGet("Test")]
        public Task<IActionResult> Test()
        {
            // Get all plants, paged

            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetPlantById([FromRoute] int id)
        {
            // Get a specific plant

            throw new NotImplementedException();
        }

        [HttpGet("{slug}")]
        public Task<IActionResult> GetPlantBySlug([FromRoute] string slug)
        {
            // Get a specific plant

            throw new NotImplementedException();
        }
    }
}
