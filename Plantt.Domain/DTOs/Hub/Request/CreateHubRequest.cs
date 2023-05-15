namespace Plantt.Domain.DTOs.Hub.Request
{
    public class CreateHubRequest
    {
        public required string Name { get; set; }
        public required int HomeId { get; set; }
    }
}
