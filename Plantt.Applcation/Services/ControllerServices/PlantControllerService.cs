using Microsoft.Extensions.Logging;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Services;

namespace Plantt.Applcation.Services.ControllerServices
{
    public class PlantControllerService : IPlantControllerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PlantControllerService> _logger;

        public PlantControllerService(IUnitOfWork unitOfWork, ILogger<PlantControllerService> logger)
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
