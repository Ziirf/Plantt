using Plantt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IAccountRepository : IRepository<AccountEntity>
    {
        AccountEntity? GetByGuid(Guid guid);
        AccountEntity? GetByUsername(string username);
        Task<AccountEntity?> GetByGuidAsync(Guid guid);
        Task<AccountEntity?> GetByUsernameAsync(string username);
        bool DoesUsernameExist(string username);
    }
}
