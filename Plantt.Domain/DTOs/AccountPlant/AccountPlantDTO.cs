using Plantt.Domain.DTOs.Plant;
using Plantt.Domain.DTOs.PlantData;

namespace Plantt.Domain.DTOs.AccountPlant
{
    public class AccountPlantDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required PlantDTO Plant { get; set; }
        public required int RoomId { get; set; }
        public required IEnumerable<PlantDataDTO>? Data { get; set; }
    }
}
