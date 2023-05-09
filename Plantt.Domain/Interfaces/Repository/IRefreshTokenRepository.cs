﻿using Plantt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.Domain.Interfaces.Repository
{
    public interface IRefreshTokenRepository : IRepository<RefreshTokenEntity>
    {
        Task<RefreshTokenEntity?> GetByRefreshTokenAsync(string refreshToken);
    }
}