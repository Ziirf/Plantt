namespace Plantt.Domain.DTOs.Hub.Request
{
    public class LoginHubRequest
    {
        public required string Identity { get; set; }
        public required string Secret { get; set; }
    }
}
