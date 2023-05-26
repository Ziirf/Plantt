namespace Plantt.Domain.DTOs.Hub
{
    public class HubDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Identity { get; set; }
        public required string Secret { get; set; }
        public required string HomeName { get; set; }
        public required int HomeId { get; set; }
    }
}
