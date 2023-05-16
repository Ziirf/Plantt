using Microsoft.Extensions.Logging;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Services.EntityServices;

namespace Plantt.Applcation.Services.EntityServices
{
    public class PlantService : IPlantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PlantService> _logger;

        public PlantService(IUnitOfWork unitOfWork, ILogger<PlantService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<PlantEntity>?> GetPlantPage(int pagesize, int page)
        {
            try
            {
                IEnumerable<PlantEntity>? plants = await _unitOfWork.PlantRepository.GetPlantPageAsync(pagesize, page);

                return plants;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                throw;
            }
        }

        public async Task<PlantEntity?> GetPlantById(int id)
        {
            try
            {
                PlantEntity? plant = await _unitOfWork.PlantRepository.GetByIdAsync(id);

                return plant;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                throw;
            }
        }
    }
}
