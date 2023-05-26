
namespace Plantt.Domain.DTOs.Sensor.Request
{
    public class UpdateSensorRequest
    {
        public required string Name { get; set; }
        public required int HubId { get; set; }
        public int? MyPlantId { get; set; }
    }
}
