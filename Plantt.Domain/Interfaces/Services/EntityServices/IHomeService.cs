using Plantt.Domain.DTOs.Home.Request;
using Plantt.Domain.Entities;

namespace Plantt.Domain.Interfaces.Services.EntityServices
{
    public interface IHomeService
    {
        Task<HomeEntity> CreateHomeAsync(CreateHomeRequest request, Guid accountGuid);
        Task DeleteHomeAsync(int id, Guid accountGuid);
        Task<IEnumerable<HomeEntity>> GetAccountHomesAsync(Guid accountGuid);
        Task<HomeEntity?> GetAccountHomeByIdAsync(Guid accountGuid, int homeId);
        Task<HomeEntity> UpdateHomeAsync(UpdateHomeRequest request, int id, Guid accountGuid);
    }
}
