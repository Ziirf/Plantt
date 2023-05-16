using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public class RoomRepository : GenericRepository<RoomEntity>, IRoomRepository
    {
        public RoomRepository(PlanttDBContext context) : base(context)
        {
        }
    }
}
