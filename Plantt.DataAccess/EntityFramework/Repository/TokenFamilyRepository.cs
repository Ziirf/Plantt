using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces.Repository;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public class TokenFamilyRepository : GenericRepository<TokenFamilyEntity>, ITokenFamilyRepository
    {
        public TokenFamilyRepository(PlanttDBContext context) : base(context)
        {
        }
    }
}
