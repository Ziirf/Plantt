using Plantt.Domain.DTOs.RefreshToken;

namespace Plantt.Domain.DTOs.Account.Response
{
    public class TokenAuthenticationResponse
    {
        public required AccountDTO Account { get; set; }
        public required string AccessToken { get; set; }
        public required RefreshTokenDTO RefreshToken { get; set; }
    }
}
