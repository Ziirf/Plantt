namespace Plantt.Domain.DTOs.PlantData.Request
{
    public class SendDataRequest
    {
        public required int SensorId { get; set; }
        public required double Temperature { get; set; }
        public required double Humidity { get; set; }
        public required double Lux { get; set; }
        public required int Moisture { get; set; }
        public required long TimeStamp { get; set; }
    }
}
