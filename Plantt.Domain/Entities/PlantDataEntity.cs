using Plantt.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("PlantData")]
    public class PlantDataEntity : IEntity
    {
        public int Id { get; set; }
        public required int Moisture { get; set; }
        public required int Lux { get; set; }
        public required int TemperatureC { get; set; }
        public required int Humidity { get; set; }
        public required DateTime CreatedTS { get; set; }

        [ForeignKey(nameof(AccountPlant))]
        [Column("FK_AccountPlant_Id")]
        public required int AccountPlantId { get; set; }
        public AccountPlantEntity? AccountPlant { get; set; }
    }
}
