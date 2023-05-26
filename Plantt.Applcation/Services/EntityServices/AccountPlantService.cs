using AutoMapper;
using Plantt.Domain.DTOs.AccountPlant.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Exceptions;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Services.EntityServices;

namespace Plantt.Applcation.Services.EntityServices
{
    public class AccountPlantService : IAccountPlantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountPlantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AccountPlantEntity?> GetPlantByIdAsync(int accountPlantId)
        {
            return await _unitOfWork.AccountPlantRepository.GetPlantWithDataPointsAsync(accountPlantId);
        }

        public IEnumerable<AccountPlantEntity> GetAccountPlantsFromRoom(int roomId)
        {
            return _unitOfWork.AccountPlantRepository.GetAllFromRoom(roomId);
        }

        public IEnumerable<AccountPlantEntity> GetAccountPlantsFromAccount(int accountId)
        {
            return _unitOfWork.AccountPlantRepository.GetAllFromAccount(accountId);
        }

        public async Task<AccountPlantEntity> CreateAccountPlantAsync(UpdateAccountPlantRequest request)
        {
            var accountPlant = _mapper.Map<AccountPlantEntity>(request);

            await _unitOfWork.AccountPlantRepository.AddAsync(accountPlant);
            await _unitOfWork.CommitAsync();

            accountPlant.Plant = _unitOfWork.PlantRepository.GetById(accountPlant.PlantId);

            return accountPlant;
        }

        public async Task<AccountPlantEntity> UpdateAccountPlantAsync(UpdateAccountPlantRequest requst, int accountPlantId)
        {
            var accountPlant = await GetPlantByIdAsync(accountPlantId);

            if (accountPlant is null)
            {
                throw new NoEntryFoundException(nameof(accountPlant));
            }

            accountPlant.Name = requst.Name;
            accountPlant.RoomId = requst.RoomId;
            accountPlant.PlantId = requst.PlantId;

            _unitOfWork.AccountPlantRepository.Update(accountPlant);
            await _unitOfWork.CommitAsync();

            accountPlant.Plant = _unitOfWork.PlantRepository.GetById(accountPlant.PlantId);

            return accountPlant;
        }

        public async Task<bool> DeleteAccountPlantAsync(int accountPlantId)
        {
            var accountPlant = await _unitOfWork.AccountPlantRepository.GetByIdAsync(accountPlantId);

            if (accountPlant is null)
            {
                return false;
            }

            _unitOfWork.AccountPlantRepository.Delete(accountPlant);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> ValidateOwnerAsync(int accountPlantId, int accountId)
        {
            return await _unitOfWork.AccountPlantRepository.IsValidOwnerAsync(accountPlantId, accountId);
        }
    }
}
