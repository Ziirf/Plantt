using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Plantt.DataAccess.EntityFramework;
using Plantt.Domain.DTOs.Requests;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;
using Plantt.Domain.Interfaces.Services;
using Plantt.Domain.Models;

namespace Plantt.Applcation.Services.ControllerServices
{
    public class AccountControllerService : IAccountControllerService
    {
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AccountControllerService(
            IPasswordService passwordService,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _passwordService = passwordService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountEntity?> GetAccountByGuidAsync(Guid guid)
        {
            return await _unitOfWork.AccountRepository.GetByGuidAsync(guid);
        }

        public async Task<AccountEntity?> GetAccountByIdAsync(int id)
        {
            return await _unitOfWork.AccountRepository.GetByIdAsync(id);
        }

        public async Task<AccountEntity?> GetAccountByUsernameAsync(string username)
        {
            return await _unitOfWork.AccountRepository.GetByUsernameAsync(username);
        }

        public async Task<AccountEntity> CreateNewAccountAsync(CreateAccountRequest accountRequest)
        {
            Password password = _passwordService.CreatePassword(accountRequest.Password);

            var newAccount = new AccountEntity()
            {
                Username = accountRequest.Username,
                HashedPassword = password.HashedPassword,
                Iterations = password.Iterations,
                Salt = password.Salt,
                Email = accountRequest.Email
            };

            await _unitOfWork.AccountRepository.AddAsync(newAccount);
            await _unitOfWork.CommitAsync();

            return newAccount;
        }

        public bool VerifyPassword(AccountEntity account, string password)
        {
            Password storedPassword = _mapper.Map<Password>(account);

            if (_passwordService.VerifyPassword(password, storedPassword))
            {
                return true;
            }

            return false;
        }
    }
}
