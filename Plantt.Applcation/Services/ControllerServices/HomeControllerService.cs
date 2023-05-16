using Plantt.Domain.DTOs.Home.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Exceptions;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.Applcation.Services.ControllerServices
{
    public class HomeControllerService : IHomeControllerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeControllerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HomeEntity>> GetAccountHomesAsync(Guid accountGuid)
        {
            var account = await _unitOfWork.AccountRepository.GetByGuidAsync(accountGuid);

            if (account is null)
            {
                throw new AccountNotFoundException($"No account found with the guid: {accountGuid}");
            }

            IEnumerable<HomeEntity> homes = _unitOfWork.HomeRepository.GetAccountHome(account.Id).ToList();

            return homes;
        }

        public async Task<HomeEntity?> GetAccountHomeByIdAsync(Guid accountGuid, int homeId)
        {
            var account = await _unitOfWork.AccountRepository.GetByGuidAsync(accountGuid);

            if (account is null)
            {
                throw new AccountNotFoundException($"No account found with the guid: {accountGuid}");
            }

            HomeEntity? home = await _unitOfWork.HomeRepository.GetAccountHomeByIdAsync(account.Id, homeId);

            return home;
        }

        public async Task<HomeEntity> CreateHomeAsync(CreateHomeRequest request, Guid accountGuid)
        {
            var account = await _unitOfWork.AccountRepository.GetByGuidAsync(accountGuid);

            if (account is null)
            {
                throw new AccountNotFoundException($"No account found with the guid: {accountGuid}");
            }

            var homeEntity = new HomeEntity()
            {
                Name = request.Name,
                AccountId = account.Id
            };

            _unitOfWork.HomeRepository.Add(homeEntity);
            await _unitOfWork.CommitAsync();

            return homeEntity;
        }

        public async Task<HomeEntity> UpdateHomeAsync(UpdateHomeRequest request, int id, Guid accountGuid)
        {
            var account = await _unitOfWork.AccountRepository.GetByGuidAsync(accountGuid);

            if (account is null)
            {
                throw new AccountNotFoundException($"No account found with the guid: {accountGuid}");
            }

            HomeEntity? homeEntity = _unitOfWork.HomeRepository.GetById(id);


            if (homeEntity is null || homeEntity.AccountId != account.Id)
            {
                throw new Exception("No home with that id was found on that account.");
            }

            homeEntity.Name = request.Name;

            _unitOfWork.HomeRepository.Update(homeEntity);
            await _unitOfWork.CommitAsync();

            return homeEntity;
        }

        public async Task DeleteHomeAsync(int id, Guid accountGuid)
        {
            var account = await _unitOfWork.AccountRepository.GetByGuidAsync(accountGuid);

            if (account is null)
            {
                throw new AccountNotFoundException($"No account found with the guid: {accountGuid}");
            }

            HomeEntity? homeEntity = _unitOfWork.HomeRepository.GetById(id);

            if (homeEntity is null || homeEntity.AccountId != account.Id)
            {
                throw new Exception("No home with that id was found on that account.");
            }

            _unitOfWork.HomeRepository.Delete(homeEntity);
            await _unitOfWork.CommitAsync();
        }
    }
}
