using Microsoft.AspNetCore.Mvc;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/{username}/[controller]")]
    public class PlantController : ControllerBase
    {
        [HttpGet()]
        public Task<IActionResult> GetAllPlants()
        {
            // Get all plants, paged

            throw new NotImplementedException();
        }


        [HttpGet("{id}")]
        public Task<IActionResult> GetPlantById([FromRoute]int id)
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

        //[HttpGet("{username}/[controller]")]
        //public Task<IActionResult> GetAllAccountPlants([FromRoute] string username)
        //{
        //    // Get all plants, paged

        //    throw new NotImplementedException();
        //}
    }
}
