using Plantt.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("Plant")]
    public class PlantEntity : IEntity
    {
        public required int Id { get; set; }
        public required string LatinName { get; set; }
        public string? CommonName { get; set; }
        public string? FloralLanguage { get; set; }
        public required string Origin { get; set; }
        public required string Category { get; set; }
        public required string Blooming { get; set; }
        public required string Color { get; set; }
        public required string Soil { get; set; }
        public required string Fertilizer { get; set; }
        public required string Pruning { get; set; }
        public required string Sunlight { get; set; }
        public required string ImageUrl { get; set; }
        public required int MinLightMmol { get; set; }
        public required int MaxLightMmol { get; set; }
        public required int MinLightLux { get; set; }
        public required int MaxLightLux { get; set; }
        public required int MinTempCelsius { get; set; }
        public required int MaxTempCelsius { get; set; }
        public required int MinHumidity { get; set; }
        public required int MaxHumidity { get; set; }
        public required int MinMoisture { get; set; }
        public required int MaxMoisture { get; set; }
        public required int MinSoilEc { get; set; }
        public required int MaxSoilEc { get; set; }

        [ForeignKey(nameof(PlantWatering))]
        [Column("FK_PlantWatering_Id")]
        public required int PlantWateringId { get; set; }
        public PlantWateringEntity? PlantWatering { get; set; }

    }
}
