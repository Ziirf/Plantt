namespace Plantt.Domain.DTOs.Hub.Request
{
    public class RegisterHubRequest
    {
        public required string Name { get; set; }
        public required int HomeId { get; set; }
    }
}
