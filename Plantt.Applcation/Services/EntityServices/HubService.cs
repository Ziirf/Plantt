using Microsoft.IdentityModel.Tokens;
using Plantt.Domain.DTOs.Hub.Request;
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

        public HubService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SaveDataAsync(DataRequest data)
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
                CreatedTS = DateTime.UtcNow,
                AccountPlantId = (int)sensorEntity.AccountPlantId,
                Humidity = data.Humidity,
                Lux = data.Lux,
                Moisture = data.Moisture,
                TemperatureC = data.Temperature
            };

            await _unitOfWork.PlantDataRepository.AddAsync(plantData);
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

        public async Task<bool> VerifyHubAsync(string identity, string secret)
        {
            var hubEntity = await _unitOfWork.HubRepository.GetHubByIdentityAsync(identity);

            if (hubEntity is null)
            {
                return false;
            }

            return hubEntity.Secret == secret;
        }

        public IEnumerable<HubEntity> GetHubsFromAccount(AccountEntity account)
        {
            return _unitOfWork.HubRepository.GetHubsFromAccount(account.Id).ToArray();
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
    }
}
