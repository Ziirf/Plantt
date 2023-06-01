using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Plantt.Domain.DTOs.PlantData.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Exceptions;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Services.EntityServices;
using System.Security.Cryptography;

namespace Plantt.Applcation.Services.EntityServices
{
    public class HubService : IHubService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HubService> _logger;

        public HubService(IUnitOfWork unitOfWork, ILogger<HubService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task SaveDataAsync(SendDataRequest data)
        {
            var sensorEntity = _unitOfWork.SensorRepository.GetById(data.SensorId);

            if (sensorEntity is null)
            {
                throw new NoEntryFoundException("No sensor found");
            }

            if (sensorEntity.AccountPlantId is null)
            {
                throw new NoEntryFoundException("No sensor plant attached to sensor");
            }

            var plantData = new PlantDataEntity()
            {
                CreatedTS = GetDateTimeFromEpoch(data.TimeStamp),
                AccountPlantId = (int)sensorEntity.AccountPlantId,
                Humidity = data.Humidity,
                Lux = data.Lux,
                Moisture = data.Moisture,
                TemperatureC = data.Temperature
            };

            await _unitOfWork.PlantDataRepository.AddAsync(plantData);
            await _unitOfWork.CommitAsync();
        }

        public async Task SaveDataAsync(IEnumerable<SendDataRequest> data)
        {
            var dataPoints = new List<PlantDataEntity>();

            foreach (var dataItem in data)
            {
                var sensorEntity = _unitOfWork.SensorRepository.GetById(dataItem.SensorId);

                if (sensorEntity is null)
                {
                    throw new NoEntryFoundException($"No sensor with id {dataItem.SensorId} found");
                }

                if (sensorEntity.AccountPlantId is null)
                {
                    throw new NoEntryFoundException($"No plant attached to sensor with id {dataItem.SensorId}");
                }

                var entry = new PlantDataEntity()
                {
                    CreatedTS = GetDateTimeFromEpoch(dataItem.TimeStamp),
                    AccountPlantId = (int)sensorEntity.AccountPlantId,
                    Humidity = dataItem.Humidity,
                    Lux = dataItem.Lux,
                    Moisture = dataItem.Moisture,
                    TemperatureC = dataItem.Temperature
                };

                dataPoints.Add(entry);

                _logger.LogInformation("The CreatedTS looks like this: {CreatedTS}", entry.CreatedTS);
            }

            await _unitOfWork.PlantDataRepository.AddRangeAsync(dataPoints);
            await _unitOfWork.CommitAsync();
        }

        public async Task<HubEntity> RegistreHubAsync(int homeId, string name)
        {
            var identity = GenerateRandomUrlEncodedString(24);
            var secret = GenerateRandomUrlEncodedString(64);

            var hub = new HubEntity()
            {
                HomeId = homeId,
                Name = name,
                Identity = identity,
                Secret = secret
            };

            await _unitOfWork.HubRepository.AddAsync(hub);
            await _unitOfWork.CommitAsync();

            return hub;
        }

        public IEnumerable<HubEntity> GetHubsFromAccount(AccountEntity account)
        {
            return _unitOfWork.HubRepository.GetHubsFromAccount(account.Id).ToArray();
        }

        public async Task<bool> VerifyHubAsync(string identity, string secret)
        {
            var hubEntity = await _unitOfWork.HubRepository.GetHubByIdentityAsync(identity);

            if (hubEntity is null)
            {
                return false;
            }

            return hubEntity.Secret == secret;
        }

        public long GetEpochTime(DateTime date)
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = date - epochStart;

            return (long)timeSpan.TotalSeconds;
        }

        public async Task<bool> ValidateOwnerAsync(int hubId, int accountId)
        {
            return await _unitOfWork.HubRepository.IsValidOwnerAsync(hubId, accountId);
        }

        public bool IsSensorChildOfHub(int hubId, params int[] sensorId)
        {
            return _unitOfWork.HubRepository.IsSensorsChildOfHubAsync(hubId, sensorId);
        }

        private DateTime GetDateTimeFromEpoch(long epochTime)
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = epochStart.AddSeconds(epochTime);

            return date;
        }

        private string GenerateRandomUrlEncodedString(int length)
        {
            byte[] bytes = new byte[length];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(bytes);
            }

            string randomString = Base64UrlEncoder.Encode(bytes);

            return randomString.Substring(0, length);
        }

        private bool IsDataCorrupt(SendDataRequest request)
        {
            if (request is null)
            {
                return true;
            }

            if (request.Moisture <= 0 && request.Lux <= 0 && request.Humidity <= 0)
            {
                return true;
            }


            return false;
        }
    }
}
