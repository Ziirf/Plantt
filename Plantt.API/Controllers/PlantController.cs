using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plantt.Domain.DTOs.Plant;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Services.EntityServices;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/")]
    public class PlantController : ControllerBase
    {
        private readonly int[] _pageSizes = { 10, 20, 50, 100 };
        private readonly IPlantService _plantService;
        private readonly IMapper _mapper;

        public PlantController(IPlantService plantService, IMapper mapper)
        {
            _plantService = plantService;
            _mapper = mapper;
        }

        [HttpGet("plants/{page}")]
        public async Task<IActionResult> GetAllPlants([FromRoute] int page, [FromQuery] int pagesize = 20, [FromQuery] bool detailed = false)
        {
            if (page <= 0)
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "Invalid value",
                    Detail = "Page can't be below 1",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            if (!_pageSizes.Contains(pagesize))
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "Invalid value",
                    Detail = "Pagesize must be either 10, 20, 50 or 100",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            IEnumerable<PlantEntity>? plants = await _plantService.GetPlantPage(pagesize, page);

            if (plants is null || plants.Count() == 0)
            {
                return NotFound(new ProblemDetails()
                {
                    Title = "No Plants Found",
                    Detail = "Was not able to find any plants on that page.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            if (detailed is true)
            {
                return Ok(_mapper.Map<IEnumerable<PlantDTO>>(plants));
            }

            return Ok(_mapper.Map<IEnumerable<PlantMinimumDTO>>(plants));
        }

        [HttpGet("plant/{id}")]
        public async Task<IActionResult> GetPlantById([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ProblemDetails()
                {
                    Title = "Invalid value",
                    Detail = "id can't be below 1",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            PlantEntity? plant = await _plantService.GetPlantById(id);

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
