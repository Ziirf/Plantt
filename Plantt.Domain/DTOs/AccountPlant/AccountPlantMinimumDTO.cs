using Plantt.Domain.DTOs.Plant;

namespace Plantt.Domain.DTOs.AccountPlant
{
    public class AccountPlantMinimumDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required PlantMinimumDTO Plant { get; set; }
        public required int RoomId { get; set; }
    }
}
