using Plantt.Domain.DTOs.AccountPlant;
namespace Plantt.Domain.DTOs.Sensor
{
    public class SensorDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int HubId { get; set; }
        public AccountPlantMinimumDTO? MyPlant { get; set; }
    }
}
