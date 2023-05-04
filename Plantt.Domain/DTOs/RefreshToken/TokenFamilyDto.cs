using Plantt.Domain.Entities;

namespace Plantt.Domain.DTOs.RefreshToken
{
    public class TokenFamilyDto
    {
        public required byte[] Identifier { get; set; }
        public DateTime? RevokeTs { get; set; }
        public int? RevokeReason { get; set; }
        public required AccountEntity Account { get; set; }
        public required ICollection<RefreshTokenEntity> RefreshTokens { get; set; }
    }
}
