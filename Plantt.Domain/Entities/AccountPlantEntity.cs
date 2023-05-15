using Plantt.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("AccountPlant")]
    public class AccountPlantEntity : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [ForeignKey(nameof(Room))]
        [Column("FK_Room_Id")]
        public int RoomId { get; set; }
        public RoomEntity? Room { get; set; }

        [ForeignKey(nameof(Plant))]
        [Column("FK_Plant_Id")]
        public int PlantId { get; set; }
        public PlantEntity? Plant { get; set; }
    }
}
