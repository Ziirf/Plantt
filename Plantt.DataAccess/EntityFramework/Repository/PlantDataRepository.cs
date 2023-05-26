using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public class PlantDataRepository : GenericRepository<PlantDataEntity>, IPlantDataRepository
    {
        public PlantDataRepository(PlanttDBContext context) : base(context)
        {
        }
    }
}
