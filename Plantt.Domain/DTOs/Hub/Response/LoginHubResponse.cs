namespace Plantt.Domain.DTOs.Hub.Response
{
    public class LoginHubResponse
    {
        public required DateTime ExpireTs { get; set; }
        public required string accessToken { get; set; }
    }
}
