using Plantt.Domain.DTOs.Account.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Enums;

namespace Plantt.Domain.Interfaces.Services
{
    public interface IAccountControllerService
    {
        Task<AccountEntity> CreateNewAccountAsync(CreateAccountRequest accountRequest);
        Task<AccountEntity?> GetAccountByGuidAsync(Guid guid);
        Task<AccountEntity?> GetAccountByIdAsync(int id);
        Task<AccountEntity?> GetAccountByUsernameAsync(string username);
        Task<AccountEntity?> Upgrade(Guid publicId, AccountRoles role);
        bool VerifyPassword(AccountEntity account, string password);
    }
}