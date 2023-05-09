using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public class TokenFamilyRepository : GenericRepository<TokenFamilyEntity>, ITokenFamilyRepository
    {
        public TokenFamilyRepository(PlanttDbContext context) : base(context)
        {
        }
    }
}
