using Microsoft.EntityFrameworkCore;
using Plantt.Domain.Entities;

namespace Plantt.DataAccess.EntityFramework
{
    public class PlanttDBContext : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<AccountPlantEntity> AccountPlants { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<TokenFamilyEntity> TokenFamilies { get; set; }
        public DbSet<HomeEntity> Homes { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<HubEntity> Hubs { get; set; }
        public DbSet<SensorEntity> Sensor { get; set; }
        public DbSet<PlantDataEntity> PlantData { get; set; }
        public DbSet<PlantEntity> Plants { get; set; }
        public DbSet<PlantWateringEntity> PlantWartering { get; set; }

        public PlanttDBContext(DbContextOptions<PlanttDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HomeEntity>()
                .ToTable(tb => tb.HasTrigger("trg_Home_Delete"));

            modelBuilder.Entity<HubEntity>()
                .ToTable(tb => tb.HasTrigger("trg_Hub_Delete"));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
