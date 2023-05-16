using Plantt.Domain.DTOs.Home.Request;
using Plantt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.Domain.Interfaces.Services
{
    public interface IHomeControllerService
    {
        Task<HomeEntity> CreateHomeAsync(CreateHomeRequest request, Guid accountGuid);
        Task DeleteHomeAsync(int id, Guid accountGuid);
        Task<IEnumerable<HomeEntity>> GetAccountHomesAsync(Guid accountGuid);
        Task<HomeEntity?> GetAccountHomeByIdAsync(Guid accountGuid, int homeId);
        Task<HomeEntity> UpdateHomeAsync(UpdateHomeRequest request, int id, Guid accountGuid);
    }
}
