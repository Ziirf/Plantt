using Plantt.Domain.DTOs.Account.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Enums;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IAccountService
    {
        Task<AccountEntity?> GetAccountByIdAsync(int id);
        Task<AccountEntity?> GetAccountByUsernameAsync(string username);
        Task<AccountEntity> CreateNewAccountAsync(CreateAccountRequest accountRequest);
        Task<AccountEntity?> ChangeAccountRoleAsync(AccountEntity account, AccountRoles role);
        bool VerifyPassword(AccountEntity account, string password);
    }
}