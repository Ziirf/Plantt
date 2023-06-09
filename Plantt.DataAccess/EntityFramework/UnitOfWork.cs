﻿using Plantt.DataAccess.EntityFramework.Repository;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Repository;

namespace Plantt.DataAccess.EntityFramework
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IAccountRepository? _accountRepository;
        public IAccountRepository AccountRepository
        {
            get
            {
                if (_accountRepository is null)
                {
                    _accountRepository = new AccountRepository(_context);
                }
                return _accountRepository;
            }
        }

        private IAccountPlantRepository? _accountPlantRepository;
        public IAccountPlantRepository AccountPlantRepository
        {
            get
            {
                if (_accountPlantRepository is null)
                {
                    _accountPlantRepository = new AccountPlantRepository(_context);
                }
                return _accountPlantRepository;
            }
        }

        private IRefreshTokenRepository? _refreshTokenRepository;
        public IRefreshTokenRepository RefreshTokenRepository
        {
            get
            {
                if (_refreshTokenRepository is null)
                {
                    _refreshTokenRepository = new RefreshTokenRepository(_context);
                }
                return _refreshTokenRepository;
            }
        }

        private ITokenFamilyRepository? _tokenFamilyRepository;
        public ITokenFamilyRepository TokenFamilyRepository
        {
            get
            {
                if (_tokenFamilyRepository is null)
                {
                    _tokenFamilyRepository = new TokenFamilyRepository(_context);
                }
                return _tokenFamilyRepository;
            }
        }

        private IPlantRepository? _plantRepository;
        public IPlantRepository PlantRepository
        {
            get
            {
                if (_plantRepository is null)
                {
                    _plantRepository = new PlantRepository(_context);
                }
                return _plantRepository;
            }
        }

        private IHubRepository? _hubRepository;
        public IHubRepository HubRepository
        {
            get
            {
                if (_hubRepository is null)
                {
                    _hubRepository = new HubRepository(_context);
                }
                return _hubRepository;
            }
        }

        private ISensorRepository? _sensorRepository;
        public ISensorRepository SensorRepository
        {
            get
            {
                if (_sensorRepository is null)
                {
                    _sensorRepository = new SensorRepository(_context);
                }
                return _sensorRepository;
            }
        }

        private IPlantDataRepository? _plantDataRepository;
        public IPlantDataRepository PlantDataRepository
        {
            get
            {
                if (_plantDataRepository is null)
                {
                    _plantDataRepository = new PlantDataRepository(_context);
                }
                return _plantDataRepository;
            }
        }

        private IHomeRepository? _homeRepository;
        public IHomeRepository HomeRepository
        {
            get
            {
                if (_homeRepository is null)
                {
                    _homeRepository = new HomeRepository(_context);
                }
                return _homeRepository;
            }
        }

        private IRoomRepository? _roomRepository;
        public IRoomRepository RoomRepository
        {
            get
            {
                if (_roomRepository is null)
                {
                    _roomRepository = new RoomRepository(_context);
                }
                return _roomRepository;
            }
        }

        private readonly PlanttDBContext _context;
        private bool _disposed = false;

        public UnitOfWork(PlanttDBContext context)
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
