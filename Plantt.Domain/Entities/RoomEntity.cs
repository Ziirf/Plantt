using Plantt.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("Room")]
    public class RoomEntity : IEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int SunlightLevel { get; set; }
        public required bool IsOutside { get; set; }

        [ForeignKey(nameof(Home))]
        [Column("FK_Home_Id")]
        public required int HomeId { get; set; }
        public HomeEntity? Home { get; set; }
        public ICollection<AccountPlantEntity> Plants { get; set; } = new List<AccountPlantEntity>();
    }
}
