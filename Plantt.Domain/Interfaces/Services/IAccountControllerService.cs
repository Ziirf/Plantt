using Plantt.Domain.DTOs.Requests;
using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services
{
    public interface IAccountControllerService
    {
        Task<AccountEntity> CreateNewAccountAsync(CreateAccountRequest accountRequest);
        Task<AccountEntity?> GetAccountByGuidAsync(Guid guid);
        Task<AccountEntity?> GetAccountByIdAsync(int id);
        Task<AccountEntity?> GetAccountByUsernameAsync(string username);
        bool VerifyPassword(AccountEntity account, string password);
    }
}