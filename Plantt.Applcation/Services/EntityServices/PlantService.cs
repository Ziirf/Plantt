using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Services.EntityServices;

namespace Plantt.Applcation.Services.EntityServices
{
    public class PlantService : IPlantService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PlantEntity>?> GetPlantPageAsync(int pagesize, int page, string? search = null)
        {
            return await _unitOfWork.PlantRepository.GetPlantPageAsync(pagesize, page, search);
        }

        public async Task<PlantEntity?> GetPlantByIdAsync(int id)
        {
            return await _unitOfWork.PlantRepository.GetByIdAsync(id);
        }
    }
}
