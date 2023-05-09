using Plantt.Domain.Enums;
using Plantt.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("TokenFamily")]
    public class TokenFamilyEntity : IEntity
    {
        public int Id { get; set; }
        public required string Identifier { get; set; }
        public DateTime? RevokeTS { get; set; }
        public TokenFamilyRevokeReason? RevokeReason { get; set; }

        [ForeignKey("Account")]
        public int FK_Account_Id { get; set; }
        public required AccountEntity Account { get; set; }
        public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();
    }
}