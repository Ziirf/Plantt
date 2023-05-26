using Plantt.Domain.DTOs.AccountPlant;

namespace Plantt.Domain.DTOs.Room
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int SunlightLevel { get; set; }
        public required bool IsOutside { get; set; }
        public required int HomeId { get; set; }
        public IEnumerable<AccountPlantMinimumDTO> MyPlants { get; set; } = Enumerable.Empty<AccountPlantMinimumDTO>();
    }
}
