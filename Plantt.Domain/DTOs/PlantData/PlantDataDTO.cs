namespace Plantt.Domain.DTOs.PlantData
{
    public class PlantDataDTO
    {
        public required int Moisture { get; set; }
        public required double Lux { get; set; }
        public required double TemperatureC { get; set; }
        public required double Humidity { get; set; }
        public required DateTime TimeStamp { get; set; }
    }
}
