using Plantt.Domain.DTOs.Room;

namespace Plantt.Domain.DTOs.Home
{
    public class HomeDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<RoomDTO>? Rooms { get; set; }
    }
}
