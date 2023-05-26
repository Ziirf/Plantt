﻿using Plantt.Domain.DTOs.Home.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Exceptions;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Services.EntityServices;

namespace Plantt.Applcation.Services.EntityServices
{
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<HomeEntity> GetAccountHomes(int accountId)
        {
            return _unitOfWork.HomeRepository.GetAccountHome(accountId).ToList();
        }

        public async Task<HomeEntity?> GetAccountHomeByIdAsync(int accountId, int homeId)
        {
            return await _unitOfWork.HomeRepository.GetAccountHomeByIdAsync(accountId, homeId);
        }

        public async Task<HomeEntity> CreateHomeAsync(UpdateHomeRequest request, int accountId)
        {
            var homeEntity = new HomeEntity()
            {
                Name = request.Name,
                AccountId = accountId
            };

            _unitOfWork.HomeRepository.Add(homeEntity);

            await _unitOfWork.CommitAsync();

            return homeEntity;
        }

        public async Task<HomeEntity> UpdateHomeAsync(UpdateHomeRequest request, int id)
        {
            HomeEntity? homeEntity = _unitOfWork.HomeRepository.GetById(id);

            if (homeEntity is null)
            {
                throw new NoEntryFoundException("No home with that id was found.");
            }

            homeEntity.Name = request.Name;

            _unitOfWork.HomeRepository.Update(homeEntity);

            await _unitOfWork.CommitAsync();

            return homeEntity;
        }

        public async Task DeleteHomeAsync(int homeId)
        {
            HomeEntity? homeEntity = await _unitOfWork.HomeRepository.GetByIdAsync(homeId);

            if (homeEntity is null)
            {
                throw new NoEntryFoundException("No home with that id was found.");
            }

            _unitOfWork.HomeRepository.Delete(homeEntity);

            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> ValidateOwnerAsync(int homeId, int accountId)
        {
            return await _unitOfWork.HomeRepository.IsValidOwnerAsync(homeId, accountId);
        }
    }
}
