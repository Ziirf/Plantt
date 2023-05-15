using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("PlantWatering")]
    public class PlantWateringEntity
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public int? DaysInterval { get; set; }
    }
}
