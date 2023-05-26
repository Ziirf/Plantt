using AutoMapper;
using Plantt.Domain.DTOs.Sensor.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Exceptions;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Services.EntityServices;

namespace Plantt.Applcation.Services.EntityServices
{
    public class SensorService : ISensorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SensorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<SensorEntity> GetHubsSensors(int hubId)
        {
            return _unitOfWork.SensorRepository.GetAllFromHub(hubId);
        }

        public async Task<SensorEntity> RegisterSensorAsync(UpdateSensorRequest request)
        {
            var sensor = _mapper.Map<SensorEntity>(request);

            await _unitOfWork.SensorRepository.AddAsync(sensor);
            await _unitOfWork.CommitAsync();

            return sensor;
        }

        public async Task<SensorEntity> UpdateSensorAsync(UpdateSensorRequest request, int sensorId)
        {
            var sensor = await _unitOfWork.SensorRepository.GetByIdAsync(sensorId);

            if (sensor is null)
            {
                throw new NoEntryFoundException();
            }

            sensor.Name = request.Name;
            sensor.HubId = request.HubId;
            sensor.AccountPlantId = request.MyPlantId;

            _unitOfWork.SensorRepository.Update(sensor);
            await _unitOfWork.CommitAsync();

            return sensor;
        }

        public async Task DeleteSensorAsync(int sensorId)
        {
            var sensor = await _unitOfWork.SensorRepository.GetByIdAsync(sensorId);

            if (sensor is null)
            {
                throw new NoEntryFoundException();
            }

            _unitOfWork.SensorRepository.Delete(sensor);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> ValidateOwnerAsync(int sensorId, int accountId)
        {
            return await _unitOfWork.SensorRepository.IsValidOwnerAsync(sensorId, accountId);
        }
    }
}
