namespace Plantt.Domain.DTOs.Hub.Request
{
    public class DataRequest
    {
        public int SensorId { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Lux { get; set; }
        public int Moisture { get; set; }
    }
}
