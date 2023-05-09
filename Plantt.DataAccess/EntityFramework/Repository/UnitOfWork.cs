using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IAccountRepository? accountRepository;
        public IAccountRepository AccountRepository
        {
            get
            {
                if (accountRepository is null)
                {
                    accountRepository = new AccountRepository(_context);
                }
                return accountRepository;
            }
        }

        private IRefreshTokenRepository? refreshTokenRepository;
        public IRefreshTokenRepository RefreshTokenRepository
        {
            get
            {
                if (refreshTokenRepository is null)
                {
                    refreshTokenRepository = new RefreshTokenRepository(_context);
                }
                return refreshTokenRepository;
            }
        }

        private ITokenFamilyRepository? tokenFamilyRepository;
        public ITokenFamilyRepository TokenFamilyRepository
        {
            get
            {
                if (tokenFamilyRepository is null)
                {
                    tokenFamilyRepository = new TokenFamilyRepository(_context);
                }
                return tokenFamilyRepository;
            }
        }

        private readonly PlanttDbContext _context;
        private bool _disposed = false;

        public UnitOfWork(PlanttDbContext context)
        {
            _context = context;
        }


        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
