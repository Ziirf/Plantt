using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Plantt.DataAccess.EntityFramework;
using Plantt.Domain.DTOs.Requests;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Services;
using Plantt.Domain.Models;

namespace Plantt.Applcation.Services.ControllerServices
{
    public class AccountControllerService : IAccountControllerService
    {
        private readonly PlanttDbContext _planttDb;
        private readonly ITokenAuthenticationService _tokenAuthenticationService;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;

        public AccountControllerService(
            PlanttDbContext planttDb,
            ITokenAuthenticationService tokenAuthenticationService,
            IPasswordService passwordService,
            IMapper mapper)
        {
            _planttDb = planttDb;
            _tokenAuthenticationService = tokenAuthenticationService;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        public AccountEntity[] GetAllAccounts()
        {
            return _planttDb.Accounts.ToArray();
        }

        public async Task<AccountEntity?> GetAccountByGuidAsync(Guid guid)
        {
            AccountEntity? account = await _planttDb.Accounts.FirstOrDefaultAsync();

            return account;
        }

        public async Task<AccountEntity?> GetAccountByUsernameAsync(string username)
        {
            AccountEntity? account = await _planttDb.Accounts.FirstOrDefaultAsync(account => account.Username == username);

            return account;
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

            await _planttDb.Accounts.AddAsync(newAccount);
            await _planttDb.SaveChangesAsync();

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
