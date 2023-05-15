namespace Plantt.Domain.DTOs.RefreshToken
{
    public class RefreshTokenDTO
    {
        public required string Token { get; set; }
        public required DateTime IssuedTS { get; set; }
        public required DateTime ExpirationTS { get; set; }
    }
}
