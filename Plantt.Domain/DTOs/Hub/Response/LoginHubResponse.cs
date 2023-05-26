namespace Plantt.Domain.DTOs.Hub.Response
{
    public class LoginHubResponse
    {
        public required long Expire { get; set; }
        public required string AccessToken { get; set; }
    }
}
