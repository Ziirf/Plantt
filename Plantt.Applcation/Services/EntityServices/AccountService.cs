using AutoMapper;
using Plantt.Domain.DTOs.Account;
using Plantt.Domain.DTOs.Account.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Enums;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Services;
using Plantt.Domain.Interfaces.Services.EntityServices;

namespace Plantt.Applcation.Services.EntityServices
{
    public class AccountService : IAccountService
    {
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(
            IPasswordService passwordService,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _passwordService = passwordService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountEntity?> GetAccountByIdAsync(int id)
        {
            throw new NotImplementedException();

            return await _unitOfWork.AccountRepository.GetByIdAsync(id);
        }

        public async Task<AccountEntity?> GetAccountByUsernameAsync(string username)
        {
            return await _unitOfWork.AccountRepository.GetByUsernameAsync(username);
        }

        public async Task<AccountEntity> CreateAccountAsync(CreateAccountRequest accountRequest)
        {
            PasswordDTO password = _passwordService.CreatePassword(accountRequest.Password);

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

        public async Task<AccountEntity?> ChangeAccountRoleAsync(AccountEntity account, AccountRoles role)
        {
            account.Role = role;
            _unitOfWork.AccountRepository.Update(account);
            await _unitOfWork.CommitAsync();

            return account;
        }

        public async Task DeleteAccount(AccountEntity account)
        {
            _unitOfWork.AccountRepository.Delete(account);
            await _unitOfWork.CommitAsync();
        }

        public bool VerifyPassword(AccountEntity account, string password)
        {
            PasswordDTO storedPassword = _mapper.Map<PasswordDTO>(account);

            if (_passwordService.VerifyPassword(password, storedPassword))
            {
                return true;
            }

            return false;
        }
    }
}
