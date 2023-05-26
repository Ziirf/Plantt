namespace Plantt.Domain.DTOs.Room.Request
{
    public class UpdateRoomRequest
    {
        public required string Name { get; set; }
        public required int SunlightLevel { get; set; }
        public required bool IsOutside { get; set; }
        public required int HomeId { get; set; }
    }
}
