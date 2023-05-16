namespace Plantt.Domain.DTOs.Room.Request
{
    public class CreateRoomRequest
    {
        public required string Name { get; set; }
        public int SunlightLevel { get; set; }
        public required bool IsInside { get; set; }
    }
}
