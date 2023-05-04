using Plantt.Domain.Entities;

namespace Plantt.Domain.DTOs.RefreshToken
{
    public class RefreshTokenDto
    {
        public required byte[] Token { get; set; }
        public required bool Used { get; set; }
        public required DateTime IssuedTS { get; set; }
        public required DateTime ExpirationTS { get; set; }
        public required TokenFamilyEntity TokenFamily { get; set; }
    }
}
