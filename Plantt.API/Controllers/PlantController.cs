using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Plantt.Domain.DTOs.Plant;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Services.EntityServices;
using Plantt.Domain.QueryParams;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/")]
    public class PlantController : ControllerBase
    {
        private readonly IPlantService _plantService;
        private readonly IMapper _mapper;

        public PlantController(IPlantService plantService, IMapper mapper)
        {
            _plantService = plantService;
            _mapper = mapper;
        }

        [HttpGet("plants/{page}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlantDTO))]
        public async Task<IActionResult> GetAllPlants([FromRoute] int page, [FromQuery] PlantQueryParams query)
        {
            IEnumerable<PlantEntity>? plants = await _plantService.GetPlantPageAsync(query.PageSize, page, query.Search);

            if (query.Detailed is true)
            {
                return Ok(_mapper.Map<IEnumerable<PlantDTO>>(plants));
            }

            return Ok(_mapper.Map<IEnumerable<PlantMinimumDTO>>(plants));
        }

        [HttpGet("plant/{plantId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlantDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlantById([FromRoute] int plantId)
        {
            if (plantId <= 0)
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "Invalid value",
                    Detail = "id can't be below 1",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            PlantEntity? plant = await _plantService.GetPlantByIdAsync(plantId);

            if (plant is null)
            {
                return NotFound(new ProblemDetails()
                {
                    Title = "No Plants Found",
                    Detail = "Was not able to find a plant with that id.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(_mapper.Map<PlantDTO>(plant));
        }
    }
}
