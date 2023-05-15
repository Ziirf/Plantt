using Microsoft.EntityFrameworkCore;
using Plantt.Domain.Entities;

namespace Plantt.DataAccess.EntityFramework
{
    public class PlanttDBContext : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<TokenFamilyEntity> TokenFamilies { get; set; }
        public DbSet<HomeEntity> Homes { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<HubEntity> Hubs { get; set; }
        public DbSet<PlantEntity> Plants { get; set; }
        public DbSet<PlantWateringEntity> PlantWartering { get; set; }

        public PlanttDBContext(DbContextOptions<PlanttDBContext> options)
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
