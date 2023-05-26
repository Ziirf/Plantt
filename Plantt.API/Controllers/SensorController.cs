using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plantt.API.Constants;
using Plantt.Domain.DTOs.Sensor;
using Plantt.Domain.DTOs.Sensor.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Services.EntityServices;

namespace Plantt.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SensorController : ControllerExtention
    {
        private readonly ISensorService _sensorService;
        private readonly IHubService _hubService;
        private readonly IMapper _mapper;

        public SensorController(ISensorService sensorService, IHubService hubService, IMapper mapper)
        {
            _sensorService = sensorService;
            _hubService = hubService;
            _mapper = mapper;
        }

        [HttpGet("Hub/{hubId}")]
        [Authorize(Policy = AuthorizePolicyConstant.Premium)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SensorDTO>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllSensors(int hubId)
        {
            AccountEntity account = GetAccountFromHttpContext();

            if (await _hubService.ValidateOwnerAsync(hubId, account.Id) == false)
            {
                return Forbid();
            }

            var sensorEntities = _sensorService.GetHubsSensors(hubId);

            return Ok(_mapper.Map<IEnumerable<SensorDTO>>(sensorEntities));
        }

        [HttpPost("Register")]
        [Authorize(Policy = AuthorizePolicyConstant.Premium)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SensorDTO))]
        public async Task<IActionResult> RegisterSensor([FromBody] UpdateSensorRequest request)
        {
            var sensorEntity = await _sensorService.RegisterSensorAsync(request);

            return Ok(_mapper.Map<SensorDTO>(sensorEntity));
        }

        [HttpPut("{sensorId}")]
        [Authorize(Policy = AuthorizePolicyConstant.Premium)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SensorDTO))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateSensor([FromRoute] int sensorId, [FromBody] UpdateSensorRequest request)
        {
            AccountEntity account = GetAccountFromHttpContext();

            if (await _sensorService.ValidateOwnerAsync(sensorId, account.Id) == false)
            {
                return Forbid();
            }

            var sensorEntity = await _sensorService.UpdateSensorAsync(request, sensorId);

            return Ok(_mapper.Map<SensorDTO>(sensorEntity));
        }

        [HttpDelete("{sensorId}")]
        [Authorize(Policy = AuthorizePolicyConstant.Premium)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteSensor([FromRoute] int sensorId)
        {
            AccountEntity account = GetAccountFromHttpContext();

            if (await _sensorService.ValidateOwnerAsync(sensorId, account.Id) == false)
            {
                return Forbid();
            }

            await _sensorService.DeleteSensorAsync(sensorId);

            return NoContent();
        }
    }
}
