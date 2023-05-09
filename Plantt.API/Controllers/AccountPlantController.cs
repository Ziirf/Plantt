using Microsoft.AspNetCore.Mvc;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/{accountName}/plant")]
    public class AccountPlantController
    {

        [HttpGet()]
        public Task<IActionResult> GetAllAccountPlants()
        {
            // Get all plants, paged

            throw new NotImplementedException();
        }


        [HttpGet("/{id}")]
        public Task<IActionResult> GetAccountPlantById([FromRoute] int id)
        {
            // Get a specific plant

            throw new NotImplementedException();
        }
    }
}
