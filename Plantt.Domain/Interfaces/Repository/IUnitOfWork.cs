using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository AccountRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        ITokenFamilyRepository TokenFamilyRepository { get; }

        void Commit();
        Task CommitAsync();
    }
}
