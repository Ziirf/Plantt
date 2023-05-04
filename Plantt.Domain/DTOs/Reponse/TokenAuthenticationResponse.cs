using Plantt.Domain.DTOs.Account;

namespace Plantt.Domain.DTOs.Reponse
{
    public class TokenAuthenticationResponse
    {
        public required AccountDto Account { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
