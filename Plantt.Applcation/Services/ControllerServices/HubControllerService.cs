using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Services;
using System;
using System.Collections.Immutable;
using System.Security.Cryptography;

namespace Plantt.Applcation.Services.ControllerServices
{
    public class HubControllerService : IHubControllerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HubControllerService> _logger;

        public HubControllerService(IUnitOfWork unitOfWork, ILogger<HubControllerService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<HubEntity> RegistreHubAsync(int homeId, string name)
        {
            try
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
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }

        public async Task<bool> VerifyHub(string identity, string secret)
        {
            try
            {
                var hubEntity = await _unitOfWork.HubRepository.GetHubByIdentityAsync(identity);

                if (hubEntity == null)
                {
                    return false;
                }

                return hubEntity.Secret == secret;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return false;
            }
        }

        public async Task<IEnumerable<HubEntity>> GetHubsFromAccount(Guid accountGuid)
        {
            try
            {
                AccountEntity? account = await _unitOfWork.AccountRepository.GetByGuidAsync(accountGuid);

                if (account is null)
                {
                    throw new NullReferenceException("Unable to find an account with that public Id");
                }

                IEnumerable<HubEntity> hubs = _unitOfWork.HubRepository.GetHubsFromAccount(account.Id).ToArray();

                return hubs;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
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
