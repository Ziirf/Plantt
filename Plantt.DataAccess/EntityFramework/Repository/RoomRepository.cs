using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public class RoomRepository : GenericRepository<RoomEntity>, IRoomRepository
    {
        public RoomRepository(PlanttDBContext context) : base(context)
        {
        }
    }
}
