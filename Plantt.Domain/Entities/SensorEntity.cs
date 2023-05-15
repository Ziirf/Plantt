using Plantt.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("Sensor")]
    internal class SensorEntity : IEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        [ForeignKey(nameof(AccountPlant))]
        [Column("FK_AccountPlant_Id")]
        public required int AccountPlantId { get; set; }
        public AccountPlantEntity? AccountPlant { get; set; }

        [ForeignKey(nameof(Hub))]
        [Column("FK_Hub_Id")]
        public required int HubId { get; set; }
        public HubEntity? Hub { get; set; }
    }
}
