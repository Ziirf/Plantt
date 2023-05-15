using Plantt.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("Hub")]
    public class HubEntity : IEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Identity { get; set; }
        public required string Secret { get; set; }
        [ForeignKey(nameof(Home))]
        [Column("FK_Home_Id")]
        public required int HomeId { get; set; }
        public HomeEntity? Home { get; set; }
    }
}
