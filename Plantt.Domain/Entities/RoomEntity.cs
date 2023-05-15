using Plantt.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("Room")]
    public class RoomEntity : IEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [ForeignKey(nameof(Home))]
        [Column("FK_Home_Id")]
        public int HomeId { get; set; }
        public required HomeEntity Home { get; set; }
    }
}
