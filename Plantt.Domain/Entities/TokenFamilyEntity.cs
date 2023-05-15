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

        [ForeignKey(nameof(Account))]
        [Column("FK_Account_Id")]
        public int AccountId { get; set; }
        public required AccountEntity Account { get; set; }
        public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();
    }
}