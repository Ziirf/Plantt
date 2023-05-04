using Microsoft.EntityFrameworkCore;
using Plantt.Domain.Entities;

namespace Plantt.DataAccess.EntityFramework
{
    public class PlanttDbContext : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<TokenFamilyEntity> TokenFamilies { get; set; }

        public PlanttDbContext(DbContextOptions<PlanttDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
