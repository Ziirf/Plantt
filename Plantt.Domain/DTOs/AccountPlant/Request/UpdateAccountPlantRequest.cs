namespace Plantt.Domain.DTOs.AccountPlant.Request
{
    public class UpdateAccountPlantRequest
    {
        public required string Name { get; set; }
        public required int RoomId { get; set; }
        public required int PlantId { get; set; }

    }
}
